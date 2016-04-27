using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;
using System.Linq;

namespace SPSGame.Unity
{
    public class UIManager : Singleton<UIManager>
    {

        public GameObject uiRoot;
        public GameObject hudRoot;

        Dictionary<Type, UIWndBase> mWindowDic = new Dictionary<Type, UIWndBase>();

        private UILoading mLoading = null;
        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <returns></returns>
        public T ShowWindow<T>() where T:UIWndBase
        {
            if( mWindowDic.ContainsKey(typeof(T)) )
            {
                mWindowDic[typeof(T)].Show(true);
                return mWindowDic[typeof(T)] as T;
            }

            T t = ResourceManager.New<T>(string.Format("UI/{0}",GetNameFromType(typeof(T))));
            U3DMod.AddChild(uiRoot, t.gameObject);
            RegisterWindow(t);
            return t;
        }

        /// <summary>
        /// 创建Hud
        /// </summary>
        /// <returns></returns>
        public T CreateHud<T>() where T : UIObject
        {
            T t = ResourceManager.New<T>(string.Format("UI/{0}", GetNameFromType(typeof(T))));
            U3DMod.AddChild(hudRoot, t.gameObject);
            return t;
        }

        public UILoading ShowLoading()
        {
            if (mLoading != null)
                return mLoading;

            mLoading = ResourceManager.New<UILoading>(string.Format("UI/{0}", GetNameFromType(typeof(UILoading))));
            U3DMod.AddChild(uiRoot, mLoading.gameObject);
            mLoading.progresPersent = 0;
            return mLoading;
        }

        public void DestroyLoading()
        {
            if (mLoading != null)
                mLoading.Destroy();
        }

        string GetNameFromType( Type t )
        {
            string typestr = t.ToString();
            int index = typestr.LastIndexOf('.');
            if (index >= 0)
                return typestr.Substring(index + 1);
            else
                return typestr;
        }


        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        public T GetWindow<T>() where T : UIWndBase
        {
            if( mWindowDic.ContainsKey(typeof(T)) )
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


        void RegisterWindow(UIWndBase wnd)
        {
            Type t = wnd.GetType();
            mWindowDic[t] = wnd;
            wnd.DestroyWndHandler = (wd, e) => { RemoveWindow(wd); };
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
            foreach( Type t in mWindowDic.Keys.ToArray() )
            {
                if( mWindowDic[t] == wnd )
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
            foreach( Type t in mWindowDic.Keys.ToArray() )
            {
                mWindowDic[t].Destroy();
            }
            mWindowDic.Clear();
        }


        public void ClearAllHud()
        {
            U3DMod.SetActive(hudRoot,true);
            hudRoot.transform.DestroyChildren();
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="context">内容</param>
        /// <param name="style">类型</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static void MsgBox(string title, string context, MsgStyle style, MessageBoxResultDelegate callback)
        {
            UIMessageBox msg = ResourceManager.New<UIMessageBox>(string.Format("UI/UIMessageBox"));
            U3DMod.AddChild(Instance.uiRoot, msg.gameObject);
            msg.ShowMessageBox(title, context, style, callback);
        }

    }
}