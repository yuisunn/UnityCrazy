using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class CMD_Login_Game : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;
            writer.writeString(actionParam.Get<string>("username"));
            writer.writeString(actionParam.Get<string>("password"));
            writer.writeString("sps"); //渠道名
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            ActParam["msg"] = package.DataReader.readString();
            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                Local.Instance.CallActionFinish(LogicActionDefine.LoginServer, 0);

                string msg = (string)ActParam["msg"];
                string[] fields = msg.Split(':');
                int userid = Convert.ToInt32(fields[0]);

                if (userid == -1)
                {
                    DebugMod.ShowNotifyMsg("错误的用户名或密码");
                }
                else if (userid == -2)
                {
                    DebugMod.ShowNotifyMsg("已经登录");
                }
                else
                {
                    List<long> listID = new List<long>();
                    for (int i = 1; i < fields.Length; i++ )
                    {
                        long charid = Convert.ToInt64(fields[i]);
                        if (charid > 0)
                            listID.Add(charid);
                    }

                    DebugMod.Log("登录成功,userid=" + userid + ",角色数量=" + listID.Count());

                    MyPlayer myplayer = Local.Instance.GetMyPlayer;
                    if (null != myplayer)
                        myplayer.OnLogin(userid, listID);

                    if (listID.Count == 0)
                    {
                        //通知渲染层进入选择角色阶段
                        //通知渲染层进入选择角色阶段
                        Local.Instance.StageMgr.BeginStage("selectrole");
                    }
                }

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
