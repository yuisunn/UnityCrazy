using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ButtonEvent : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var eventPar = ActParam["ButtonEventType"];
                if (null == eventPar)
                    return false;

                int buttonevent = -1;
                ButtonEventType type;
                if( int.TryParse(eventPar.ToString(),out buttonevent) )
                {
                    type = (ButtonEventType)buttonevent;
                }
                else
                {
                    DebugMod.LogError("can't get ButtonEventType in ButtonEvent");
                    return false;
                }

                EventManager.Trigger<EventButtonEvent>(new EventButtonEvent(type));
                
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