using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_HitSprite : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                int hurtID = (int)ActParam["CreatureID"];
                int targetSprID = (int)ActParam["SpriteID"];

                GameScene scene = Local.Instance.SceneMgr.GetMyScene();
                if (null == scene)
                    return false;

                HurtObj hurtObj = scene.hurtManager.GetObj(hurtID);
                if (null == hurtObj)
                    return false;

                hurtObj.OnBulletHit(targetSprID);
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
