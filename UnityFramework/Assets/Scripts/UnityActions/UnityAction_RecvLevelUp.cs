using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_RecvLevelUp : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                LocalDataToC upGrade = ActParam["NewData"] as LocalDataToC;
                if (upGrade == null)
                {
                    return false;
                }

                if (null == ActParam)
                    return false;
                var upError = ActParam["Error"];
                if (upError == null)
                {
                    return false;
                }

                EventManager.Trigger<EventUpSkill>(new EventUpSkill(upGrade, (int)upError));
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
