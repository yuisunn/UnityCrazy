using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_GetBagEquip : LogicAction
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

                long equipId = (long)ActParam["EquipId"];

                if (myItemModule.IsGetBagEquipInfo())
                {
                    ActionParam param = null;

                    if (!myItemModule.GetPlayerEquipChooseInfoParam(equipId, out param))
                    {
                        return false;
                    }

                    if (null == param)
                    {
                        return false;
                    }

                    //调用UnityAction发送ActionParam
                    Local.Instance.CallUnityAction(UnityActionDefine.ShowPlayerChooseEquip, param);

                    return true;

                }
                else
                {
                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        Local.Instance.CallActionFinish(LogicActionDefine.GetBagEquip, 0);
                        return false;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_GetBagEquipInfo, ActParam, null);

                    return true;
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
