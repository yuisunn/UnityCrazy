using System;
//using UnityEngine;
using SPSGame.Tools;

namespace SPSGame
{

    public class NetActionFactory : ActionFactory
    {
        private static NetActionFactory _Instance;
        public static NetActionFactory Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new NetActionFactory();
                return _Instance;
            }
        }

        protected override string ActionFormat()
        {
            return "SPSGame.{0}";
        }
    }

    /// <summary>
    /// 游戏Action接口
    /// </summary>
    public class NetAction : ActionBase
    {
        public NetAction()
        {
            ActParam = new ActionParam();
        }
        /// <summary>
        /// 尝试解Body包
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public ActionParam TryDecodePackage(NetPackage package)
        {
            try
            {
                if (null != ActParam)
                    ActParam.Clear();
                return DecodePackage(package);
            }
            catch (Exception ex)
            {
                DebugMod.LogError(string.Format("Action {0} decode package error:{1}", ActionId, ex));
                return null;
            }
        }

        public virtual void SendParameterHttp(NetWriterHttp writer, ActionParam actionParam){ }
        public virtual void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam) { }
        public virtual ActionParam DecodePackage(NetPackage package) 
        {
            return ActParam;
        }
        public override bool ProcessAction()
        {
            return true;
        }
    }

}
