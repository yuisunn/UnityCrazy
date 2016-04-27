using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_LoadCreature : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var creturepar = ActParam["CreatureID"];
                int ceratureid = -1;
                if (creturepar != null && !int.TryParse(creturepar.ToString(), out ceratureid))
                {
                }

                var creturetypepar = ActParam["CreatureType"];
                int creturetype = -1;
                if (creturepar != null && !int.TryParse(creturetypepar.ToString(), out creturetype))
                {
                }

                var respar = ActParam["ResID"];
                int resid = -1;
                if (respar != null && !int.TryParse(respar.ToString(), out resid))
                {
                }

                var sprpar = ActParam["SpriteID"];
                int sprid = -1;
                if (sprpar != null && !int.TryParse(sprpar.ToString(), out sprid))
                {
                }

                var targetsprid = ActParam["TargetSpriteID"];
                int targetid = -1;
                if (targetsprid != null && !int.TryParse(targetsprid.ToString(), out targetid))
                {
                }

                var speedpar = ActParam["Speed"];
                int speed = -1;
                if (speedpar != null && !int.TryParse(speedpar.ToString(), out speed))
                {
                }

                var lifepar = ActParam["Life"];
                float life = -1;
                if (lifepar != null && !float.TryParse(lifepar.ToString(), out life))
                {
                }

                var posx = ActParam["PosX"];
                short px = -1;
                if (null != posx && !short.TryParse(posx.ToString(), out px))
                {
                }

                var posy = ActParam["PosY"];
                short py = -1;
                if (null != posy&&!short.TryParse(posy.ToString(), out py))
                {
                }

                var posz = ActParam["PosZ"];
                short pz = -1;
                if (null != posy && !short.TryParse(posz.ToString(), out pz))
                {
                }

                var dirx = ActParam["DirX"];
                short dx = -1;
                if (null != dirx && !short.TryParse(dirx.ToString(), out dx))
                {
                }

                var diry = ActParam["DirY"];
                short dy = -1;
                if (null != posy && !short.TryParse(diry.ToString(), out dy))
                {
                }

                var dirz = ActParam["DirZ"];
                short dz = -1;
                if (null != posy && !short.TryParse(dirz.ToString(), out dz))
                {
                }

                var hitpar = ActParam["HitOnce"];
                bool once = false;
                if (null != posy && !bool.TryParse(hitpar.ToString(), out once))
                {
                }

                //DebugMod.LogWarning( "Load Creature:"+ resid);
                switch( (ECreatureType)creturetype)
                {
                    case ECreatureType.Bullet:
                        EventManager.Trigger<EventLoadCreatureBullet>(new EventLoadCreatureBullet(ceratureid, resid, sprid, targetid, speed/10f, life));
                        break;
                    case ECreatureType.Cross:
                        EventManager.Trigger<EventLoadCreatureCross>(new EventLoadCreatureCross(ceratureid, resid, sprid, new UnityEngine.Vector3(dx / 10f, dx / 10f, dx / 10f), speed, life, once));
                        break;
                    case ECreatureType.Hang:
                        EventManager.Trigger<EventLoadCreatureHang>(new EventLoadCreatureHang(ceratureid, resid, sprid, life));
                        break;
                    case ECreatureType.Point:
                        EventManager.Trigger<EventLoadCreaturePoint>(new EventLoadCreaturePoint(ceratureid, resid,new UnityEngine.Vector3(px/10f,py/10f,pz/10f), life));
                        break;
                    default:
                        break;
                }
                
                //EventManager.Trigger<EventActSprite>(new EventActSprite(spriteid, actid, speed/100f,able));
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