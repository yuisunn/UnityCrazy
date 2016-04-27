using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_RecvStudySkill : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var studyData = ActParam["Error"] ;
                if (studyData == null)
                {
                    return false;
                }

                var idskill = ActParam["IDSkill"] ;
                if (idskill == null)
                {
                    return false;
                }

                EventManager.Trigger<EventStudySkill>(new EventStudySkill((int)studyData,(int)idskill));
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
