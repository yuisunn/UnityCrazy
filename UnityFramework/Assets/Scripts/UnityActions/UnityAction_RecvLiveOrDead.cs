using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_RecvLiveOrDead : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var lifestate = ActParam["LifeState"];
                if (lifestate == null)
                {
                    return false;
                }

                EventManager.Trigger<EventToDeathOrReborn>(new EventToDeathOrReborn(lifestate.ToString()));
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
