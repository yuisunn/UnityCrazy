using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace SPSGame.Unity
{
    public class Threader
    {

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }

        public static int maxThreads = 8;
        static int numThreads;

        List<Action> _actions = new List<Action>();

        List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        List<Action> _currentActions = new List<Action>();


        public void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }


        public void QueueOnMainThread(Action action, float time)
        {
            if (time != 0)
            {
                lock (_delayed)
                {
                    _delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (_actions)
                {
                    _actions.Add(action);
                }
            }
        }

        public Thread RunAsync(Action a)
        {
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref numThreads);//以原子操作的形式递增指定变量的值并存储结果
            ThreadPool.QueueUserWorkItem(RunAction, a); //在线程池可执行是执行
            return null;
        }

        private void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);//原子递减
            }

        }


        // Update is called once per frame
        public void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            foreach (var a in _currentActions) //执行 action
            {
                a();
            }


            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where((d) => d.time <= Time.time)); //list里的条件筛选
                foreach (var item in _currentDelayed) //删除 _currentDelayed 和 _delayed 重复的
                    _delayed.Remove(item);
            }
            foreach (var delayed in _currentDelayed) //执行 延时 action
            {
                delayed.action();
            }
        }

    }
}