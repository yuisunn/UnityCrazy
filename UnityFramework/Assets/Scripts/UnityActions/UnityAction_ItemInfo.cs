using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ItemInfo: UnityAction
    {
       
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                Dictionary<string, string> itemInfo = ActParam["ItemInfo"] as Dictionary<string, string>;
                EventManager.Trigger<EventItemInfo>(new EventItemInfo(itemInfo));
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