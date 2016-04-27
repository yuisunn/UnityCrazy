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
    public class CMD_CS_LeaveBattle : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            writer.writeShort(0);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            long charid = package.DataReader.getLong();
            if (charid == Local.Instance.GetMyPlayer.CharID)
            {
                MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
                if (null != move)
                    move.OnAckLeaveBattle();
            }
            return ActParam;
        }

    }
}
