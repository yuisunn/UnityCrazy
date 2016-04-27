using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_ReturnToLogin : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                Local.Instance.StageMgr.BeginStage("login");
                Local.Instance.SceneMgr.OnReset();
                Local.Instance.GetMyPlayer.Clear();
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
