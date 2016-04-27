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
    public class CMD_SC_PlayerOut : NetAction
    {

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            long charid = package.DataReader.getLong();
            int sceneSeq = package.DataReader.getInt();
            if (charid == Local.Instance.GetMyPlayer.CharID)
            {
                Local.Instance.GetMyPlayer.OnDisapear();
            }
            else
                Local.Instance.SceneMgr.OnPlayerOut(charid, sceneSeq);
           
            return ActParam;
        }

    }
}
