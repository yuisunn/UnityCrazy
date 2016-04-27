using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ShowPlayerChooseEquip : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                List<Dictionary<string, string>> info = ActParam["ChooseEquipInfo"] as List<Dictionary<string, string>>;

                EventManager.Trigger<EventShowChooseEquipInfo>(new EventShowChooseEquipInfo(info));

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
