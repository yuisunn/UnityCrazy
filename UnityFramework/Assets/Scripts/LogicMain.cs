using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SPSGame.Tools;
using SPSGame.Unity;

namespace SPSGame
{

    public class LogicMain : SPSFramework
    {
        private static LogicMain _Instance;
        public static LogicMain Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new LogicMain();
                return _Instance;
            }
        }

        private Queue<UnityAction> m_queueUnityAction = new Queue<UnityAction>();

        public LogicMain()
        {
            //System.Type thisType = System.Type.GetType("SPSGame.LogicMain");
            UnityActionFactory.Instance.ActionAssembly = System.Reflection.Assembly.GetCallingAssembly();
        }

        private void UnityActionCallBack(UnityActionDefine actionType, ActionParam param)
        {
            UnityAction action = (UnityAction)UnityActionFactory.Instance.CreateAction(actionType);
            if (null == action)
                return;

            action.ActionId = (int)actionType;
            action.ActParam = param;
            lock (m_queueUnityAction)
            {
                m_queueUnityAction.Enqueue(action);
            }
        }

        private void DequueUnityAction()
        {
            UnityAction action = null;
            do
            {
                action = null;
                lock (m_queueUnityAction)
                {
                    if (0 < m_queueUnityAction.Count)
                        action = m_queueUnityAction.Dequeue();
                }
                if (null != action)
                    action.ProcessAction();
            } while (action != null);
        }

        protected override void InitMore()
        {
            InitUnityActionCallBack(UnityActionCallBack);
            InitUnityReadFileCallBack(DataManager.Instance.GetConfigStringData);
        }

        public override void OnAppUpdate()
        {
            DequueUnityAction();
        }
    }
     
}
