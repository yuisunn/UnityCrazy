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
    public class CMD_SC_MyPlayerStatus : NetAction
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

            MyPlayerStatus data = new MyPlayerStatus();
            if (0 >= package.DataReader.Msg2NetData<MyPlayerStatus>(data))
                return null;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null != myplayer)
                myplayer.OnRcvStatus(data);
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
