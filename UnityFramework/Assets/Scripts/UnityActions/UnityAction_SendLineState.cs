using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_SendLineState : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var stateStr = ActParam["LineState"];
                if (null == stateStr)
                    return false;

                string linestr = stateStr.ToString();
                string[] strs = linestr.Split(',');
                int[] ints = new int[strs.Length];
                for (int i = 0; i < ints.Length; ++i)
                {
                    if (!int.TryParse(strs[i], out ints[i]))
                    {
                        DebugMod.LogError("Get wrong uid int LoadSprite");
                        return false;
                    }
                }

                EventManager.Trigger<EventSendLineState>(new EventSendLineState(ints));
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