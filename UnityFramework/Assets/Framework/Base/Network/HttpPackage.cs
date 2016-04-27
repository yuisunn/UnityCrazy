using UnityEngine;
using SPSGame.CsShare;

namespace SPSGame
{
    public class HttpPackage : NetPackage
    {
        public WWW WwwObject { get; set; }
        private byte[] buffDecompress = null;
        public string error
        {
            get
            {
                if (IsOverTime)
                {
                    return "http request over time";
                }
                else
                {
                    return WwwObject.error;
                }
            }
        }

        public HttpPackage()
        {
            buffDecompress = new byte[1024 * 64];
        }

        public bool GetResponse(byte[] dstBuff)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                System.Array.Copy(WwwObject.bytes, 0, dstBuff, 0, WwwObject.bytes.Length);
                return true;

            }
            string strEncoding = null;
            if (WwwObject.responseHeaders.ContainsKey("CONTENT-ENCODING"))
            {
                strEncoding = WwwObject.responseHeaders["CONTENT-ENCODING"];
            }

            if (strEncoding != null && strEncoding == "gzip")
            {
                if (null == buffDecompress)
                    return false;
                int len = 0;
                if (!NetReader.Decompression(WwwObject.bytes, buffDecompress, out len))
                    return false;

                System.Array.Copy(buffDecompress, 0, dstBuff, 0, len);
                return true;
            }
            else
            {
                System.Array.Copy(WwwObject.bytes, 0, dstBuff, 0, WwwObject.bytes.Length);
                return true;
            }
        }

    }

}