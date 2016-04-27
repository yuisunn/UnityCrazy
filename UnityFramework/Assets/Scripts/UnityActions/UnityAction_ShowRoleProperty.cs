using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ShowRoleProperty : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                EventManager.Trigger<EventRoleBaseProperty>(new EventRoleBaseProperty(ActParam.Get<short>("Force"), ActParam.Get<short>("Speed"), ActParam.Get<short>("Wit"), ActParam.Get<short>("ForceUp"), ActParam.Get<short>("SpeedUp"), ActParam.Get<short>("WitUp")));
                EventManager.Trigger<EventRoleDetailProperty>(new EventRoleDetailProperty(ActParam.Get<int>("MaxHp"), ActParam.Get<int>("PhyATK"), ActParam.Get<int>("MagicATK"), ActParam.Get<int>("PhysicProtect"), ActParam.Get<int>("MagicProtect"), ActParam.Get<int>("PhyCrit"), ActParam.Get<int>("CritHit")));

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
