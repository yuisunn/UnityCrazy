using SLCGame.Tools.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLCGame.Unity
{
    public class TreeWndBase : UIWndBase
    { 
        /// <summary>
        /// 所有子窗口
        /// </summary>
        protected List<TreeWndBase> m_WinChilds; 

        protected TreeWndBase RootWnd
        {
            get
            {
                return rootWnd;
            }

            set
            {
                rootWnd = value;
            }
        }

        private TreeWndBase rootWnd;


        public override void Init()
        {
            base.Init();
            this.hideType = HideWndTypeEnum.Move;
        }
        /// <summary>
        /// 隐藏全部子窗口
        /// </summary>
        protected void HideAllChildWnd()
        {
            if (m_WinChilds.Count < 1) return;
            for (int i = 0; i < m_WinChilds.Count; i++)
                m_WinChilds[i].HideWnd();
        }

        /// <summary>
        /// 销毁全部子窗口
        /// </summary>
        protected void DestoryAllChildWnd()
        {
            if (m_WinChilds.Count < 1) return;
            for (int i = 0; i < m_WinChilds.Count; i++)
                m_WinChilds[i].DestoryWnd();
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public override void HideWnd()
        {
            base.HideWnd(); 
            HideAllChildWnd();
        }
        /// <summary>
        /// 销毁窗口
        /// </summary>
        public override void DestoryWnd()
        {
            base.DestoryWnd();
            Destroy();
            DestoryAllChildWnd();
        }

        public override void RefreshUI()
        {
            base.RefreshUI();
            for (int i = 0; i < m_WinChilds.Count; i++)
                m_WinChilds[i].RefreshUI();
        }
         
    }
}