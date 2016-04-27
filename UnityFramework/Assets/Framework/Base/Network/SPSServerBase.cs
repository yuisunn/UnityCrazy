using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
     public abstract class SPSServerBase : SPSCmdParser
    {
        protected string _serverName;

        public string ServerName
        {
            get
            {
                 return _serverName;
            }
            set
            {
                _serverName = value;
            }

        }

        public abstract void OnInit();

        public abstract void OnUpdate();

        public abstract void OnApplicationQuit();

        /// <summary>
        /// 网络请求回调统一处理方法
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public delegate bool CommonDataCallback(NetReader reader);

        /// <summary>
        /// 注册网络请求回调统一处理方法
        /// </summary>
        public CommonDataCallback CommonCallback { get; set; }

        /// <summary>
        /// 网络请求出错回调方法
        /// </summary>
        /// <param name="nType"></param>
        /// <param name="actionId"></param>
        /// <param name="strMsg"></param>
        public delegate void NetError(eNetError nType, int actionId, string strMsg);

        /// <summary>
        /// 注册网络请求出错回调方法
        /// </summary>
        public NetError NetErrorCallback { get; set; }



        public SPSServerBase()
        {
            NetErrorCallback = (type, id, msg) =>
            {
                DebugMod.LogError(string.Format("Net error:{0}-{1}-{2}", type, id, msg));
            };
        }


        protected void OnNetError(int nActionId, string str)
        {
            if (NetErrorCallback != null)
            {
                NetErrorCallback(eNetError.eConnectFailed, nActionId, str);
            }
        }
        protected void OnNetTimeOut(int nActionId)
        {
            if (NetErrorCallback != null)
            {
                NetErrorCallback(eNetError.eTimeOut, nActionId, "timeout.");
            }

        }

        protected bool ProcessPackage(NetPackage package)
        {
            if (null == package || package.DataReader == null)
                return false;

            try
            {
                bool result = true;
                if (CommonCallback != null)
                {
                    result = CommonCallback(package.DataReader);
                }

                if (!result)
                {
                    DebugMod.LogError("CommonCallback fail.");
                    return false;
                }

                ActionParam actionResult = null;

                if (null != package.Action)
                {
                    actionResult = package.Action.TryDecodePackage(package);
                    if (null == actionResult)
                    {
                        DebugMod.LogError("Decode package fail.");
                        return false;
                    }

                    if (!package.Action.ProcessAction())
                    {
                        DebugMod.LogError("ProcessAction fail." + package.ActionId);
                        return false;
                    }
                }
                else
                {
                    actionResult = new ActionParam();
                    actionResult["package"] = package;
                }

                if (0 != ProcessServerCmd((CsShare.SPSCmd)package.ActionId, actionResult))
                {
                    DebugMod.LogError("ProcessServerCmd fail." + package.ActionId);
                    return false;
                }
                return true;

            }
            catch(Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }

        }
    }
}
