using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
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
            return "SPSGame.LogicAction_{0}";
        }
    }

    public abstract class LogicAction : ActionBase
    {
    }
}
