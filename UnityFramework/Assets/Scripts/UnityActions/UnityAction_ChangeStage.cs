using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ChangeStage : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var stageNameTemp = ActParam["GameStage"];
                if (null == stageNameTemp)
                    return false;

                string stageName = stageNameTemp.ToString().ToLower();
                switch (stageName)
                {
                    case "startup":
                        SPSGame.Unity.StageManager.Instance.ChangeGameStage(EGameStage.StartUp);               
                        break;
                    case "update":
                        SPSGame.Unity.StageManager.Instance.ChangeGameStage(EGameStage.Update);
                        break;
                    case "login":
                        SPSGame.Unity.StageManager.Instance.ChangeGameStage(EGameStage.Login);
                        break;
                    case "selectrole":
                        SPSGame.Unity.StageManager.Instance.ChangeGameStage(EGameStage.SelectRole);
                        break;
                    case "gaming":
                        SPSGame.Unity.StageManager.Instance.ChangeGameStage(EGameStage.Gaming);
                        break;
                    default:
                        break;
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