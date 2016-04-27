using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class EventManager : Singleton<EventManager>
    {

        public delegate void EventDelegate<T>(T t) where T : EventArgs;

        private Dictionary<Type, Delegate> mEventDic = new Dictionary<Type, Delegate>();

        public static void Register<T>(EventDelegate<T> evtfunc) where T : EventArgs
        {
            Delegate del;
            if (Instance.mEventDic.TryGetValue(typeof(T), out del))
            {
                Instance.mEventDic[typeof(T)] = Delegate.Combine(del, evtfunc);            }
            else
            {
                Instance.mEventDic[typeof(T)] = evtfunc;
            }
        }


        public static void Remove<T>(EventDelegate<T> evtfunc) where T : EventArgs
        {
            Delegate del;
            if (Instance.mEventDic.TryGetValue(typeof(T), out del))
            {
                Delegate after = Delegate.Remove(del, evtfunc);
                if (after == null)
                    Instance.mEventDic.Remove(typeof(T));
            }
        }


        public static void Trigger<T>(T e) where T : EventArgs
        {
            Delegate del;
            if (Instance.mEventDic.TryGetValue(typeof(T), out del))
            {
                EventDelegate<T> callback = del as EventDelegate<T>;
                try
                {
                    if (callback != null)
                        callback(e);
                }
                catch
                {
                    DebugMod.LogError("Event Trigger error on:" + typeof(T).ToString());
                }

            }
        }

    }
}