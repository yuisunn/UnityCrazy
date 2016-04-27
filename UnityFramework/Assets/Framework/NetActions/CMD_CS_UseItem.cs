using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class CMD_CS_UseItem : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
            {
                return;
            }

            MyPlayer myPlayer = Local.Instance.GetMyPlayer;

            if (null == myPlayer)
            {
                return;
            }

            long charid = myPlayer.CharID;

            writer.writeLong(charid);
            writer.writeShort(actionParam.Get<short>("Num"));
            writer.writeLong(actionParam.Get<long>("ItemID"));
            writer.writeShort(actionParam.Get<short>("UseType"));
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
            {
                return null;
            }

            if (null == package.DataReader)
            {
                return null;
            }

            long itemId = package.DataReader.readInt64();

            short itemNum = package.DataReader.getShort();

            MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

            if (null != myItemModule)
            {
                ActionParam tabParam = null;
                if (myItemModule.ReduceBagTabItemInfo(itemId, itemNum, out tabParam))
                {
                    if (null == tabParam)
                    {
                        return null;
                    }

                    //发消息，将结果返回给分页签界面
                    Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, tabParam);
                }

                ActionParam param = null;
                if (!myItemModule.ReducePlayerItemInfo(itemId, itemNum, out param))
                {
                    return null;
                }

                if (null == param)
                {
                    return null;
                }

                //发消息，将结果返回给背包综合界面
                Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);
                
            }

            return ActParam;
        }
    }
}
