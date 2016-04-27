using System;

namespace SPSGame
{
    public enum Status
    {
        eStartRequest = 0,
        eEndRequest = 1,
    }

    public enum eNetError
    {
        eConnectFailed = 0,
        eTimeOut = 1,
    }
    /// <summary>
    /// 网络包
    /// </summary>
    public class NetPackage
    {
        /// <summary>
        /// 协议编号
        /// </summary>
        public int ActionId
        {
            get;
            set;
        }

        /// <summary>
        /// 协议脚本
        /// </summary>
        public NetAction Action
        {
            set;
            get;
        }

        /// <summary>
        /// 协议参数，收、发都用它
        /// </summary>
        public ActionParam ActionParam
        {
            set;
            get;
        }

        public NetReader DataReader
        {
            set;
            get;
        }

        /// <summary>
        /// 服务器编号
        /// </summary>
        public short ServerID
        {
            set;
            get;

        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { set; get; }

        public string strURL { set; get; }
        public bool IsGet { get; set; }
        public ResponseContentType respContentType { set; get; }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsOverTime
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            set;
            get;
        }

        /// <summary>
        /// 错误码 0代码成功 非0失败
        /// </summary>
        public int ErrorCode
        {
            set;
            get;
        }

        //消息唯一编号，递增
        public int MsgId { set; get; }

        //自定义数据
        public object UserData { set; get; }

        //public NetReader Reader { get; set; }

    }

}