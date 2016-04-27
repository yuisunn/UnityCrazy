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
    public class CMD_SC_MonsterOut : NetAction
    {
        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int seq = package.DataReader.getInt();
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;

            scene.monsterMng.MonsterOut(seq);
           
            return ActParam;
        }

    }
}
