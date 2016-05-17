using UnityEngine;
using System.Collections;

namespace SLCGame
{
    public class LogicActionFactory : ActionFactory
    {
        private static LogicActionFactory _Instance;
        public static LogicActionFactory Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new LogicActionFactory();
                return _Instance;
            }
        }

        protected override string ActionFormat()
        {
            return "Action{0}";
        }
    }

    public abstract class LogicAction : ActionBase
    {
        public LogicAction()
        {
            ActParam = new ActionParam();
        }

        public virtual void SendParameterLogic(string id, ActionParam actionParam) { }

        public override bool ProcessAction()
        {
            return true;
        }
    }
}