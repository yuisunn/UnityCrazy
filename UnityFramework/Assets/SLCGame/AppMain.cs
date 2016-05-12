using UnityEngine;
using System.Collections;

namespace SLCGame
{
    /// <summary>
    /// 用来介入 sdk等
    /// </summary>
    public class AppMain : MonoBehaviour
    { 
        // Use this for initialization
        void Start()
        {
            GameMain.Instance.Init();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
