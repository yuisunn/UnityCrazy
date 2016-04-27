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
    public class CMD_SC_PlayerDead : NetAction
    {

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            long charid = package.DataReader.getLong();
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;
            if (charid == Local.Instance.GetMyPlayer.CharID)
            {
                Local.Instance.GetMyPlayer.ToDead();
            }
            else
            {
                PlayerObj player = scene.playerMng.GetPlayer(charid);
                if (null != player)
                    player.ToDead();
            }
            return ActParam;
        }

    }
}
