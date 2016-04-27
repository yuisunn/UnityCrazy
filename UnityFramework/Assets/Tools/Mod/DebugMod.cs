using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace SPSGame.Tools
{
    public class DebugMod
    {

        public delegate void LogDelegate(string msg);

//#if UNITY_EDITOR
//#else
        private static LogDelegate LogNormal;
        private static LogDelegate LogWarn;
        private static LogDelegate LogErr;

        public static void SetLogNormal( LogDelegate ld )
        {
            LogNormal = ld;
        }

        public static void SetLogWarn(LogDelegate ld)
        {
            LogWarn = ld;
        }

        public static void SetLogError(LogDelegate ld)
        {
            LogErr = ld;
        }
//#endif

        public static void ShowNotifyMsg(object message)
        {
            //#if UNITY_EDITOR
            Debug.Log(message);
            //#else
            if (null != LogNormal)
                LogNormal(message.ToString());
            //#endif
        }

        /// <summary>
        /// 调试输出普通信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void Log(object message)
        {
//#if UNITY_EDITOR
            Debug.Log(message);
//#else
            if (null != LogNormal)
                LogNormal(message.ToString());
//#endif
        }


        /// <summary>
        /// 调试输出警告信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void LogWarning(object message)
        {
//#if UNITY_EDITOR
            Debug.LogWarning(message);

//#else
            if (null != LogWarn)
                LogWarn(message.ToString());
//#endif
        }

        public static void LogError(object category, object message)
        {
            LogError(string.Format("{0} : {1}", category , message));
        }

        /// <summary>
        /// 调试输出错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void LogError(object message)
        {
//#if UNITY_EDITOR
            Debug.LogError(message);

//#else
            if (null != LogErr)
                LogErr(message.ToString());
//#endif

        }

        public static void LogException(Exception e)
        {
//#if UNITY_EDITOR
            Debug.LogException(e);
//#else

//#endif
        }

        public static void LogException(Exception e, string extMsg, bool showMsgBox)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("应用程序出现了异常:\r\n{0}\r\n", e.Message);

                stringBuilder.AppendFormat("\r\n 额外信息: {0}", extMsg);
                if (null != e)
                {
                    if (e.InnerException != null)
                    {
                        stringBuilder.AppendFormat("\r\n {0}", e.InnerException.Message);
                    }
                    stringBuilder.AppendFormat("\r\n {0}", e.StackTrace);
                }

                //记录异常日志文件
                //LogManager.WriteException(stringBuilder.ToString());
                if (showMsgBox)
                {
                    //弹出异常日志窗口
                    System.Console.WriteLine(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        /// <summary>
        /// 格式化堆栈信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void LogFormatStack(System.Diagnostics.StackTrace stackTrace, string extMsg)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("应用程序出现了对象锁定超时错误:\r\n");

                stringBuilder.AppendFormat("\r\n 额外信息: {0}", extMsg);
                stringBuilder.AppendFormat("\r\n {0}", stackTrace.ToString());

                //记录异常日志文件
                //LogManager.WriteException(stringBuilder.ToString());
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }
        }

    }
}