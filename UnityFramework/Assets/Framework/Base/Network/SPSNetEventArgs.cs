using System;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace SPSGame
{
    /// <summary>
    /// 网络通讯的操作类型
    /// </summary>
    public enum NetEventType 
    { 
        Event_Invalid = 0,
        Connect_Success = 1, 
        Connect_Refuse = 2,
        Connect_Timeout = 3,
        Send_Success = 4,
        Send_Timeout = 5,
        Send_Fail = 6,
        Recv_Success = 7,
        Recv_Timeout = 8,
        Recv_Fail = 9,
        Connect_Close = 10,
        Connect_Close_Fail = 11,
        Action_Invalid = 12,    //无效的行为id
     }
	
    /// <summary>
    /// 与服务器端通信连接参数
    /// </summary>
    public class SPSNetEventArgs : EventArgs
    {
        /// <summary>
        /// 网络通讯的操作类型
        /// </summary>
        public NetEventType eventType;

        /// <summary>
        /// 网络命令
        /// </summary>
        public int ActionID = -1;

        public string ServerName;
        /// <summary>
        /// 远程地址
        /// </summary>
        public string RemoteEndPoint;

        /// <summary>
        /// 错误描述信息
        /// </summary>
        public string ErrorMsg;

        ///// <summary>
        ///// Socket连接错误码
        ///// </summary>
        //public string Error;

        ///// <summary>
        ///// 内部错误描述信息
        ///// </summary>
        //public string ErrorStr;

        ///// <summary>
        ///// 是否回到起始页面
        ///// </summary>
        //public bool ReturnStartPage = false;

        ///// <summary>
        ///// 是否显示错误对话框
        ///// </summary>
        //public bool ShowMsgBox = false;		

        ///// <summary>
        ///// 网络通讯字段
        ///// </summary>
        //public string[] fields = null;

        ///// <summary>
        ///// 二进制格式返回数据
        ///// </summary>
        //public byte[] bytesData = null;

        //public int datalen = 0;
    }
}

