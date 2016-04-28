using UnityEngine;
using System.Collections;
using SPSGame.Unity;
using System.Collections.Generic;
using SPSGame;
using System.Threading;
using System;

public class GameMain : UnitySingleton<GameMain> {

    private static Threader _frmworkThreader = new Threader();
    private static bool _appExit = false;
    private Queue<LogicAction> m_queueLogicAction = new Queue<LogicAction>();


    public void Awake()
    {
       
    }

    public void OnApplicationPause()
    {

    }
    public void OnApplicationFocus()
    { 

    }

    public void Init()
    {
        try
        { 
            //创建逻辑线程
            DebugMod.Log("GameMain Init");
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
            _frmworkThreader.Update();
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
                while (!_appExit)
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

    public void Update()
    {
        DequueLogicAction();
    }


    public bool CallLogicAction(Def.LogicActionDefine actionType, ActionParam param)
    {
        try
        {
            LogicAction action = (LogicAction)LogicActionFactory.Instance.CreateAction(actionType);
            if (null == action)
                return false;
            action.ActionId = (int)actionType;
            action.ActParam = param;

            lock (m_queueLogicAction)
            {
                m_queueLogicAction.Enqueue(action);
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
                lock (m_queueLogicAction)
                {
                    if (0 < m_queueLogicAction.Count)
                        action = m_queueLogicAction.Dequeue();//移除并返回栈顶数据
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
     
}
