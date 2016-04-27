using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{

    public class StageManager
    {

        protected SPSStageBase m_currectStage = null;
        protected Dictionary<string, SPSStageBase> m_appStageSet = new Dictionary<string, SPSStageBase>();

        public bool RegisterStage(string stagename, SPSStageBase stage)
        {
            if (null == stage)
                return false;
            if (m_appStageSet.ContainsKey(stagename))
                return false;
            stage.OnInit();
            m_appStageSet.Add(stagename, stage);
            return true;
        }

        public SPSStageBase GetStage(string name)
        {
            if (!m_appStageSet.ContainsKey(name))
                return null;
            return m_appStageSet[name];
        }

        public bool BeginStage(string name)
        {
            if (!m_appStageSet.ContainsKey(name))
                return false;
            SPSStageBase stage = m_appStageSet[name];
            m_currectStage = stage;
            stage.BeginStage();
            return true;
        }

        public bool EndStage(string name)
        {
            if (!m_appStageSet.ContainsKey(name))
                return false;
            m_appStageSet[name].EndStage();
            m_currectStage = null;
            return true;
        }

        public void OnUpdate()
        {
            if (null != m_currectStage)
            {
                if (m_currectStage.WaitingEnd)
                {
                    string nextstage = m_currectStage.NextStage;
                    m_currectStage.EndStage();
                    BeginStage(nextstage);
                }
                else
                    m_currectStage.OnUpdate();
            }
        }
    } 
}

