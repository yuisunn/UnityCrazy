using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ActSprite : UnityAction
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
                    DebugMod.LogError("Get wrong SpriteID int ActSprite");
                    return false;
                }


                var act = ActParam["ActID"];
                if (null == act)
                    return false;
                int actid = -1;
                if (!int.TryParse(act.ToString(), out actid))
                {
                    DebugMod.LogError("Get wrong ActID int ActSprite");
                    return false;
                }

                var spd = ActParam["Speed"];
                if (null == spd)
                    return false;
                int speed = -1;
                if (!int.TryParse(spd.ToString(), out speed))
                {
                    DebugMod.LogError("Get wrong Speed int ActSprite");
                    return false;
                }


                EventManager.Trigger<EventActSprite>(new EventActSprite(spriteid, actid, speed/100f));
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