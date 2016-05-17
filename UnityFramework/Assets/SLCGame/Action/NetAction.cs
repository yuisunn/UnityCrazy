using UnityEngine;
using System.Collections;

namespace SLCGame
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
            return "Action{0}";
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

        public virtual void SendParameterNet(string id, ActionParam actionParam) { }
        public virtual void SendParameterLocal(string id, ActionParam actionParam) { }
        public override bool ProcessAction()
        {
            return true;
        }
    }
}