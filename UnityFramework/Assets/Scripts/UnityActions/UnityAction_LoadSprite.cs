using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_LoadSprite : UnityAction
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

                var resID = ActParam["ResID"];
                if (null == resID)
                    return false;
                int rid = -1;
                if (!int.TryParse(resID.ToString(), out rid))
                {
                    DebugMod.LogError("Get wrong resid int LoadSprite");
                    return false;
                }

                var type = ActParam["SpriteTypeID"];
                if (null == type)
                    return false;
                int typeid = -1;
                if (!int.TryParse(type.ToString(), out typeid))
                {
                    DebugMod.LogError("Get wrong typeid int LoadSprite");
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
                if (null == posy)
                    return false;
                short dy = -1;
                if (!short.TryParse(diry.ToString(), out dy))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var dirz = ActParam["DirZ"];
                if (null == posz)
                    return false;
                short dz = -1;
                if (!short.TryParse(dirz.ToString(), out dz))
                {
                    DebugMod.LogError("Get wrong x int LoadSprite");
                    return false;
                }

                var showparm = ActParam["Show"];
                if (null == showparm)
                    return false;
                int show = -1;
                if (!int.TryParse(showparm.ToString(), out show))
                {
                    DebugMod.LogError("Get wrong show int LoadSprite");
                    return false;
                }

                bool isshow = show == 0 ? false : true;

                EventManager.Trigger<EventLoadSprite>(new EventLoadSprite(spriteid, rid, typeid, new Vector3(px / 10f, (float)py, pz / 10f), new Vector3(dx / 100f, dy / 100f, dz / 100f), isshow));
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