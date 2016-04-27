using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class CMD_SC_EnterGame : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            //if (null == actionParam)
            //    return;
            //writer.writeLong(actionParam.Get<long>("CharID"));
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            Local.Instance.CallActionFinish(LogicActionDefine.EnterGame, 0);

            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int serverId = package.DataReader.getInt();
            Local.Instance.OnEnterGame(serverId);
            return ActParam;
        }

        //public override bool ProcessAction()
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        DebugMod.LogException(ex);
        //        return false;
        //    }
        //}
    }
}
