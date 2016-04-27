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
    public class CMD_SC_GetNewItem : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
            {
                return;
            }

            //测试用代码
            MyPlayer myPlayer = Local.Instance.GetMyPlayer;

            if (null == myPlayer)
            {
                return;
            }

            long charid = myPlayer.CharID;

            writer.writeLong(charid);
            writer.writeInt32(actionParam.Get<int>("ItemNo"));
            writer.writeShort(actionParam.Get<short> ("Num"));
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

            List<ItemInfo> itemInfoList = new List<ItemInfo>();

            if (0 >= package.DataReader.Msg2NetDataList<ItemInfo>(itemInfoList))
            {
                return null;
            }

            MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

            if (null != myItemModule)
            {   
                //发送新增物品信息给界面
                ActionParam param = null;

                myItemModule.AddPlayerItemInfo(itemInfoList, out param);

                if (null == param)
                {
                    return null;
                }

                //发消息，将结果返回给背包综合界面
                Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);

                List<ActionParam> paramList = null;

                if (myItemModule.AddBagTabItemInfo (itemInfoList, out paramList))
                {
                    //发消息，将结果返回给分页签界面

                    if (null == paramList)
                    {
                        return null;
                    }

                    foreach (ActionParam p in paramList)
                    {
                        Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, p);
                    }
                }
            }

            return ActParam;
        }
    }
}
