using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_BackPackInfo : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                short type = (short)ActParam["Type"];
                var sprName = ActParam["PackInfo"];
                List<Dictionary<string, string>> packInfo = sprName as List<Dictionary<string, string>>;
                EventManager.Trigger<EventPackInfo>(new EventPackInfo(type, packInfo));
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