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
    public class CMD_SC_MyPlayerInfo : NetAction
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

            MyPlayerInfo data = new MyPlayerInfo();
            if (0 >= package.DataReader.Msg2NetData<MyPlayerInfo>(data))
                return null;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null != myplayer)
                myplayer.SetInfo(data);
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
