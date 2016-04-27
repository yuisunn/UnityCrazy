using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using UnityEngine;
using SPSGame.CsShare;
using SPSGame.Tools;
public enum ResponseContentType
{
    Stream = 0,
    Json
}
public class NetWriterTcp
{
    //private int _userid = 0;
    private short _svrid = 0;
    private int _actionid = 0;
    private short _counter = 0;
    private byte[] m_bytePostData = new byte[(int)TcpCommonEnum.SOCKET_BUF_SIZE_CLIENT_SEND];
    private string s_strPostData = "";

    public short MsgId
    {
        get { return _counter; }
    }

    //public int UserID
    //{
    //    get { return _userid; }
    //    set
    //    {
    //        _userid = value;
    //    }
    //}

    public int ActionID
    {
        get { return _actionid; }
        set
        {
            _actionid = value;
        }
    }

    public short ServerID
    {
        get { return _svrid; }
        set
        {
            _svrid = value;
        }
    }

    public NetWriterTcp()
    {
    }

    public void writeHeader(short serverID, int actionID)
    {
        s_strPostData = "";
        if (++_counter > 30000)
            _counter = 1;

        ServerID = serverID;
        ActionID = actionID;
        //writeWord(_counter);
        //writeWord(_svrid);
        //writeInt32(_actionid);
    }

    public void writeInt32(int nValue)
    {
        if (s_strPostData.Length > 0)
            s_strPostData += string.Format(":{0}", nValue);
        else
            s_strPostData += string.Format("{0}", nValue);
    }

    public void writeFloat(float fvalue)
    {
        if (s_strPostData.Length > 0)
            s_strPostData += string.Format(":{0}", fvalue);
        else
            s_strPostData += string.Format("{0}", fvalue);
    }

    public void writeString(string szValue)
    {
        if (szValue == null)
        {
            return;
        }
        if (szValue.Contains(':'))
        {
            DebugMod.LogError("msg send to server cannot include ':' !");
            return;
        }
        if (s_strPostData.Length > 0)
            s_strPostData += string.Format(":{0}", szValue); 
        else
            s_strPostData += string.Format("{0}", szValue); 
    }

    public void writeLong(long nValue)
    {
        if (s_strPostData.Length > 0)
            s_strPostData += string.Format(":{0}", nValue);
        else
            s_strPostData += string.Format("{0}", nValue);
    }

    public void writeShort(short sValue)
    {
        if (s_strPostData.Length > 0)
            s_strPostData += string.Format(":{0}", sValue);
        else
            s_strPostData += string.Format("{0}", sValue);
    }

    public byte[] PostData(out int length)
    {
        length = 0;

        int headerlen = 14; // 2 + 4 + 2 + 2 + 4;
        int taillen = 2;

        byte[] byActionId = BitConverter.GetBytes(_actionid);
        SPSGame.Tools.Assert.Check(byActionId.Length == 4);

        byte[] bySvrId = BitConverter.GetBytes(_svrid);
        SPSGame.Tools.Assert.Check(bySvrId.Length == 2);

        byte[] msgcounter = BitConverter.GetBytes(_counter);
        SPSGame.Tools.Assert.Check(msgcounter.Length == 2);

        int temp = Encoding.UTF8.GetByteCount(s_strPostData);
        if(temp > 50000)        //最多支持50000字节
            return null;
        ushort postStrLen = (ushort)temp;

        int dataLen = (int)postStrLen + sizeof(ushort);

        byte[] lenData = BitConverter.GetBytes(dataLen);
        byte[] lenStr = BitConverter.GetBytes(postStrLen);

        if (dataLen + headerlen + taillen > m_bytePostData.Length)
        {
            DebugMod.LogError("dataLen is too long: " + s_strPostData);
            return null;
        }

        //包头标识
        int dstindex = 0;
        m_bytePostData[dstindex] = (byte)TcpCommonEnum.PACKET_START1;
        dstindex += 1;
        m_bytePostData[dstindex] = (byte)TcpCommonEnum.PACKET_START2;
        dstindex += 1;

        Buffer.BlockCopy(byActionId, 0, m_bytePostData, dstindex, byActionId.Length);
        dstindex += byActionId.Length;

        Buffer.BlockCopy(bySvrId, 0, m_bytePostData, dstindex, bySvrId.Length);
        dstindex += bySvrId.Length;

        Buffer.BlockCopy(msgcounter, 0, m_bytePostData, dstindex, msgcounter.Length);
        dstindex += msgcounter.Length;

        //加包长度，拆包时使用，4字节
        Buffer.BlockCopy(lenData, 0, m_bytePostData, dstindex, lenData.Length);
        dstindex += lenData.Length;

        //字符串长度，2字节
        Buffer.BlockCopy(lenStr, 0, m_bytePostData, dstindex, lenStr.Length);
        dstindex += lenStr.Length;

        //字符串字节流
        dstindex += Encoding.UTF8.GetBytes(s_strPostData, 0, s_strPostData.Length, m_bytePostData, dstindex);

        //结尾标识
        m_bytePostData[dstindex] = (byte)TcpCommonEnum.PACKET_END1;
        dstindex += 1;
        m_bytePostData[dstindex] = (byte)TcpCommonEnum.PACKET_END2;
        dstindex += 1;

        length = dstindex;

        SPSGame.Tools.Assert.Check(length == dataLen + headerlen + taillen);
        return m_bytePostData;
    }

}