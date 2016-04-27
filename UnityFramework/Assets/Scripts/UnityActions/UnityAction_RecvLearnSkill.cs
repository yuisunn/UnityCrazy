using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_RecvLearnSkill : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                List<LocalDataToC> DataList = ActParam["CharInfo"] as List<LocalDataToC>;
                Dictionary<int, int> LeanDic = ActParam["ChosenInfo"] as Dictionary<int, int>;


                DebugMod.Log(DataList[0].CharStageName);

                EventManager.Trigger<EventLeanSkill>(new EventLeanSkill(DataList, LeanDic));
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
