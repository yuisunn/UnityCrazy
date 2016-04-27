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
    public class CMD_CS_ItemResort : NetAction
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

            MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

            if (null == myItemModule)
            {
                return null;
            }

             List<ItemLocationChange> locationChangeList = new List<ItemLocationChange>();

            if (0 >= package.DataReader.Msg2NetDataList<ItemLocationChange>(locationChangeList))
            {
                return null;
            }

            //将物品信息返回给界面
            ActionParam param = null;
            if (myItemModule.OnRcvPlayerItemResortInfo(locationChangeList, out param))
            {
                Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);
            }

            return ActParam;
        }
    }
}
