using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class LogicAction_UseSkill : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                var tmp = ActParam["SkillID"];
                if (null == tmp)
                    return false;
                int skillid = (int)tmp;

                MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
                if (null != fight)
                {
                    fight.OnUseSkill(skillid);
                }
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
