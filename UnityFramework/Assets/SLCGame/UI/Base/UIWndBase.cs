using UnityEngine;
using System.Collections;
using System;
using SLCGame.Tools.Unity;

namespace SLCGame.Unity
{
    public enum HideWndTypeEnum
    { 
        Move = 1,
        Context = 2,
        Destory =3
    }
    public class UIWndBase : UIObject 
    {
        public HideWndTypeEnum hideType; 

        public ContextBase m_Context;

        public WndDestroyDelegate DestroyWndHandler = null;

        public WndSelectedDelegate SelectedWndHandler = null;

        public VoidDelegate CloseWndHandler = null;

        public virtual void Init()
        { 

        }

        public virtual void Show()
        { 

        }
        /// <summary>
        /// 指定隐藏类型
        /// </summary>
        /// <param name="type"></param>
        public virtual void HideWnd(HideWndTypeEnum type)
        {
            if(type == HideWndTypeEnum.Move)
                UIMgr.Instance.hideByMoveFun();
        }
        /// <summary>
        /// 使用默认或自己预先设定的
        /// </summary>
        public virtual void HideWnd()
        {
            if (this.hideType == HideWndTypeEnum.Move)
                UIMgr.Instance.hideByMoveFun();
        }

        public virtual void DestoryWnd()
        {
            Destroy();
        }

        public virtual void RefreshUI()
        {
            
        }
        public virtual void RefreshData(ContextBase context)
        {

        }

        public virtual ContextBase RefreshData()
        {
            return null;
        }

        protected virtual void OnClickClose(GameObject obj)
        {
            if (CloseWndHandler != null)
                CloseWndHandler();
        }

        public virtual void Destroy()
        {
            if (DestroyWndHandler != null)
                DestroyWndHandler(this, new EventWndDestroy()); 
        }

        public void OnEnter(ContextBase context)
        { 
        }

        public void OnExit(ContextBase context)
        { 
        }

        public void OnPause(ContextBase context)
        { 
        }

        public void OnResume(ContextBase context)
        { 
        }
    }
}