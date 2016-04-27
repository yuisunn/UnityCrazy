using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare;
using SPSGame.CsShare.Data;


namespace SPSGame
{
    public class CMD_SC_PlayerPos : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
            if (null != move)
                move.WriteMsgMoveTo(writer, actionParam);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;

            long charid = package.DataReader.getLong();
            bool visible = package.DataReader.getBool();
            short x = package.DataReader.getShort();
            short y = package.DataReader.getShort();
            short z = package.DataReader.getShort();
            bool dead = package.DataReader.getBool();
            int hpNow = package.DataReader.getInt();
            if (charid == Local.Instance.GetMyPlayer.CharID)
            {
                if (dead)
                    Local.Instance.GetMyPlayer.ToDead();
                else
                    Local.Instance.GetMyPlayer.Reborn();

                Local.Instance.GetMyPlayer.ChangeHp(hpNow);
                Local.Instance.GetMyPlayer.UpdateSpriteInfo();
            }
            else
            {
                PlayerObj player = scene.playerMng.GetPlayer(charid);
                if (null != player)
                {
                    if (dead)
                        player.ToDead();
                    else
                        player.Reborn();
                    player.ChangeHp(hpNow);
                    player.UpdateSpriteInfo();
                }
            }

            Local.Instance.SceneMgr.ForceSetPlayerPos(charid, x, y, z, visible);
           
            return ActParam;
        }

        //public override bool ProcessAction()
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        DebugMod.LogException(ex);
        //        return false;
        //    }
        //}
    }
}
