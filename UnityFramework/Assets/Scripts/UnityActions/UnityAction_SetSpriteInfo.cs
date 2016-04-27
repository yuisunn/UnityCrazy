using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_SetSpriteInfo : UnityAction
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
                    DebugMod.LogError("Can't get SpriteID in HurtSprite");
                    return false;
                }

                var namepar = ActParam["Name"];
                if (null == namepar)
                    return false;
                string name = namepar.ToString();

                var levelpar = ActParam["Level"];
                if (null == levelpar)
                    return false;
                int level = -1;
                if (!int.TryParse(levelpar.ToString(), out level))
                {
                    DebugMod.LogError("Can't get Level in SetSpriteInfo");
                    return false;
                }

                var maxpar = ActParam["MaxHealth"];
                if (null == maxpar)
                    return false;
                int max = -1;
                if (!int.TryParse(maxpar.ToString(), out max))
                {
                    DebugMod.LogError("Can't get MaxHealth in SetSpriteInfo");
                    return false;
                }

                var currentpar = ActParam["CurrentHealth"];
                if (null == currentpar)
                    return false;
                int current = -1;
                if (!int.TryParse(currentpar.ToString(), out current))
                {
                    DebugMod.LogError("Can't get CurrentHealth in SetSpriteInfo");
                    return false;
                }

                EventManager.Trigger<EventSetSpriteInfo>(new EventSetSpriteInfo(spriteid, name, level,max,current));
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