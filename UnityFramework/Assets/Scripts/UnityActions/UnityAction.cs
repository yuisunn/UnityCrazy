using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class UnityActionFactory : ActionFactory
    {
        private static UnityActionFactory _Instance;
        public static UnityActionFactory Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new UnityActionFactory();
                return _Instance;
            }
        }

        protected override string ActionFormat()
        {
            return "SPSGame.UnityAction_{0}";
        }
    }

    public abstract class UnityAction : ActionBase
    {

    }
}
