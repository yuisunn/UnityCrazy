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
    public class CMD_CS_EquipInfo_List : NetAction
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
            writer.writeShort(actionParam.Get<short>("EquipPlanNo"));
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

            short equipPlanNo = package.DataReader.getShort(); //解析获取的装备方案编号

            List<EquipInfo> equipInfoList = new List<EquipInfo>();

            if (0 >= package.DataReader.Msg2NetDataList<EquipInfo>(equipInfoList))
            {
                return null;
            }

            MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

            if (null != myItemModule)
            {
                myItemModule.OnRcvPlayerEquiInfo(equipInfoList, equipPlanNo);
            }

            return ActParam;
        }
    }
}
