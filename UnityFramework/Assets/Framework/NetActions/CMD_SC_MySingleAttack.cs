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
    public class CMD_SC_MySingleAttack : NetAction
    {
        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            MySingleAttackParam param = new MySingleAttackParam();
            package.DataReader.Msg2NetData<MySingleAttackParam>(param);

            MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
            if (null != fight)
            {
                fight.OnServerAttack(param);
            }
            return ActParam;
        }

    }
}
