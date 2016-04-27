using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_DeleteChar : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
                if (null == gamesvr)
                    return false;
                if (!gamesvr.Connected())
                {
                    Local.Instance.CallActionFinish(LogicActionDefine.DeleteChar, 0);
                    return false;
                }

                gamesvr.ReadySend(CsShare.SPSCmd.CMD_Delete_Character, ActParam, null);
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
