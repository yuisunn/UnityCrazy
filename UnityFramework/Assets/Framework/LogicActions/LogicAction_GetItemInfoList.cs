using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_GetItemInfoList : LogicAction
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

                if (null != myItemModule)
                {
                    if (myItemModule.IsGetBagItemInfo () && myItemModule.IsGetBagEquipInfo ())
                    {
                        //从myItemModule中取物品数据，返回界面
                        ActionParam param = null;
                        if (!myItemModule.GetPlayerBagInfoParam(out param))
                        {
                            return false;
                        }

                        Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);   

                        return true;
                    }
                }

                GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                if (null == gameServer)
                {
                    Local.Instance.CallActionFinish(LogicActionDefine.GetItemInfoList, 0);
                    return false;
                }

                gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_ItemInfo_List, ActParam, null);

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
