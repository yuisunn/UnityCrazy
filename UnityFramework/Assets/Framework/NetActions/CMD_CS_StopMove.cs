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
    public class CMD_CS_StopMove : NetAction
    {
        protected ParamStopMove dd = new ParamStopMove();
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
            if (null != move)
                move.WriteMsgStopMove(writer, actionParam);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            long charid = package.DataReader.getLong();
            
            dd.CurrX = package.DataReader.getShort();
            dd.CurrZ = package.DataReader.getShort();

            OtherPlayerMove module = Local.Instance.GetModule("othermove") as OtherPlayerMove;
            if (null != module)
                module.OnServerStopMove(charid, dd);

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
