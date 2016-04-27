using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;


namespace SPSGame.Unity
{

    public class UIWndSelectRole : UIWndBase
    {

        public UILabel playerNameLabel;
        public UILabel roleNameLabel;
        public GameObject selectBtn;
        public GameObject backBtn;
        public GameObject deleteBtn;

        public VoidDelegate DeleteCharHandler = null;
        public VoidDelegate SelectRoleHandler = null;

        protected override void Start()
        {
            base.Start();

            ListenOnClick(selectBtn, OnClickSelectRole);
            ListenOnClick(backBtn, OnClickBack);
            ListenOnClick(deleteBtn, OnClickDeleteChar);
        }


        void OnClickSelectRole(GameObject obj)
        {
            if (SelectRoleHandler != null)
                SelectRoleHandler();
        }

        void OnClickDeleteChar( GameObject obj )
        {
            if (DeleteCharHandler != null)
                DeleteCharHandler();
        }

        void OnClickBack(GameObject obj)
        {
            Show(false);
        }


    }
}