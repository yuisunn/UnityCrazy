using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ControlSpriteHud : UnityAction
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
                int spriteid = -100;
                if (!int.TryParse(uidStr.ToString(), out spriteid))
                {
                    DebugMod.LogError("Can't get SpriteID int HurtSprite");
                    return false;
                }


                var hudpar = ActParam["ShowHud"];
                if (null == hudpar)
                    return false;
                bool show = false;
                if (!bool.TryParse(hudpar.ToString(), out show))
                {
                    DebugMod.LogError("Can't get HurtNumber int HurtSprite");
                    return false;
                }

                EventManager.Trigger<EventShowHud>(new EventShowHud(spriteid, show));
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