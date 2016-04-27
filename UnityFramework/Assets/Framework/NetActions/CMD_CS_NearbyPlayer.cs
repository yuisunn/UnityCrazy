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
    public class CMD_CS_NearbyPlayer : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;
            var varTmp = actionParam["listReq"];
            if (null == varTmp)
                return;
            List<long> listReq = varTmp as List<long>;
            foreach (var charid in listReq)
            {
                writer.writeLong(charid);
            }
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int sceneSeq = package.DataReader.getInt();
            List<NearbyPlayerID> list = new List<NearbyPlayerID>();
            package.DataReader.Msg2NetDataList<NearbyPlayerID>(list);

            Local.Instance.SceneMgr.OnRcvNearbyPlayerID(sceneSeq, list);
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
