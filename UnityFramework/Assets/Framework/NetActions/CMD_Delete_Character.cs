using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    //创建角色
    public class CMD_Delete_Character : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam || null == writer )
                return;

            writer.writeLong(actionParam.Get<long>("CharID"));
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;


            ActParam["CharID"] = package.DataReader.getLong();
            ActParam["Error"] = package.DataReader.getByte();
            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                long charid = (long)ActParam["CharID"];
                byte error = (byte)ActParam["Error"];
                if (error > 0)
                {
                    ShowErrorMsg(charid, error);
                    return true;
                }

                Local.Instance.ShowUIMsg("删除角色成功", MsgPos.Dialog);
                Local.Instance.CallActionFinish(LogicActionDefine.DeleteChar, charid);

                MyPlayer myplayer = Local.Instance.GetMyPlayer;
                if (null != myplayer)
                {
                    NetDataPlayerBase pb = myplayer.GetMyPlayerBase(charid);
                    if (null != pb)
                    {
                        myplayer.OnDeletePlayer(charid);

                        DebugMod.Log("角色删除成功，id=" + charid);

                        ActionParam param = new ActionParam();
                        param["CharID"] = charid;
                        param["CharClass"] = pb.CharClass;
                        Local.Instance.CallUnityAction(UnityActionDefine.DeleteChar, param);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                Local.Instance.CallActionFinish(LogicActionDefine.DeleteChar, 0);
                DebugMod.LogException(ex);
                return false;
            }
        }

        protected void ShowErrorMsg(long charid, byte error)
        {
            DebugMod.Log("角色删除失败，id=" + charid + "，原因：" + error);
            string msg = "删除角色失败";
            switch (error)
            {
                case 1:
                    msg += "，未登录";
                    break;
                case 2:
                    msg = "，帐号不存在";
                    break;
                case 3:
                    msg = "，帐号数据异常";
                    break;
                case 4:
                    msg = "，帐号数据库异常";
                    break;
                case 10:
                    msg = "，角色不存在";
                    break;
                case 11:
                    msg = "，角色早已被删除";
                    break;
                case 12:
                    msg = "，帐号ID不匹配";
                    break;
                case 13:
                    msg = "，角色数据库异常";
                    break;
            }

            Local.Instance.ShowUIMsg(msg, MsgPos.Dialog);
        }
    }
}
