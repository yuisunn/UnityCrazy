using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;


namespace SPSGame.Unity
{
    public class UIWndSelectLine : UIWndBase
    {
        public GameObject backBtn;
        public GameObject enterGameBtn;
        public GameObject[] linesBtn;
        public GameObject[] LineSelectedSpr;
        public GameObject[] serversBtn;
        public GameObject[] serverSelectSpr;
        public GameObject[] serverLab;
        Dictionary<GameObject, string> serverDic = new Dictionary<GameObject, string>();
        //Dictionary<GameObject, string> lineDic = new Dictionary<GameObject, string>();
        private string severStr = "";
        private string lineStr = "";
        public VoidDelegate OnEnterGameHandler = null;

        protected override void Start()
        {
            base.Start();

            ListenOnClick(backBtn, OnClickBack);
            ListenOnClick(enterGameBtn, OnClickEnterGame);

            for (int i = 0; i < linesBtn.Length; i++)
            {
                serverDic.Add(linesBtn[i], i.ToString());
                ListenOnClick(linesBtn[i], OnClickLine);
            }

            for (int i = 0; i < serversBtn.Length; i++)
            {
                serverDic.Add(serversBtn[i], i.ToString());
                ListenOnClick(serversBtn[i], OnClickserver);
            }
        }
        void OnClickBack(GameObject obj)
        {
            Show(false);
        }


        void OnClickEnterGame(GameObject obj)
        {
            if (OnEnterGameHandler != null)
                OnEnterGameHandler();
        }

        /// <summary>
        /// 选择线路
        /// </summary>
        /// <param name="obj"></param>
        void OnClickLine(GameObject obj)
        {
            if (serverDic.ContainsKey(obj))
            {
                Logicer.RequestLineGroup( SelecteLine(obj) );
            }
            if (lineStr == "0")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[0], true);
                ServerChangeLab(1);
            }
            if (lineStr == "1")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[1], true);
                ServerChangeLab(11);
            }
            if (lineStr == "2")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[2], true);
                ServerChangeLab(21);
            }
            if (lineStr == "3")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[3], true);
                ServerChangeLab(31);
            }
            if (lineStr == "4")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[4], true);
                ServerChangeLab(41);
            }
            if (lineStr == "5")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[5], true);
                ServerChangeLab(51);
            }
            if (lineStr == "6")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[6], true);
                ServerChangeLab(61);
            }
            if (lineStr == "7")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[7], true);
                ServerChangeLab(71);
            }
            if (lineStr == "8")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[8], true);
                ServerChangeLab(81);
            }
            if (lineStr == "9")
            {
                DeselectLine();
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                U3DMod.SetActive(LineSelectedSpr[9], true);
                ServerChangeLab(91);
            }
        }
        private string SelecteLine(GameObject obj)
        {
            serverDic.TryGetValue(obj, out lineStr);
            return lineStr;
        }
        /// <summary>
        /// 取消选中线路
        /// </summary>
        void DeselectLine()
        {
            for (int i = 0; i < LineSelectedSpr.Length; i++)
            {
                U3DMod.SetActive(LineSelectedSpr[i], false);
            }
        }
        /// <summary>
        /// 选择服务器
        /// </summary>
        /// <param name="obj"></param>
        void OnClickserver(GameObject obj)
        {
            if (serverDic.ContainsKey(obj))
            {
                SelectServer(obj);
            }
            if (severStr == "0")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[0], true);
                
            }
            if (severStr == "1")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[1], true);
                
            }
            if (severStr == "2")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[2], true);
              
            }
            if (severStr == "3")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[3], true);
             
            }
            if (severStr == "4")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[4], true);
               
            }
            if (severStr == "5")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[5], true);
                
            }
            if (severStr == "6")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[6], true);
              
            }
            if (severStr == "7")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[7], true);
               
            }
            if (severStr == "8")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[8], true);
                
            }
            if (severStr == "9")
            {
                DeselectServer();
                U3DMod.SetActive(serverSelectSpr[9], true);
               
            }

        }
        private string SelectServer(GameObject obj)
        {
            serverDic.TryGetValue(obj, out severStr);
            return severStr;
        }
        /// <summary>
        /// 取消选择服务器
        /// </summary>
        void DeselectServer()
        {
            for (int i = 0; i < serverSelectSpr.Length; i++)
            {
                U3DMod.SetActive(serverSelectSpr[i], false);
            }
        }
        /// <summary>
        /// 服务器变化
        /// </summary>
        /// <param name="number"></param>
        void ServerChangeLab(int number) 
        {
            for (int i = 0; i < serverLab.Length; i++)
            {
                serverLab[i].GetComponent<UILabel>().text = (i + number).ToString();
            }
        }
    }
}