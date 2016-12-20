using UnityEngine;
using System.Collections; 

namespace SLCGame.Unity
{
    public class UINWndClose : UIWndBase
    {

        public GameObject close;
        // Use this for initialization
        void Awake()
        {
            //ListenOnClick(close, CloseClick);
        }
         

        void CloseClick(GameObject obj)
        {
            OnClickClose(obj);
        }
    }
}
