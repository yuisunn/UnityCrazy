using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;
using UnityEngine;

namespace SPSGame
{
    public class UnityAction_ControlPlayer : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                bool able = false;
                var ablepar = ActParam["ControlAble"];

                if (null != ablepar && bool.TryParse(ablepar.ToString(), out able))
                {
                    EventManager.Trigger<EventPlayerControlAble>(new EventPlayerControlAble(able));
                }

                int speed = -1;
                var speedpar = ActParam["ControlSpeed"];

                if (null != speedpar && int.TryParse(speedpar.ToString(), out speed))
                {
                    EventManager.Trigger<EventPlayerControlSpeed>(new EventPlayerControlSpeed(speed / 10f));
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