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
    public class CMD_SC_GetNewEquip : NetAction
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
            writer.writeInt32(actionParam.Get<int>("EquipNo"));
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

            List<EquipInfo> equipInfoList = new List<EquipInfo>();

            if (0 >= package.DataReader.Msg2NetDataList<EquipInfo>(equipInfoList))
            {
                return null;
            }

            MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

            if (null != myItemModule)
            {
                //发送新增装备信息给界面
                ActionParam param = null;

                myItemModule.AddPlayerEquipInfo(equipInfoList, out param);

                if (null == param)
                {
                    return null;
                }

                //发消息，将结果返回给背包综合界面
                Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);

                ActionParam tabParam = null;

                if (myItemModule.AddBagTabEquipInfo (equipInfoList, out tabParam))
                {
                    if (null == tabParam)
                    {
                        return null;
                    }

                    //发消息，将结果返回给背包分页签界面
                    Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, tabParam);
                }
            }

            return ActParam;
        }
    }
}
