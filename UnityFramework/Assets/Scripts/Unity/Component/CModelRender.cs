using UnityEngine;
using System.Collections;

using System;

namespace SPSGame.Unity
{

    public class CModelRender : U3DComponent
    {
        void OnBecameVisible()
        {
            UIWndGaming gaming = UIManager.Instance.GetWindow<UIWndGaming>();
            if(gaming != null)
            {
                gaming.SetImageTopShow(false);
                gaming.SetImageBottomShow(false);
            }
               
        }

        void OnBecameInvisible()
        {
            UIWndGaming gaming = UIManager.Instance.GetWindow<UIWndGaming>();
            if (gaming != null && CameraManager.Instance.camera3D != null)
            {
                if(transform.position.y > CameraManager.Instance.camera3D.transform.position.y )
                    gaming.SetImageTopShow(true);
                else
                    gaming.SetImageBottomShow(true);
            }
        }

    }
}