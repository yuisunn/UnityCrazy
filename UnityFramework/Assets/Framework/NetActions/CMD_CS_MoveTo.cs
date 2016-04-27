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
    public class CMD_CS_MoveTo : NetAction
    {
        protected ParamMoveTo dd = new ParamMoveTo();
        protected List<MovePoint> listMove = new List<MovePoint>();
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
            if (null != move)
                move.WriteMsgMoveTo(writer, actionParam);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            long charid = package.DataReader.getLong();
            listMove.Clear();
            package.DataReader.Msg2NetDataList<MovePoint>(listMove);
            if (listMove.Count != 2)
            {
                DebugMod.LogError("MovePoint 列表数量错误：" + listMove.Count);
                return null;
            }


            dd.CurrX = listMove[0].X;
            dd.CurrZ = listMove[0].Z;
            dd.DestX = listMove[1].X;
            dd.DestZ = listMove[1].Z;

            OtherPlayerMove module = Local.Instance.GetModule("othermove") as OtherPlayerMove;
            if (null != module)
                module.OnLogicMoveTo(charid, dd);

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
