using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_GetBagTabItemInfo : LogicAction
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

                 if (myItemModule.IsGetBagItemInfo() && myItemModule.IsGetBagEquipInfo())
                 {
                     short type = (short)ActParam["Type"]; //表示物品的类别

                     //从myItemModule中取物品数据，返回界面
                     ActionParam param = null;
                     if (!myItemModule.GetBagTabItemInfo(type, out param))
                     {
                         return false;
                     }

                     Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);

                     return true;
                 }

                 return false;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
