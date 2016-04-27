using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class GameModuleBase
    {

        protected string m_ModuleName = "";
        public string ModuleName
        {
            get
            {
                return m_ModuleName;
            }
        }
        //public void RegisterMe(GameModuleManager mgr, string moduleName)
        //{
        //    Tools.Assert.Check(mgr != null);
        //    m_ModuleName = moduleName;
        //    mgr.RegisterModule(moduleName, this);
        //}

        public virtual void OnInit()
        {

        }

        public virtual void OnUpdate()
        {

        }
    }
}
