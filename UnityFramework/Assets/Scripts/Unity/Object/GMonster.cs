using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class GMonster:GSprite
    {
        protected UIHudMonsterInfo mInfoUI = null;
        
        public override void Show(bool isshow)
        {
            base.Show(isshow);
        }

        public override void ShowInfoUI(bool isshow)
        {
            if (mInfoUI != null)
            {
                mInfoUI.ShowBlood(mIsShow);
            }
        }

        protected override void OnLoadResComplete()
        {
            base.OnLoadResComplete();

            mInfoUI = UIManager.Instance.CreateHud<UIHudMonsterInfo>();
            if (mInfoUI != null)
            {
                UIFollowTarget flw = mInfoUI.gameObject.AddComponent<UIFollowTarget>();
                flw.target = u3dObject.transform;

                string heigthstr = DataManager.Instance.GetModelData(resID, "HudHeight");
                float height = 0;
                if (float.TryParse(heigthstr, out height))
                {
                    hudHeight = height;
                    flw.extraHeight = height;
                }

                if (spriteInfo!= null)
                    mInfoUI.SetInfo(spriteInfo.spriteName, spriteInfo.spriteLevel, spriteInfo.maxHealth, spriteInfo.currentHealth);

                mInfoUI.ShowBlood(false);
            }
            else
            {
                DebugMod.LogWarning("can't load UIHudMonsterInfo");
            }

            GameObject shadow = ResourceManager.New("Prefab/Shadow");
            U3DMod.AddChild(u3dObject, shadow);
        }


        public override void SetSpriteInfo(SpriteInfo info)
        {
            base.SetSpriteInfo(info);

            if( mInfoUI != null )
            {
                mInfoUI.SetInfo(info.spriteName, info.spriteLevel, info.maxHealth, info.currentHealth);
            }
        }

    }
}