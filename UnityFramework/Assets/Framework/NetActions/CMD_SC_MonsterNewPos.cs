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
    public class CMD_SC_MonsterNewPos : NetAction
    {
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

            int monsterSeq = package.DataReader.getInt();
            short x = package.DataReader.getShort();
            short z = package.DataReader.getShort();

            Local.Instance.SceneMgr.ForceSetMonsterPos(monsterSeq, x, z);
           
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
