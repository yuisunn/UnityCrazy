using UnityEngine;
using System.Collections;
using SLCGame.Unity;

namespace SLCGame
{
    /// <summary>
    /// 用来介入 sdk等
    /// </summary>
    public class AppMain : MonoBehaviour
    {
        public void InitializeMode()
        { }
        public void InitializeUnity()
        { }
        public void InitializeGame()
        {
            GameMain.Instance.Init();
        }
        public void InitializeWindowTitle()
        { }
        public void InitializeOptionValues()
        {
        }

        public void Awake()
        { 
            InitializeMode();
            InitializeWindowTitle();
            InitializeOptionValues();
            InitializeGame();
            InitializeUnity();
        }

        public void HotUpdate()
        {
            HotUpdateMgr.Instance.CheckExtractResource();
        }

        public void Start()
        {
            HotUpdate();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
