using System.Collections;
using SPSGame.Tools;

namespace SPSGame
{

    //一个SPSStageBase包含场景、UI等需要表现的东西
    public abstract class SPSStageBase
    {
        protected string m_stageName = "";
        public string StageName
        {
            get
            {
                return m_stageName;
            }
            set
            {
                m_stageName = value;
            }
        }
        protected bool m_InStage = false;
        public bool InStage
        {
            get
            {
                return m_InStage;
            }
            set
            {
                m_InStage = value;
            }
        }

        protected bool m_waitEnd = false;
        public bool WaitingEnd
        {
            get
            {
                return m_waitEnd;
            }
        }

        protected string m_nextStage = "";

        public string NextStage
        {
            get
            {
                return m_nextStage;
            }
        }

        public SPSStageBase(string name)
        {
            m_stageName = name;
        }

        // Use this for initialization
        public virtual void OnInit()
        {
            m_InStage = false;
            m_nextStage = "";
            m_waitEnd = false;
        }

        public virtual void RegisterCmds()
        {

        }

        // Update is called once per frame
        public virtual void OnUpdate()
        {

        }

        public abstract void AfterStageBegin();

        public abstract void BeforeStageEnd();

        public void BeginStage()
        {
            m_nextStage = "";
            m_InStage = true;
            DebugMod.Log("Stage " + m_stageName + " begin.");
            AfterStageBegin();
        }

        public void EndStage()
        {
            m_waitEnd = false;
            BeforeStageEnd();
            m_nextStage = "";
            m_InStage = false;
            DebugMod.Log("Stage " + m_stageName + " end.");
        }

        public void WaitEndStage(string nextstage)
        {
            m_waitEnd = true;
            m_nextStage = nextstage;
        }
    } 
}
