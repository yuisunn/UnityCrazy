using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_HurtSprite : UnityAction
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

                var typepar = ActParam["HurtType"];
                if (null == typepar)
                    return false;
                int type = -1;
                if (!int.TryParse(typepar.ToString(), out type))
                {
                    DebugMod.LogError("Can't get HurtType int HurtSprite");
                    return false;
                }

                var numpar = ActParam["HurtNumber"];
                if (null == numpar)
                    return false;
                int num = -1;
                if (!int.TryParse(numpar.ToString(), out num))
                {
                    DebugMod.LogError("Can't get HurtNumber int HurtSprite");
                    return false;
                }

                EventManager.Trigger<EventHurtSprite>(new EventHurtSprite(spriteid, (EHurtType)type,num));
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