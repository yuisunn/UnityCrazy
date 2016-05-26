using UnityEngine;
using System.Collections;

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
        // Update is called once per frame
        void Update()
        {

        }
    }
}
