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
    public class CMD_Create_Character_Over : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {

        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            int error = package.DataReader.getInt();
            ActParam["error"] = error;

            if (error == 0)
            {
                long charid = package.DataReader.getLong();
                ActParam["charid"] = charid;
            }

            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                Local.Instance.CallActionFinish(LogicActionDefine.CreateChar, 0);

                int error = (int)ActParam["error"];
                if (error > 0)
                {
                    ShowErrorMsg(error);
                }
                else
                {
                    long charid = (long)ActParam["charid"];
                    DebugMod.Log("创建角色成功，id=" + charid);
                    Local.Instance.ShowUIMsg("创建角色成功", MsgPos.Dialog);

                    MyPlayer myplayer = Local.Instance.GetMyPlayer;
                    if (null != myplayer)
                    {
                        NetDataPlayerBase newplayer = myplayer.OnCreatePlayerOver(charid);
                        if (null != newplayer)
                        {
                            ActionParam param = new ActionParam();
                            param["CharID"] = newplayer.CharID;
                            param["FirstName"] = newplayer.FirstName;
                            param["LastName"] = newplayer.LastName;
                            param["CharClass"] = newplayer.CharClass;
                            param["CharGrade"] = 1;
                            param["VipLevel"] = newplayer.VipLevel;
                            param["CharLevel"] = newplayer.Level;
                            param["Map"] = newplayer.Map;
                            param["Cloth"] = newplayer.Cloth;
                            Local.Instance.CallUnityAction(UnityActionDefine.SendChar, param);
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Local.Instance.CallActionFinish(LogicActionDefine.CreateChar, 0);
                DebugMod.LogException(ex);
                return false;
            }
        }

        protected void ShowErrorMsg(int error)
        {
            DebugMod.Log("创建角色失败，原因：" + error);
            string msg = "创建角色失败";
            switch (error)
            {
                case 1:
                    msg += "，未登录";
                    break;
                case 2:
                    msg = "，此名称已被使用";
                    break;
                case 3:
                    msg = "，服务器状态异常";
                    break;
                case 4:
                    msg = "，没有多余的角色ID可分配";
                    break;
                case 5:
                    msg = "，角色数据库状态异常";
                    break;
                case 6:
                    msg = "，角色ID重复";
                    break;
                case 7:
                    msg = "，角色数据异常";
                    break;
                case 8:
                    msg = "，帐号数据库状态异常";
                    break;
                case 9:
                    msg = "，名字过长";
                    break;
                case 10:
                    msg = "，职业类型错误";
                    break;
            }

            Local.Instance.ShowUIMsg(msg, MsgPos.Dialog);
        }
    }
}
