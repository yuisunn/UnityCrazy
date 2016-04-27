using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;


namespace SPSGame.Unity
{

    public class UIWndFlag : UIWndBase
    {

       
        public GameObject backBtn;
       
      

        protected override void Awake()
        {
            base.Awake();
            ListenOnClick(backBtn, OnClickClose);
        }



    }
}