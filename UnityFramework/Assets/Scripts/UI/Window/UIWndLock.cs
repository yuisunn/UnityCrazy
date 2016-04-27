using UnityEngine;
using System.Collections;
using SPSGame.Tools;

namespace SPSGame.Unity 
{
    public class UIWndLock : UIWndBase
    {
       
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public GameObject backBtn = null;
        public GameObject sureBtn = null;
        //public GameObject undoBtn = null;
        UIWndBackPack backPack = new UIWndBackPack();
        public int zuanShiNum = 700;
        protected override void Awake()
        {
            base.Awake();
          
        }
        protected override void Start()
        {
            base.Start();
            ListenOnClick(backBtn, OnClickBack);
            ListenOnClick(sureBtn, OpenCell);
        }
        
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBack(GameObject obj) 
        {
            Hide();
        }
        void OpenCell(GameObject obj) 
        {
            if (65 <= zuanShiNum)
            {
                zuanShiNum -= 65;
                Debug.Log(zuanShiNum);
            }
        }
    }
}

