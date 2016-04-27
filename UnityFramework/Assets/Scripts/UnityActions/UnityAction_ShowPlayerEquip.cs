using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ShowPlayerEquip : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                List<Dictionary<string, string>> info = ActParam["PlayerEquip"] as List<Dictionary<string, string>>;

                EventManager.Trigger<EventShowPLayerEquipInfo>(new EventShowPLayerEquipInfo(info));

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
