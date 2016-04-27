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
    public class CMD_CS_ChangeMap : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
            if (null != move)
                move.WriteMsgChangeMap(writer, actionParam);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int sceneId = package.DataReader.getInt();
            int sceneSeq = package.DataReader.getInt();
            bool visible = package.DataReader.getBool();
            Local.Instance.GetMyPlayer.SetVisible(visible);
            short x = package.DataReader.getShort();
            short y = package.DataReader.getShort();
            short z = package.DataReader.getShort();
            short width = package.DataReader.getShort();
            short height = package.DataReader.getShort();
            short layer = package.DataReader.getShort();
            Local.Instance.GetMyPlayer.SetPos(x, y, z);
            Local.Instance.SceneMgr.OnRcvSceneSize(sceneId, width, height, layer);
            Local.Instance.SceneMgr.OnRcvChangeSceneEnd(sceneId, sceneSeq);
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
