using UnityEngine;
using System.Collections;
using SLCGame.Unity;
using System.Collections.Generic;
using System;
using System.Linq;
using SLCGame.Tools;
using SLCGame.Tools.Unity;

namespace SLCGame.Unity
{
    /// <summary>
    /// 5.8增加ui缓存
    /// </summary>
    public class UIMgr : Singleton<UIMgr>
    {
        public delegate void WndHideByMove();
        public WndHideByMove hideByMoveFun;

        private GameObject uiRoot;
        public GameObject UiRoot
        {
            get
            {
                return uiRoot;
            }

            set
            {
                uiRoot = value;
            }
        }

        //缓存整个界面
        Dictionary<Type, UIWndBase> mWindowDic = new Dictionary<Type, UIWndBase>(); 
        //缓存context
        Dictionary<Type, ContextBase> mContextDic = new Dictionary<Type, ContextBase>();


        Queue<Type> mWindowQueue;
        Queue<Type> mContextQueue;
        public int hideMax = 10;
        public int contextMax = 10;
          
        /// <summary>
        /// 显示窗口 
        /// 1用缓存
        /// 2用context数据
        /// 3重写生成
        /// </summary>
        /// <returns></returns>
        public T ShowWindow<T>(GameObject root=null) where T : UIWndBase
        { 
            if (mWindowDic.ContainsKey(typeof(T)))
            {
                mWindowDic[typeof(T)].Show();
                return mWindowDic[typeof(T)] as T;
            }
            else
            {
                //待修改
                GameObject obj = AssetBundleMgr.Instance.LoadUIPerfab(GetNameFromType(typeof(T))); 
                if (root == null)
                    U3DMod.AddChild(UiRoot, obj);
                else
                    U3DMod.AddChild(root, obj);
                T t = obj.AddComponent<T>();
                t.OnInit();
                RegisterWindow(t);

                if (mContextDic.ContainsKey(typeof(T)))
                    t.RefreshData(mContextDic[typeof(T)]);
                else
                    RegisterContext(typeof(T), t.RefreshData());
                t.Show();
                return t;
            } 
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        public T GetWindow<T>() where T : UIWndBase
        {
            UIWndBase t;
            mWindowDic.TryGetValue(typeof(T), out t); 
            return t as T;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        public T GetContext<T>() where T : ContextBase
        {
            ContextBase t;
            mContextDic.TryGetValue(typeof(T), out t); 
            return t as T;
        }


        /// <summary>
        /// 隐藏窗口 指定方法
        /// </summary>
        /// <returns></returns>
        public void HideWindow<T>(HideWndTypeEnum type) where T : UIWndBase
        {
            UIWndBase wnd =null;
            if (mWindowDic.TryGetValue(typeof(T), out wnd))
                wnd.HideWnd(type); 
        }

        /// <summary>
        /// 使用默认或设定好的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideWindow<T>() where T : UIWndBase
        {
            UIWndBase wnd = null;
            if (mWindowDic.TryGetValue(typeof(T), out wnd))
                wnd.HideWnd();
        }

        /// <summary>
        /// 隐藏全部窗口 默认或设定的方式
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public void HideWindow(UIWndBase wnd)
        {
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                if (mWindowDic[t] == wnd)
                {
                    mWindowDic[t].HideWnd();
                    break;
                }
            }
        }
         
        /// <summary>
        /// 注册窗口 保存缓存
        /// </summary>
        /// <param name="wnd"></param> 
        void RegisterWindow(UIWndBase wnd)
        {   
            Type t = wnd.GetType();
            mWindowDic.Add(t,wnd);
            mContextDic.Add(t, wnd.m_Context);
              
            if (mWindowQueue != null)
            {
                mWindowQueue.Enqueue(t);
                if (mWindowQueue.Count > hideMax)
                {
                    Type type = mWindowQueue.Dequeue();
                    mWindowDic.Remove(type);
                    DestroyWindow(type); 

                }
            } 
            // wnd.DestroyWndHandler = (wd, e) => { RemoveWindow(wd); };
        }
        /// <summary>
        /// 注册上下文
        /// </summary>
        /// <param name="t"></param>
        /// <param name="wnd"></param>
        public void RegisterContext(Type t, ContextBase wnd)
        {
            if (mContextQueue != null)
            {
                mContextQueue.Enqueue(t);
                if (mContextQueue.Count > contextMax)
                {
                    Type type = mContextQueue.Dequeue();
                    mContextDic.Remove(type); 
                }
            }
        }

        /// <summary>
        /// 销毁context
        /// </summary>
        /// <param name="t"></param>
        public void DestoryContext(Type t)
        {
            mContextDic.Remove(t);
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public void DestroyWindow(Type t)
        {  
           mWindowDic[t].Destroy();
           if(mWindowDic[t].hideType == HideWndTypeEnum.Destory)
           {
                DestoryContext(t);
           } 
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public void DestoryWindow<T>() 
        {
            Type t = typeof(T);
            mWindowDic[t].Destroy();
            if (mWindowDic[t].hideType == HideWndTypeEnum.Destory)
            {
                DestoryContext(t);
            }
        }


        /// <summary>
        /// 销毁所有窗口 和context
        /// </summary>
        /// <returns></returns>
        public void ClearAllWindow()
        {
            foreach (Type t in mWindowDic.Keys.ToArray())
            {
                mWindowDic[t].Destroy(); 
            }
            mContextDic.Clear();
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
