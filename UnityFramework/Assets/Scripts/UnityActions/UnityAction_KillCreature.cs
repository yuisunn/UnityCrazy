using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_KillCreature : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var creturepar = ActParam["CreatureID"];
                int ceratureid = -1;
                if (creturepar != null && !int.TryParse(creturepar.ToString(), out ceratureid))
                {
                }

                EventManager.Trigger<EventKillCreature>(new EventKillCreature(ceratureid));
                
                //EventManager.Trigger<EventActSprite>(new EventActSprite(spriteid, actid, speed/100f,able));
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