using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_LocatSprite : UnityAction
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


                var posx = ActParam["PosX"];
                if (null == posx)
                    return false;
                short px = -1;
                if (!short.TryParse(posx.ToString(), out px))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var posy = ActParam["PosY"];
                if (null == posy)
                    return false;
                short py = -1;
                if (!short.TryParse(posy.ToString(), out py))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var posz = ActParam["PosZ"];
                if (null == posz)
                    return false;
                short pz = -1;
                if (!short.TryParse(posz.ToString(), out pz))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var dirx = ActParam["DirX"];
                if (null == dirx)
                    return false;
                short dx = -1;
                if (!short.TryParse(dirx.ToString(), out dx))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var diry = ActParam["DirY"];
                if (null == diry)
                    return false;
                short dy = -1;
                if (!short.TryParse(diry.ToString(), out dy))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var dirz = ActParam["DirZ"];
                if (null == dirz)
                    return false;
                short dz = -1;
                if (!short.TryParse(dirz.ToString(), out dz))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                EventManager.Trigger<EventLocatSprite>(new EventLocatSprite(spriteid, new Vector3(px / 10f, (float)py, pz / 10f), new Vector3(dx / 100f, dy / 100f, dz / 100f)));
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