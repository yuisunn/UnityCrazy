using UnityEngine;
using System.Collections;
using System;
using SLCGame.Tools.Unity;

namespace SLCGame.Unity
{
    public class UIWndBase : UIObject
    {

        public WndDestroyDelegate DestroyWndHandler = null;

        public WndSelectedDelegate SelectedWndHandler = null;

        public VoidDelegate CloseWndHandler = null;

        public virtual void Init()
        {
            //EventMgr.Register<>(RefreshUI);
        }

        public virtual void HideWnd()
        {
            U3DMod.SetActive(this.gameObject, false);
        }

        public virtual void DestoryWnd()
        {
            Destroy();
        }

        public virtual void RefreshUI()
        {
            
        }

        protected virtual void OnClickClose(GameObject obj)
        {
            if (CloseWndHandler != null)
                CloseWndHandler();
        }

        public override void Destroy()
        {
            if (DestroyWndHandler != null)
                DestroyWndHandler(this, new EventWndDestroy());
            else
                base.Destroy();
            base.Destroy();
        }

    }
}