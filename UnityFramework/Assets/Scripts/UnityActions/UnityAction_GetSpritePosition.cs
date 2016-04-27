using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;


namespace SPSGame
{
    public class UnityAction_GetSpritePosition : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var uidStr = ActParam["SpriteID"];
                if (null == uidStr)
                    return false;
                int spriteid = -1;
                if (!int.TryParse(uidStr.ToString(), out spriteid))
                {
                    DebugMod.LogError("Get wrong uid int LoadSprite");
                    return false;
                }

                GSprite spr = ObjectManager.Instance.GetSprite(spriteid);
                Vector3 pos = spr.u3dObject.transform.position;
                Logicer.SendSpritePos(pos.x , pos.z);
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