using UnityEngine;
using System.Collections;
using SLCGame.Unity;
using System.Collections.Generic;
using SLCGame;
using System.Threading;
using System; 
using SLCGame.Tools;
using SLCGame.Tools.Unity;

namespace SLCGame
{
    public class GameMain : UnitySingleton<GameMain>
    {
        public static GameObject GlobalObject
        {
            get;
            private set;
        }

        private static Threader m_FrmworkThreader = new Threader();
        private static bool m_AppExit = false;
        private Queue<LogicAction> m_QueueLogicAction = new Queue<LogicAction>();
         

        public void Awake()
        {
            GlobalObject = gameObject; 
        }

        public void OnApplicationPause()
        {

        }
        public void OnApplicationFocus()
        {

        }

        public void Init()
        {
            GlobalObject = gameObject;
            try
            {
                //创建逻辑线程
                DebugMod.Log("GameMain Init");
                LuaScriptMgr.Instance.Init();
                LuaScriptMgr.Instance.InitStart();
                //Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
                //t.Start(this);
                //Thread.Sleep(0);
                NetActionFactory.Instance.ActionAssembly = System.Reflection.Assembly.GetCallingAssembly();
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public void OnThreadUpdate()
        {
            try
            {
                DequueLogicAction();
                m_FrmworkThreader.Update();
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }
        public static void ThreadProc(object param)
        {
            try
            {
                DebugMod.Log("Logic Thread Start.");

                GameMain frame = param as GameMain;
                long prevTick = DateTime.Now.Ticks;
                if (null != param)
                {
                    while (!m_AppExit)
                    {
                        long nowTick = DateTime.Now.Ticks;
                        long elapseTick = nowTick - prevTick;
                        if (elapseTick > 333333)        //33ms
                        {
                            prevTick = nowTick;
                            frame.OnThreadUpdate();
                        }
                    }
                }

                DebugMod.Log("Logic Thread Exit.");
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public void FileCkeckEnd()
        {

        }

        public void Update()
        {
            DequueLogicAction();
        }


        public bool CallLogicAction(ActionDefine actionType, ActionParam param)
        {
            try
            {
                LogicAction action = (LogicAction)LogicActionFactory.Instance.CreateAction(actionType);
                if (null == action)
                    return false;
                action.ActionId = (int)actionType;
                action.ActParam = param;

                lock (m_QueueLogicAction)
                {
                    m_QueueLogicAction.Enqueue(action);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        private void DequueLogicAction()
        {
            try
            {
                LogicAction action = null;
                do
                {
                    action = null;
                    lock (m_QueueLogicAction)
                    {
                        if (0 < m_QueueLogicAction.Count)
                            action = m_QueueLogicAction.Dequeue();//移除并返回栈顶数据
                    }
                    if (null != action)
                        action.ProcessAction();
                } while (action != null);
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        protected void OnDestroy()
        {
            if(LuaScriptMgr.Instance!=null)
                LuaScriptMgr.Instance.Destroy();
            AssetBundleMgr.Instance.ClearAllAssetBundles(); 
        }

    }
}
