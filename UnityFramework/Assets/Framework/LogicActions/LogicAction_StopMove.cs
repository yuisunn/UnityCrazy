using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class LogicAction_StopMove : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
                if (null != move)
                    move.OnEngineStopMove(ActParam);
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
