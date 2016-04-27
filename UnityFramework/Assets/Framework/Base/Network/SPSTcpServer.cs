using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using SPSGame.CsShare;
using SPSGame.Tools;

namespace SPSGame
{
    public class SPSTcpServer : SPSServerBase, IDisposable
    {
        private NetReader m_reader = null;
        public NetReader Reader 
        {
            get
            {
                return m_reader;
            }
        }
        private NetWriterTcp m_writer = new NetWriterTcp();
        public NetWriterTcp Writer
        {
            get
            {
                return m_writer;
            }
        }

        private short _ServerID = 0;
        public short ServerID
        {
            get
            {
                return _ServerID;
            }
        }
        /// <summary>
        /// 连接成功的Socket
        /// </summary>
        private Socket _Socket = null;
        private string m_Address = "";
        private int m_Port = 0;

        private int totalSendBytes = 0;
        private int totalRecvBytes = 0;

        /// <summary>
        /// 保存接收到的数据包
        /// </summary>
        private CircleBuffer _packRcvPool = null;
        private byte[] _packRcvBuff = null;

        private IHeadFormater HeadFormater { get; set; }

        /// <summary>
        /// 连接超时检测定时器
        /// </summary>
        private System.Timers.Timer timerConnectTimeout;

        private const int HearInterval = 10000; //10秒
        private System.Threading.Timer _heartbeatThread = null;
        private byte[] _hearbeatPackage = null;

        private int _flaglen = sizeof(byte) * 2;     //PACKET_START1,PACKET_START2
        private int _headlen = 0;
        private int _minlen = 0;   // 每个完整包的最小长度

        private readonly Queue<NetPackage> m_sendQueue = new Queue<NetPackage>();

        private int _rcvTimeout = 5000;        //_Socket.ReceiveTimeout
        private int _sendTimeOut = 5000;       //_Socket.SendTimeout

        public void Dispose()
        {
            //Dispose(true);
            _packRcvPool = null;
            _packRcvBuff = null;
            m_sendQueue.Clear();

            GC.SuppressFinalize(this);
        }

        public bool Connected()
        {
            if (null == _Socket)
                return false;
            if (! _Socket.Connected)
                return false;
            if (!_Socket.Poll(1000, SelectMode.SelectWrite))
                return false;
            return true;
        }

        public bool ResetSocket()
        {
            try
            {
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _Socket.ReceiveTimeout = _rcvTimeout;
                _Socket.SendTimeout = _sendTimeOut;
            }
            catch(Exception e)
            {
                DebugMod.LogException(e);
                return false;
            }
            return true;
        }

        public bool Create(string address, int port, int rcvTimeout = 5000, int sendTimeOut = 5000, int connTimeout = 5000, int maxPackSize = (int)TCPClientEnum.MAX_PACKET_SIZE)
        {
            try
            {
                SetRemote(address, port);

                _packRcvPool = new CircleBuffer(maxPackSize);
                _packRcvBuff = new byte[(int)TcpCommonEnum.SOCKET_BUF_SIZE_SERVER_SEND];

                timerConnectTimeout = new System.Timers.Timer(connTimeout);
                timerConnectTimeout.AutoReset = false;
                timerConnectTimeout.Elapsed += new ElapsedEventHandler(CheckConnectTimeout);
                timerConnectTimeout.Interval = (Double)connTimeout;
                timerConnectTimeout.Stop();

                //开始发送心跳消息
                _hearbeatPackage = BuildHearbeatPackage();

            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }
            return true;
        }

        public override void OnInit()
        {
            HeadFormater = new DefaultHeadFormater();
            m_reader = new NetReader(HeadFormater);

            int actIdLen = 4;      //ActionId的长度
            int svrIdLen = 2;    //ServerId的长度
            int lenlen = 4;       //长度的长度
            int dataFmtLen = 1;   //压缩标志的长度

            _headlen = _flaglen + dataFmtLen + actIdLen + svrIdLen + lenlen;
            _minlen = _headlen + _flaglen;   // 每个完整包的最小长度
        }

        public override void OnUpdate()
        {
            DoAllNetEvent();

            bool busy = true;

            while (busy)
            {
                busy = false;
                //处理收到的服务器消息，每次最多一条
                NetPackage packRcv = this.PullOutPackage();
                if (packRcv != null)
                {
                    busy = true;
                    //DebugMod.Log("收到服务器消息：" + packRcv.ActionId);
                    ProcessPackage(packRcv);
                }

                //发送准备发往服务器的消息，每次最多一条
                NetPackage packSend = DequeueSend();
                if (packSend != null)
                {
                    busy = true;
                    this.SendPackage(packSend);
                }
            }
        }

        public override void OnApplicationQuit()
        {
            DebugMod.Log("OnApplicationQuit，断开连接");
            this.Close();
            if (timerConnectTimeout != null)
            {
                timerConnectTimeout.Stop();
                timerConnectTimeout = null;
            }
            if (_heartbeatThread != null)
            {
                _heartbeatThread.Dispose();
                _heartbeatThread = null;
            }
        }

        /// <summary>
        /// 将待发送的消息压入队列
        /// </summary>
        public bool ReadySend(CsShare.SPSCmd cmdType, ActionParam actionParam, object UserData)
        {
            try
            {
                NetAction action = (NetAction)NetActionFactory.Instance.CreateAction(cmdType);
                if (null == action)
                {
                    DebugMod.LogError("Create NetAction Fail, SPSCmd=" + cmdType);
                    return false;
                }

                NetPackage package = new NetPackage();
                package.Action = action;
                package.ActionId = (int)cmdType;
                package.ActionParam = actionParam;
                package.UserData = UserData;

                lock (m_sendQueue)
                {
                    m_sendQueue.Enqueue(package);
                }
            }
            catch(Exception e)
            {
                DebugMod.LogException(e);
            }
            return true;
        }

        public NetPackage DequeueSend()
        {
            NetPackage package = null;
            lock(m_sendQueue)
            {
                if (0 < m_sendQueue.Count)
                    package = m_sendQueue.Dequeue();
            }
            return package;
        }


        public void SetRemote(string address, int port)
        {
            m_Address = address;
            m_Port = port;
        }

        public void SetIPAddress(string address)
        {
            m_Address = address;
        }

        public int Connect(short serverID)
        {
            if (_Socket != null)
            {
                DebugMod.Log("发起新的连接前先断开原有连接");
                Close();
                ResetSocket();
            }
            else
            {
                ResetSocket();
            }

            if (null == _Socket)
                return 0;

            _ServerID = serverID;
      
            try
            {
                DebugMod.Log(GetRemoteEndPoint() + " : Socket Connect...");

                IPAddress[] ipArray = System.Net.Dns.GetHostAddresses(m_Address);
                IPAddress ipAddress = ipArray[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, m_Port);

                timerConnectTimeout.Start();

                _Socket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnected), null);

                return 1;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return 0;
        }

        /// <summary>
        /// 检测连接是否超时
        /// </summary>
        private void CheckConnectTimeout(object source, ElapsedEventArgs e)
        {
            try
            {
                //if (null != timerConnectTimeout)
                //{
                //    timerConnectTimeout.Stop();
                //}
                DebugMod.Log(GetRemoteEndPoint() + ":ConnectTimeout");
                ProcessNetEvent(new SPSNetEventArgs() { ServerName = this.ServerName, RemoteEndPoint = GetRemoteEndPoint(), eventType = NetEventType.Connect_Timeout });
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public string GetRemoteEndPoint()
        {
            try
            {
                return string.Format("{0}:{1}", m_Address, m_Port);
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return null;
        }

        private void OnConnected(IAsyncResult iar)
        {
            try
            {
                if (null != timerConnectTimeout)
                {
                    timerConnectTimeout.Stop();
                }

                if (null == _Socket)
                {
                    return;
                }

                // Complete the connection.
                _Socket.EndConnect(iar);

                DebugMod.Log(GetRemoteEndPoint() + "Socket Connected Complete");

                _Socket.BeginReceive(_packRcvBuff, 0, _packRcvBuff.Length, SocketFlags.None, new AsyncCallback(OnSockRcv), _packRcvBuff);

                //连接成功的事件
                ProcessNetEvent(new SPSNetEventArgs() { ServerName = this.ServerName, RemoteEndPoint = GetRemoteEndPoint(), eventType = NetEventType.Connect_Success });

                _heartbeatThread = new System.Threading.Timer(SendHeartbeatPackage, null, HearInterval, HearInterval);
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }
        }

        //public virtual int DoServerNetEvent(SPSNetEventArgs args)
        //{
        //    return 0;
        //}

        private void OnSockRcv(IAsyncResult iar)
        {
            try
            {
                if (null == _Socket)
                {
                    return;
                }

                SocketError socketError = SocketError.Success;
                int recvLength = 0;
                byte[] bytesData = null;
                if (_Socket.Connected && iar != null)  //判断是否是connected，避免被close后，抛出System.ObjectDisposedException: The object was used after being disposed异常
                {
                    recvLength = _Socket.EndReceive(iar, out socketError);
                    bytesData = iar.AsyncState as byte[];
                }

                //DebugMod.Log(GetRemoteEndPoint() + "Socket Rcv Length=" + recvLength);

                if (recvLength <= 0 || null == bytesData)
                {
                    DebugMod.Log("断开连接");
                    //主动断开
                    Close();
                    return;
                }

                //DebugMod.Log("收到数据,len:" + recvLength);
                PushRcvData(bytesData, 0, recvLength);

                _Socket.BeginReceive(bytesData, 0, bytesData.Length, SocketFlags.None, new AsyncCallback(OnSockRcv), bytesData);

                return;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
                ProcessNetEvent(new SPSNetEventArgs() { ServerName = this.ServerName, RemoteEndPoint = GetRemoteEndPoint(), eventType = NetEventType.Recv_Fail });
            }
        }

        //sock子线程收到数据时调用，把数据填充到_packRcvPool
        public void PushRcvData(byte[] bytesData, int offset, int recvLength)
        {
            if (offset + recvLength > bytesData.Length)
            {
                return;
            }

            lock (_packRcvPool)
            // 统计接收字节数
            {
                totalRecvBytes += recvLength;

                //把收到的数据压到循环缓冲里
                _packRcvPool.PutData(bytesData, offset, recvLength);
            }
        }

        public byte[] BuildHearbeatPackage()
        {
            byte[] hbdata = null;
            try
            {
                Writer.writeHeader(_ServerID, (int)SPSCmd.CMD_HEARTBEAT);
                int len = 0;
                byte[] postdata = Writer.PostData(out len);
                if (null == postdata)
                    return null;
                hbdata = new byte[len];
                Buffer.BlockCopy(postdata, 0, hbdata, 0, len);
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }
            return hbdata;
        }

        private void SendHeartbeatPackage(object state)
        {
            return;
            try
            {
                if (_hearbeatPackage == null)
                    return;

                if (!PostSend(_hearbeatPackage, _hearbeatPackage.Length))
                {
                    DebugMod.LogError("send heartbeat package fail");
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (_Socket == null) 
                return;
            try
            {
                lock (this)
                {
                    _Socket.Shutdown(SocketShutdown.Both);
                    _Socket.Close();
                    _Socket = null;

                    ProcessNetEvent(new SPSNetEventArgs() { ServerName = this.ServerName, RemoteEndPoint = GetRemoteEndPoint(), eventType = NetEventType.Connect_Close });

                    _heartbeatThread.Dispose();
                    _heartbeatThread = null;
                }

            }
            catch (Exception)
            {
                _Socket = null;
                ProcessNetEvent(new SPSNetEventArgs() { ServerName = this.ServerName, RemoteEndPoint = GetRemoteEndPoint(), eventType = NetEventType.Connect_Close_Fail });
                throw;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        private bool PostSend(byte[] data, int length)
        {
            try
            {
                if (_Socket == null)
                    return false;

                IAsyncResult asyncSend = _Socket.BeginSend(data, 0, length, SocketFlags.None, new AsyncCallback(sendCallback), _Socket);
                bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
                if (!success)
                {
                    DebugMod.Log("asyncSend error close socket");
                    Close();
                    return false;
                }

                totalSendBytes += length;
                return true;
            }
            catch (Exception)
            {
                DebugMod.LogError("PostSend异常，断开连接");
                Close();
                //DebugMod.LogException(e);
                return false;
            }
        }

        private void sendCallback(IAsyncResult asyncSend)
        {
        }

        /// <summary>
        /// 立刻发送消息到服务器 
        /// </summary>
        public bool SendPackage(NetPackage package)
        {
            try
            {
                if (null == package || package.Action == null)
                {
                    //throw new ArgumentException(string.Format("Not found {0} of NetAction object.", package.ActionId));
                    return false;
                }

                //package.Action.Head.MsgId = this.Writer.MsgId - 1;
                package.SendTime = DateTime.Now;

                this.Writer.writeHeader(_ServerID, package.ActionId);

                package.Action.SendParameterTcp(Writer, package.ActionParam);

                int length = 0;
                byte[] data = Writer.PostData(out length);
                if (data == null || length <= 0)
                {
                    return false;
                }

                bool bSuccess = PostSend(data, length); ;
                //DebugMod.Log("Socket send actionId:" + package.ActionId + ", msgId:" + package.MsgId);
                return bSuccess;
            }
            catch (Exception ex)
            {
                DebugMod.LogError("Socket send actionId: " + package.ActionId + " error" + ex);
                return false;
            }

        }


        public NetPackage PullOutPackage()
        {
            try
            {
                Reader.Reset();
                bool isZipData = false;
                int packlen = 0;
                int actionId = 0;
                short serverId = -1;
                lock (_packRcvPool)
                {
                    packlen = PullOutCore(Reader.ParseBuff, Reader.ParseBuffZip, Reader.ParseBuffZip.Length,
                        out isZipData, out actionId, out serverId);
                }

                if (packlen < 0)
                    return null;        //未收到消息或未收到完整消息

                if (isZipData && packlen > 0)  //是否需要解压缩
                {
                    int declen = 0;
                    if (!NetReader.Decompression(Reader.ParseBuffZip, Reader.ParseBuff, out declen))
                        return null;
                    Reader.PackParseLen = declen;
                }
                else
                {
                    Reader.PackParseLen = packlen;
                }

                NetPackage package = new NetPackage();
                package.ActionId = actionId;
                package.ServerID = serverId;
                package.Action = (NetAction)NetActionFactory.Instance.CreateAction((SPSCmd)package.ActionId);
                package.DataReader = this.Reader;
                return package;
            }
            catch(Exception ex)
            {
                DebugMod.LogException(ex);
                return null;
            }
        }

        //返回-1表示未收到消息或未收到完整消息
        //返回值代表除了消息固定信息外的额外数据长度，返回值0代表没有额外数据，但也是有效的消息
        public int PullOutCore(byte[] byteDest, byte[] zipDest, int bufflen, out bool bIsZipdata, out int ActionId, out short ServerId)
        {
            bIsZipdata = false;
            ActionId = 0;
            ServerId = -1;
            try
            {
                if (bufflen != (int)TcpCommonEnum.SOCKET_BUF_SIZE_SERVER_SEND)
                    return -1;

                int total_len = _packRcvPool.GetValidCount();
                if (total_len <= _minlen)
                    return -1;

                int dataindex = 0;
                //flag1,flag2///////////////////////////////////////////////////////////////////////
                byte[] aFlag = {0,0};
                for (int i = 0; i < aFlag.Length; i++)
                {
                    if (!_packRcvPool.GetByte(dataindex + i, out aFlag[i]))
                    {
                        _packRcvPool.SetEmpty();
                        DebugMod.LogError("PullOutCore", "invalid flag " + i);
                        return -1;
                    }
                }

                //check start flag
                if (aFlag[0] != (byte)TcpCommonEnum.PACKET_START1 || aFlag[1] != (byte)TcpCommonEnum.PACKET_START2)
                {
                    _packRcvPool.SetEmpty();
                    DebugMod.LogError("PullOutCore", "invalid start flag , data[0]" + aFlag[0] + " data[1]" + aFlag[1]);
                    return -1;
                }

                dataindex += aFlag.Length;

                //actionid//////////////////////////////////////////////////////////////////////////////////////////
                byte[] aActId = { 0, 0, 0, 0 };
                for (int i = 0; i < aActId.Length; i++)
                {
                    if (!_packRcvPool.GetByte(dataindex + i, out aActId[i]))
                    {
                        _packRcvPool.SetEmpty();
                        DebugMod.LogError("PullOutCore", "invalid ActionId");
                        return -1;
                    }
                }
                ActionId = BitConverter.ToInt32(aActId, 0);

                dataindex += aActId.Length;

                //ServerId//////////////////////////////////////////////////////////////////////////////////////////
                byte[] aSvrId = { 0, 0 };
                for (int i = 0; i < aSvrId.Length; i++)
                {
                    if (!_packRcvPool.GetByte(dataindex + i, out aSvrId[i]))
                    {
                        _packRcvPool.SetEmpty();
                        DebugMod.LogError("PullOutCore", "invalid ServerId");
                        return -1;
                    }
                }
                ServerId = BitConverter.ToInt16(aActId, 0);

                dataindex += aSvrId.Length;

                //data format(is zip?)/////////////////////////////////////////////////////////////////////////
                byte dataFmt = 0;
                if (!_packRcvPool.GetByte(dataindex, out dataFmt))
                {
                    _packRcvPool.SetEmpty();
                    DebugMod.LogError("PullOutCore", "invalid dataFmt");
                    return -1;
                }

                bIsZipdata = dataFmt == 1 ? true : false;

                dataindex += 1;

                //length////////////////////////////////////////////////////////////////////////////////////////////
                byte[] aLen = { 0, 0, 0, 0 };
                for (int i = 0; i < aLen.Length; i++)
                {
                    if (!_packRcvPool.GetByte(dataindex + i, out aLen[i]))
                    {
                        _packRcvPool.SetEmpty();
                        DebugMod.LogError("PullOutCore", "invalid pack length");
                        return -1;
                    }
                }

                //read data length
                int length = BitConverter.ToInt32(aLen, 0);

                //check data length
                if (length < 0 || length > (int)TcpCommonEnum.SOCKET_BUF_SIZE_SERVER_SEND)
                {
                    //	assert(0);
                    DebugMod.LogError("PullOutCore", "invalid length : " + length);
                    _packRcvPool.SetEmpty();
                    return -1;
                }

                dataindex += aLen.Length;

                if (total_len < _minlen + length)
                    return -1;   //uncompleted data 未接受完包数据

                //已接受完包数据
                int ePos = _headlen + length;
                if (ePos + _flaglen > bufflen)
                {
                    //	assert(0);
                    DebugMod.LogError("PullOutCore", "invalid length ePos [" + ePos + "] buff length [" + bufflen + "]");
                    _packRcvPool.SetEmpty();
                    return -1;
                }

                //验证包尾标识
                byte eFlag1 = 0, eFlag2 = 0;
                if (!_packRcvPool.GetByte(ePos, out eFlag1))
                {
                    _packRcvPool.SetEmpty();
                    DebugMod.LogError("PullOutCore", "invalid end flag 1");
                    return -1;
                }

                if (!_packRcvPool.GetByte(ePos + sizeof(byte), out eFlag2))
                {
                    _packRcvPool.SetEmpty();
                    DebugMod.LogError("PullOutCore", "invalid end flag 2");
                    return -1;
                }

                //check end flags
                if (eFlag1 != (byte)TcpCommonEnum.PACKET_END1 || eFlag2 != (byte)TcpCommonEnum.PACKET_END2)
                {
                    //	assert(0);
                    DebugMod.LogError("PullOutCore", "invalid end flag , data length : " + length);
                    _packRcvPool.SetEmpty();
                    return -1;
                }

                //read data
                if (length > 0)
                {
                    if (bIsZipdata)
                    {
                        _packRcvPool.CopyData(zipDest, 0, _headlen, length);
                    }
                    else
                    {
                        _packRcvPool.CopyData(byteDest, 0, _headlen, length);
                    }
                }

                _packRcvPool.HeadIncrease(_minlen + length);

                return length;

            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return -1;
            }
        }
    }
}
