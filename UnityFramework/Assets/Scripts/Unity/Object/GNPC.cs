using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class GNPC:GSprite
    {
        UIHudNPCInfo mInfoUI = null;

        protected override void OnLoadResComplete()
        {
            base.OnLoadResComplete();

            mInfoUI = UIManager.Instance.CreateHud<UIHudNPCInfo>();
            if (mInfoUI != null)
            {
                UIFollowTarget flw = mInfoUI.gameObject.AddComponent<UIFollowTarget>();
                flw.target = u3dObject.transform;
                flw.extraHeight = 2.5f;
                U3DMod.SetActive(mInfoUI.gameObject, isShow);

                if (spriteInfo != null)
                    mInfoUI.SetInfo(spriteInfo.spriteName);
            }
            else
            {
                DebugMod.LogWarning("can't load UIHudNPCInfo");
            }

            GameObject shadow = ResourceManager.New("Prefab/Shadow");
            U3DMod.AddChild(u3dObject, shadow);
        }

    }
}