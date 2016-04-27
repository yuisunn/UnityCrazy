using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using UnityEngine;
using SPSGame.CsShare;
using SPSGame.Tools;

public class NetWriterHttp
{
    private ulong s_userID = 0;
    private string s_strSessionID = "";
    private string s_strSt = "";
    private static ResponseContentType _respContentType = ResponseContentType.Stream;
    private string s_strUrl = "";
    private string s_strPostData = "";
    private string s_strUserData = "";
    private int s_Counter = 1;
    private string s_md5Key = "";
    private byte[] m_bytePostData = new byte[(int)TcpCommonEnum.SOCKET_BUF_SIZE_CLIENT_SEND];
    public bool IsGet { get; private set; }
        
    public static ResponseContentType ResponseContentType
    {
        get
        {
            return _respContentType;
        }
    }

    public int MsgId
    {
        get { return s_Counter; }
    }

    public ulong UserID
    {
        get { return s_userID; }
    }

    public string SessionID
    {
        get { return s_strSessionID; }
    }

    public string St
    {
        get { return s_strSt; }
    }

    public void SetMd5Key(string value)
    {
        s_md5Key = value;
    }

    public void resetData()
    {
        s_strPostData = "";
        s_strUserData = string.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}", s_Counter, s_strSessionID, s_userID, s_strSt);
        s_Counter++;
    }

    public void setSessionID(string pszSessionID)
    {
        if (pszSessionID != null)
        {
            s_strSessionID = pszSessionID;
            resetData();
        }
    }

    public void setUserID(ulong value)
    {
        s_userID = value;
        resetData();
    }

    public void setStime(string pszTime)
    {
        if (pszTime != null)
        {
            s_strSt = pszTime;
            resetData();
        }
    }

    public NetWriterHttp()
    {
        resetData();
    }


    public void writeInt32(string szKey, int nValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, nValue);
    }

    public void writeFloat(string szKey, float fvalue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, fvalue);
    }

    public void writeString(string szKey, string szValue)//
    {
        if (szValue == null)
        {
            return;
        }
        s_strUserData += string.Format("&{0}=", szKey);

        s_strUserData += url_encode(szValue);
    }

    public void writeLong(string szKey, long nValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, nValue);
    }

    public void writeShort(string szKey, long sValue)
    {
        s_strUserData += string.Format("&{0}={1}", szKey, sValue);
    }
    //public void writeBuf(string szKey, byte[] buf, int nSize)
    //{
    //    System.Diagnostics.Debug.Assert(false);
    //}

    public void SetUrl(string szUrl, ResponseContentType respContentType, bool isGet = false)
    {
        s_strUrl = szUrl;
        IsGet = isGet;
        _respContentType = respContentType;
    }

    public string GetUrl()
    {
        return s_strUrl;
    }

    //public bool IsSocket()
    //{
    //    if (s_strUrl != null && !s_strUrl.Contains("http"))
    //    {
    //        return true;
    //    }
    //    return false;
    //}


    public string url_encode(string str)
    {
        return WWW.EscapeURL(str);
    }

    public string getMd5String(byte[] buf)
    {
        return MD5Utils.Encrypt(buf);
    }
    public string getMd5String(string input)
    {
        return getMd5String(Encoding.Default.GetBytes(input));
    }

    public byte[] PostData(out int length)
    {
        length = 0;
        if (_respContentType == ResponseContentType.Json)
        {
            s_strPostData = s_strUserData + "&sign=" + getMd5String(s_strUserData + s_md5Key);
            //DebugMod.Log("request param:" + s_strPostData);
        }
        else
        {
            s_strPostData = "d="; // IsSocket() ? "?d=" : "d=";
            string str = s_strUserData + "&sign=" + getMd5String(s_strUserData + s_md5Key);
            //DebugMod.Log("request param:" + str);
            s_strPostData += url_encode(str);
        }

        //if (IsSocket())
        //{
        //    int postDataLen = Encoding.UTF8.GetByteCount( s_strPostData );
        //    byte[] len = BitConverter.GetBytes(postDataLen);

        //    if (postDataLen + len.Length > m_bytePostData.Length)
        //    {
        //        DebugMod.LogError("postdata is too long: " + s_strUserData);
        //        return null;
        //    }

        //    //加包长度，拆包时使用
        //    Buffer.BlockCopy(len, 0, m_bytePostData, 0, len.Length);
        //    length = Encoding.UTF8.GetBytes(s_strPostData, 0, s_strPostData.Length, m_bytePostData, len.Length);
        //    length += len.Length;
        //}
        //else
        {
            int postDataLen = Encoding.UTF8.GetByteCount( s_strPostData );
            if (postDataLen > m_bytePostData.Length)
            {
                DebugMod.LogError("postdata is too long: " + s_strUserData);
                return null;
            }

            length = Encoding.UTF8.GetBytes(s_strPostData, 0, s_strPostData.Length, m_bytePostData, 0);
        }

        return m_bytePostData;
    }

}