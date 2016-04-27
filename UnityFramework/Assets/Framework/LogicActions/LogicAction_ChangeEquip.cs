using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_ChangeEquip : LogicAction
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
                    Local.Instance.CallActionFinish(LogicActionDefine.ChangeEquip, 0);
                    return false;
                }

                gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_ChangeEquip, ActParam, null);

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
