using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;


namespace SPSGame
{
    public class UnityAction_LoadScene : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var uidStr = ActParam["SceneID"];
                if (null == uidStr)
                    return false;
                int sceneid = -1;
                if (!int.TryParse(uidStr.ToString(), out sceneid))
                {
                    DebugMod.LogError("Get wrong SceneID int LoadScene");
                    return false;
                }

                var resPar = ActParam["ResID"];
                if (null == resPar)
                    return false;
                int resid = -1;
                if (!int.TryParse(resPar.ToString(), out resid))
                {
                    DebugMod.LogError("Get wrong ResID int LoadScene");
                    return false;
                }

                EventManager.Trigger<EventLoadScene>(new EventLoadScene(sceneid,resid));
                
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