using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class LogicAction_ButtonEvent : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                SPSGame.Unity.ButtonEventType type = (SPSGame.Unity.ButtonEventType)ActParam["ButtonEvent"];
                if (type == Unity.ButtonEventType.LeaveBattle)
                {
                    MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
                    if (null != move)
                        move.OnEngineLeaveBattle();
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
