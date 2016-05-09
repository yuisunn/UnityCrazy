using UnityEngine;
using System.Collections;
using System; 


namespace SLCGame.Unity
{
    public class UIWndBase : UIObject
    {

        public WndDestroyDelegate DestroyWndHandler = null;

        public WndSelectedDelegate SelectedWndHandler = null;

        public VoidDelegate CloseWndHandler = null;

        public virtual void Init()
        {
            //EventManager.Instance.Regiter<EventRoleDataRefresh>(RefreshUI);
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