using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_RecvChosenSkill : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                Dictionary<int, ChosenSkillData> chonseDic = ActParam["ChosenData"] as Dictionary<int, ChosenSkillData>;

                EventManager.Trigger<EventChosenSkill>(new EventChosenSkill(chonseDic));
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
