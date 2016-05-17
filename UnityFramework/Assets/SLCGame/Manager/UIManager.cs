using UnityEngine;
using System.Collections;
using SLCGame.Unity;
using System.Collections.Generic;
using System;
using System.Linq;
using SLCGame.Tools;

namespace SLCGame.Unity
{
    /// <summary>
    /// 5.8增加ui缓存
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        public GameObject uiRoot;
        public GameObject hudRoot;

        Dictionary<Type, UIWndBase> mWindowDic = new Dictionary<Type, UIWndBase>();
        //处理缓存
        Queue<Type> cachQueue;

        public int WIN_MAX = 10;



        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <returns></returns>
        public T ShowWindow<T>() where T : UIWndBase
        { 
            if (mWindowDic.ContainsKey(typeof(T)))
            {
                mWindowDic[typeof(T)].Show(true); 
                return mWindowDic[typeof(T)] as T;
            } 
            T t = ResourceManager.New<T>(string.Format("Prefabs/Ui/Pop/{0}", GetNameFromType(typeof(T))));
            U3DMod.AddChild(uiRoot, t.gameObject);
            RegisterWindow(t);
            return t;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        public T GetWindow<T>() where T : UIWndBase
        {
            if (mWindowDic.ContainsKey(typeof(T)))
            {
                return mWindowDic[typeof(T)] as T;
            }
            return null;
        }


        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <returns></returns>
        public void HideWindow<T>() where T : UIWndBase
        {
            if (mWindowDic.ContainsKey(typeof(T)))
                mWindowDic[typeof(T)].Show(false);
        }


        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public void HideWindow(UIWndBase wnd)
        {
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                if (mWindowDic[t] == wnd)
                {
                    mWindowDic[t].Show(false);
                    break;
                }
            }
        }
        /// <summary>
        /// 增加了缓存
        /// </summary>
        /// <param name="wnd"></param>

        void RegisterWindow(UIWndBase wnd)
        { 
            wnd.Show();
            Type t = wnd.GetType();
            mWindowDic[t] = wnd;
            wnd.DestroyWndHandler = (wd, e) => { RemoveWindow(wd); };

            if (cachQueue != null)
            {
                cachQueue.Enqueue(t);
                if (mWindowDic.Count > WIN_MAX)
                {
                    Type ch = cachQueue.Dequeue();
                    DestroyWindow(mWindowDic[ch]);
                }
            }
            else
            {
                cachQueue = new Queue<Type>(); 
            }
        }


        /// <summary>
        /// 从字典移除窗口
        /// </summary>
        /// <returns></returns>
        public UIWndBase RemoveWindow<T>() where T : UIWndBase
        {
            UIWndBase wnd = null;
            if (mWindowDic.ContainsKey(typeof(T)))
            {
                wnd = mWindowDic[typeof(T)];
                mWindowDic.Remove(typeof(T));
            }
            return wnd;
        }


        /// <summary>
        /// 从字典移除窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public UIWndBase RemoveWindow(UIWndBase wnd)
        {
            UIWndBase _wnd = null;
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                if (mWindowDic[t] == wnd)
                {
                    _wnd = mWindowDic[t];
                    mWindowDic.Remove(t);
                    break;
                }
            }
            return _wnd;
        }


        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <returns></returns>
        public void DestroyWindow<T>() where T : UIWndBase
        {
            if (mWindowDic.ContainsKey(typeof(T)))
            {
                UIWndBase wnd = mWindowDic[typeof(T)];
                RemoveWindow<T>();
                wnd.Destroy();
            }
        }


        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public void DestroyWindow(UIWndBase wnd)
        {
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                if (mWindowDic[t] == wnd)
                {
                    mWindowDic.Remove(t);
                    wnd.Destroy();
                    break;
                }
            }
        }


        /// <summary>
        /// 销毁所有窗口
        /// </summary>
        /// <returns></returns>
        public void ClearAllWindow()
        {
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                mWindowDic[t].Destroy();
            }
            mWindowDic.Clear();
        }


        string GetNameFromType(Type t)
        {
            string typestr = t.ToString();
            int index = typestr.LastIndexOf('.');
            if (index >= 0)
                return typestr.Substring(index + 1);
            else
                return typestr;
        }
    }
}
