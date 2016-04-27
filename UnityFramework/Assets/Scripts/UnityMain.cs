using UnityEngine;
using System.Collections;
using System;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class UnityMain : Singleton<UnityMain>
    {

        public Threader Threader = new Threader();
        public Timers Timers = new Timers();
        public Coroutiner Coroutiner = new Coroutiner();

        public readonly SceneManager SceneManager = SceneManager.Instance;

        public static void UT(Action action)
        {
            UnityMain.Instance.Threader.QueueOnMainThread(action);
        }

        public static int StartCoroutine(IEnumerator coroutine,Coroutiner.ECoroutineLevel level = Coroutiner.ECoroutineLevel.High)
        {
            //Test_FakeLogic.Instance.StartCoroutine(coroutine);
            return Instance.Coroutiner.AddCoroutine(coroutine, level);
            //return 1;
        }

        public void Init()
        {       
            //UIManager.Instance.ShowLoginUI();
        }

        public void FixedUpdate()
        {
            RenderManager.Instance.FixedUpdate();
        }

        // Update is called once per frame
        public void Update()
        {
            //ResourceManager.Instance.Update();
            AssetBundleManager.Instance.Update();
            RenderManager.Instance.Update();

            Timers.Update();
            Threader.Update();
            Coroutiner.Update();
        }



        public void LateUpdate()
        {
            
            CameraManager.Instance.LateUpdate();
        }
    }
}