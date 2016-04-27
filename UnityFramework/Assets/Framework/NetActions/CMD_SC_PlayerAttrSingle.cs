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
    public class CMD_SC_PlayerAttrSingle : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            List<PlayerAttrSingle> playerAttrSingleList = new List<PlayerAttrSingle>();

            if (0 >= package.DataReader.Msg2NetDataList<PlayerAttrSingle>(playerAttrSingleList))
            {
                return null;
            }

            MyPlayer myPlayer = Local.Instance.GetMyPlayer;

            if (null != myPlayer)
            {
                //为改变的人物属性赋值
                if (!myPlayer.UpdatePlayerAttr(playerAttrSingleList))
                {
                    return null;
                }
            }

            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
