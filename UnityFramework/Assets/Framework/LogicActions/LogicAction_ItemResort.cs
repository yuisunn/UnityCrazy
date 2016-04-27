using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_ItemResort : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                if (null == gameServer)
                {
                    return false;
                }

                gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_ItemResort, ActParam, null);

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
