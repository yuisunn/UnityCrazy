using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using SLCGame.Tools.Unity;
using SLCGame.Unity;

namespace SLCGame.Unity
{  

    public class TheardEvent
    {
        public string name;
        public float waitTime;
        public Action action;
        public Action callback;

        public TheardEvent(Action act, Action back)
        {
            action = act;
            callback = back;
        }
 
    } 
    /// <summary>
    /// unity 运行的线程管理
    /// 没有延时执行
    /// </summary>
    public class ThreadMgr : MonoBehaviour
    {
        public static int m_iMaxThreads = 8;
        static int m_iRunThreadsNum;
        private int m_iCount;

        static bool initialized;
        private static ThreadMgr m_Instence;
        public static ThreadMgr Instence
        {
            get
            {
                if(m_Instence!=null)
                    Initialize();
                return m_Instence;
            }
        }

        void Awake()
        {
            m_Instence = this;
            initialized = true;
        }

        static void Initialize()
        {
            if (!initialized)
            {
                if (!Application.isPlaying)
                    return;
                initialized = true;
                var g = new GameObject("ThreadMgr");
                m_Instence = g.AddComponent<ThreadMgr>();
                DontDestroyOnLoad(g);
            }
        }
         
        private List<TheardEvent> m_RunActionQueue = new List<TheardEvent>();

        private List<TheardEvent> m_ActionQueue = new List<TheardEvent>();

        private List<TheardEvent> m_DelayActionQueue = new List<TheardEvent>();

        private List<TheardEvent> m_RunDelayActionQueue = new List<TheardEvent>();

        public void StopAllThread()
        {

        }
        /// <summary>
        /// 主线程中执行
        /// </summary>
        /// <param name="ev"></param>
        public static void OnMainThreadQueue(TheardEvent ev)
        { 
            if (ev.waitTime > 0)
            {
                lock (m_Instence.m_DelayActionQueue)
                {
                    m_Instence.m_DelayActionQueue.Add(ev);
                }
            }
            else
            {
                lock (m_Instence.m_ActionQueue)
                {
                    m_Instence.m_ActionQueue.Add(ev);
                }
            } 
        } 
        /// <summary>
        /// 新建线程执行
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public static Thread RunAsync(TheardEvent ev)
        {
            Initialize();
            while (m_iRunThreadsNum >= m_iMaxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref m_iRunThreadsNum);//数线程加1
            ThreadPool.QueueUserWorkItem(RunAction, ev);
            return null;
        }
        /// <summary>
        /// 子线程回调
        /// </summary>
        /// <param name="ev"></param>
        private static void RunAction(object ev)
        {
            TheardEvent cache = (TheardEvent)ev; 
            try
            {
                if (cache.action == null)
                {
                    DebugMod.Log("theard action is null" + cache.name); 
                }
                cache.action(); 
            }
            catch
            {
                DebugMod.LogError("run thunder cathch" + cache.name);
            }
            finally
            {
                if(cache.callback != null)
                    cache.callback(); 
                Interlocked.Decrement(ref m_iRunThreadsNum);
            } 
        }
        /// <summary>
        /// 主线程回调
        /// </summary>
        /// <param name="ev"></param>
        private static void MainRunAction(object ev)
        {
            TheardEvent cache = (TheardEvent)ev;
            try
            {
                if (cache.action == null)
                {
                    DebugMod.Log("theard action is null" + cache.name);
                }
                cache.action();
            }
            catch
            {
                DebugMod.LogError("run thunder cathch" + cache.name);
            }
            finally
            {
                if (cache.callback != null)
                    cache.callback();
                Interlocked.Decrement(ref m_iRunThreadsNum);
            } 
        }

        // Update is called once per frame
        void Update()
        {
            lock (m_ActionQueue)
            {
                m_RunActionQueue.Clear();
                m_RunActionQueue.AddRange(m_ActionQueue);
                m_ActionQueue.Clear();
            }
            foreach (var ev in m_RunActionQueue)
            {
                MainRunAction(ev); 
            }
            //lock (m_DelayActionQueue)
            //{
            //    m_RunDelayActionQueue.Clear(); 
            //    m_RunDelayActionQueue.AddRange(m_DelayActionQueue.Where());
            //    foreach (var item in m_RunDelayActionQueue)

            //        m_DelayActionQueue.Remove(item);
            //}
            //foreach (var delayed in m_RunDelayActionQueue)
            //{
            //    delayed.Run();
            //} 
        }
    }
}