using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class GameModuleManager
    {
        protected Dictionary<string, GameModuleBase> m_ModuleDic = new Dictionary<string, GameModuleBase>();

        public void OnUpdate()
        {
            foreach(var module in m_ModuleDic)
            {
                module.Value.OnUpdate();
            }
        }

        public bool RegisterModule(string moduleName, GameModuleBase module)
        {
            string lowerName = moduleName.ToLower();
            if (m_ModuleDic.ContainsKey(lowerName))
                return false;
            module.OnInit();
            m_ModuleDic[lowerName] = module;
            return true;
        }

        public GameModuleBase GetModule(string moduleName)
        {
            string lowerName = moduleName.ToLower();
            if (!m_ModuleDic.ContainsKey(lowerName))
                return null;
            return m_ModuleDic[lowerName];
        }
    }
}
