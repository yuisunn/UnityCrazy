using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_UseItem : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

                if (null == myItemModule)
                {
                    return false;
                }

                int type = myItemModule.GetItemType(ActParam.Get<long>("ItemID")); ; 

                if (1 == type)
                {
                    ActParam["UseType"] = (short)130;
                }
                else if (0 == type)
                {
                    if (1 == ActParam.Get<int>("IsUse"))
                    {
                        ActParam["UseType"] = (short)110;
                    }
                    else
                    {
                        ActParam["UseType"] = (short)120;
                    }
                }
                else
                {
                    return false;
                }

                GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                if (null == gameServer)
                {
                    Local.Instance.CallActionFinish(LogicActionDefine.UseItem, 0);
                    return false;
                }

                gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_UseItem, ActParam, null);

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
