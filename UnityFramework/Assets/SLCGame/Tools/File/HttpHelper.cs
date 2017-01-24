using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text; 
using SLCGame.Unity;

namespace SLCGame.Tools
{
    internal class WebReqState
    {
        public byte[] m_Buffer;

        public FileStream m_FileStream;

        public const int m_BufferSize = 1024;

        public Stream m_OrginalStream;

        public HttpWebResponse m_WebResponse;

        public WebReqState(string path)
        {
            m_Buffer = new byte[1024];
            m_FileStream = new FileStream(path, FileMode.Create);
        }

    }
    class HttpDownFile
    {
        string m_SavePath = null; 
        string m_Url = null;
        public HttpDownFile(string path, string url)
        {
            m_SavePath = path; 
            m_Url = url;
        }

        public void DownloadFile()
        { 
            DebugMod.Log("asyDownload:" + m_Url); 
            HttpWebRequest httpRequest = WebRequest.Create(m_Url) as HttpWebRequest;
            httpRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), httpRequest);
        }

        void ResponseCallback(IAsyncResult ar)
        { 
            HttpWebRequest req = ar.AsyncState as HttpWebRequest;
            if (req == null) return;
            HttpWebResponse resp = req.EndGetResponse(ar) as HttpWebResponse;
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                resp.Close();
                DebugMod.LogError("http error:" + resp.StatusCode);
                return;
            } 
            WebReqState st = new WebReqState(m_SavePath);
            st.m_WebResponse = resp;
            Stream responseStream = resp.GetResponseStream();
            st.m_OrginalStream = responseStream;
            responseStream.BeginRead(st.m_Buffer, 0, WebReqState.m_BufferSize, new AsyncCallback(ReadDataCallback), st);
        }

        void ReadDataCallback(IAsyncResult ar)
        {
            WebReqState rs = ar.AsyncState as WebReqState;
            int read = rs.m_OrginalStream.EndRead(ar);
            if (read > 0)
            {
                rs.m_FileStream.Write(rs.m_Buffer, 0, read);
                rs.m_FileStream.Flush();
                rs.m_OrginalStream.BeginRead(rs.m_Buffer, 0, WebReqState.m_BufferSize, new AsyncCallback(ReadDataCallback), rs);
            }
            else
            {
                rs.m_FileStream.Close();
                rs.m_OrginalStream.Close();
                rs.m_WebResponse.Close();
                DebugMod.Log("http download url finish"+ m_Url);
            }
        }

    }
}
