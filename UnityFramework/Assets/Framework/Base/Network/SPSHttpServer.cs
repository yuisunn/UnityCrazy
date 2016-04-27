using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SPSGame.Tools;

namespace SPSGame
{
    public class SPSHttpServer : SPSServerBase, IDisposable
    {
        protected string m_Address = "";
        protected int m_Port = 0;
        private readonly Queue<NetPackage> m_sendQueue = new Queue<NetPackage>();
        protected NetReader m_reader = null;
        public NetReader Reader
        {
            get
            {
                return m_reader;
            }
        }
        protected NetWriterHttp m_writer = null;
        public NetWriterHttp Writer
        {
            get
            {
                return m_writer;
            }
        }


        protected int OVER_TIME = 30;
        private const int NETSUCCESS = 0;

        public int NetSuccess
        {
            get { return NETSUCCESS; }
        }

        public IHeadFormater HeadFormater { get; set; }

        HttpPackage httpPackage = null;

        public void Dispose()
        {
            httpPackage = null;
        }

        public bool Create(string address, int port, int overtime = 30)
        {
            HeadFormater = new DefaultHeadFormater();
            m_reader = new NetReader(HeadFormater);
            m_writer = new NetWriterHttp();
            SetRemote(address, port);
            OVER_TIME = overtime;
            httpPackage = new HttpPackage();
            return true;
        }

        public override void OnInit()
        {

        }
        public override void OnUpdate()
        {
            try
            {
                DoAllNetEvent();

                NetPackage package = null;
                lock (m_sendQueue)
                {
                    if (0 < m_sendQueue.Count)
                        package = m_sendQueue.Dequeue();
                }
                if (null != package)
                {
                    HttpGetRequest(package);
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public override void OnApplicationQuit()
        {
        }

        public void SetRemote(string address, int port)
        {
            m_Address = address;
            m_Port = port;
        }

        /// <summary>
        /// Send
        /// </summary>
        public bool ReadySend(CsShare.SPSCmd cmdType, ActionParam actionParam, string szURL, bool IsGet=true, ResponseContentType respContentType = ResponseContentType.Stream, object UserData=null)
        {
            try
            {
                NetPackage package = new NetPackage();
                package.ActionId = (int)cmdType;
                package.Action = (NetAction)NetActionFactory.Instance.CreateAction(cmdType);
                if (null == package.Action)
                    return false;
                package.strURL = szURL;
                package.IsGet = IsGet;
                package.respContentType = respContentType;
                package.UserData = UserData;

                lock (m_sendQueue)
                {
                    m_sendQueue.Enqueue(package);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        private bool HttpGetRequest(NetPackage package)
        {
            try
            {
                if (null == package.Action || null == package.strURL)
                {
                    DebugMod.LogError(string.Format("Not found {0} of NetAction object.", package.ActionId));
                    return false;
                }

                if (null == httpPackage)
                    return false;

                Writer.SetUrl(package.strURL, package.respContentType, package.IsGet);

                //package.Action.Head.MsgId = this.Writer.MsgId - 1;
                package.SendTime = DateTime.Now;

                this.Writer.resetData();
                Writer.writeInt32("actionId", package.ActionId);
                package.Action.SendParameterHttp(Writer, package.ActionParam);
                int length = 0;
                byte[] postData = Writer.PostData(out length);
                if (postData == null || length <= 0)
                {
                    return false;
                }

                httpPackage.WwwObject = package.IsGet ? new WWW(string.Format("{0}?{1}", package.strURL, Encoding.UTF8.GetString(postData))) : new WWW(package.strURL, postData);
                httpPackage.ActionId = package.ActionId;
                httpPackage.Action = package.Action;

                TimeSpan tsStart = new TimeSpan(package.SendTime.Ticks);
                TimeSpan tsEnd = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = tsEnd.Subtract(tsStart).Duration();

                if (ts.Seconds > OVER_TIME)
                {
                    httpPackage.IsOverTime = true;
                }

                return OnHttpRespond(httpPackage);
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// http respond
        /// </summary>
        /// <param name="package"></param>
        public bool OnHttpRespond(HttpPackage package)
        {
            try
            {
                if (package.error != null)
                {
                    OnNetError(package.ActionId, package.error);
                    return false;
                }
                else if (package.IsOverTime)
                {
                    OnNetTimeOut(package.ActionId);
                    return false;
                }
                else
                {
                    package.DataReader = this.Reader;
                    if (package.GetResponse(Reader.ParseBuff))
                        return ProcessPackage(package);
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
