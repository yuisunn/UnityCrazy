using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_GetItemDetailInfo : LogicAction
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

                long itemId = ActParam.Get<long> ("ItemId");

                ActionParam param = null;
                myItemModule.GetPlayerItemDetailInfoParam(itemId, out param);

                if (null == param)
                {
                    return false;
                }

                Dictionary<string, string> itemInfo = param["ItemInfo"] as Dictionary<string, string>;

                //调用UnityAction，将Param结果返回
                if (!Local.Instance.CallUnityAction(UnityActionDefine.ItemInfo, param))
                {
                    return false;
                }

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
