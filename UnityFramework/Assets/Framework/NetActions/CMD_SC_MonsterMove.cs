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
    public class CMD_SC_MonsterMove : NetAction
    {
        protected ParamMoveTo dd = new ParamMoveTo();
        //public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        //{
        //    MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
        //    if (null != move)
        //        move.WriteMsgMoveTo(writer, actionParam);
        //}

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int monsterSeq = package.DataReader.getInt();
            short speed = package.DataReader.getShort();
            dd.CurrX = package.DataReader.getShort();
            dd.CurrZ = package.DataReader.getShort();
            dd.DestX = package.DataReader.getShort();
            dd.DestZ = package.DataReader.getShort();


            MonsterMove module = Local.Instance.GetModule("monmove") as MonsterMove;
            if (null != module)
                module.OnLogicMoveTo(monsterSeq, dd, speed);

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
