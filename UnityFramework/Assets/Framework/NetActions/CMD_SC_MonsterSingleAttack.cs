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
    public class CMD_SC_MonsterSingleAttack : NetAction
    {
        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            ParamMonsterAttack pp = new ParamMonsterAttack();
            pp.monSeq = package.DataReader.getInt();
            pp.charid = package.DataReader.getLong();
            pp.hurttype = package.DataReader.getShort();
            pp.playerHp = package.DataReader.getInt();
            MonsterAttack module = Local.Instance.GetModule("monatt") as MonsterAttack;
            if (module != null)
            {
                module.OnServerMonsterAttack(pp);
            }
           
            return ActParam;
        }

    }
}
