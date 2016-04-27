using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class LogicAction_ChangeMapResult : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
                if (null != move)
                    move.OnEngineLoadSceneFinish(ActParam);

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
