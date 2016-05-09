using UnityEngine;
using System.Collections;
using SLCGame.Unity;

namespace SLCGame.Unity
{
    public class UIMessageBox : UIWndBase
    {
        public UILabel lblNote;
        public UILabel title;
        public GameObject btnYes;
        public GameObject btnNo;
        public GameObject btnCancel;
        public GameObject btnNo2;
        public GameObject btnSubmit;


        private MessageBoxResultDelegate callback;

        protected override void Awake()
        {
            ListenOnClick(btnYes, OnSubmitHandler);
            ListenOnClick(btnNo, OnNoHandler);
            ListenOnClick(btnCancel, OnCancelHandler);
            ListenOnClick(btnNo2, OnNoHandler);
            ListenOnClick(btnSubmit, OnSubmitHandler);
            this.ResetButtons();
        }

        private void ResetButtons()
        {
            U3DMod.SetActive(btnYes, false);
            U3DMod.SetActive(btnNo, false);
            U3DMod.SetActive(btnCancel, false);
            U3DMod.SetActive(btnNo2,false);
            U3DMod.SetActive(btnSubmit,false);
        }

        public void ShowMessageBox(string title, string context, MsgStyle style, MessageBoxResultDelegate callback)
        {
            this.lblNote.text = context;
            this.callback = callback;
            this.title.text = title;

            this.ResetButtons();

            if (style == MsgStyle.Yes)
            {
                U3DMod.SetActive(btnSubmit, true);
            }
            else if (style == MsgStyle.YesAndNo)
            {
                U3DMod.SetActive(btnYes, true);
                U3DMod.SetActive(btnNo, true);
            }
            else
            {
                U3DMod.SetActive(btnNo2, true);
                U3DMod.SetActive(btnYes, true);
                U3DMod.SetActive(btnCancel, true);
            }

        }

        private void OnSubmitHandler(GameObject o)
        {
            U3DMod.SetActive(this.gameObject, false);
            if (callback != null)
            {
                callback.Invoke(MsgResult.Yes);
            }

            Destroy();
        }

        private void OnNoHandler(GameObject o)
        {
            U3DMod.SetActive(this.gameObject, false);
            if (callback != null)
            {
                callback.Invoke(MsgResult.No);
            }
            Destroy();
        }

        private void OnCancelHandler(GameObject o)
        {
            U3DMod.SetActive(this.gameObject, false);
            if (callback != null)
            {
                callback.Invoke(MsgResult.Cancel);
            }
            Destroy();
        }

    }
}