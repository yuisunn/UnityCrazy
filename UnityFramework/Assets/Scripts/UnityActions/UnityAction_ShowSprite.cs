using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ShowSprite : UnityAction
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
                    DebugMod.LogError("Get wrong spriteid int ShowSprite");
                    return false;
                }

                var showparm = ActParam["Show"];
                if (null == showparm)
                    return false;
                int show = -1;
                if (!int.TryParse(showparm.ToString(), out show))
                {
                    DebugMod.LogError("Get wrong show int ShowSprite");
                    return false;
                }

                bool isshow = show == 0 ? false : true;

                EventManager.Trigger<EventShowSprite>(new EventShowSprite(spriteid, isshow));
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