using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_LoginServer : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var username = ActParam["username"];
                if (null == username)
                    return false;
                var password = ActParam["password"];
                if (null == password)
                    return false;

                GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
                if (null == gamesvr)
                    return false;

                var ip = ActParam["ip"];
                if (null != ip)
                {
                    string strIp = (string)ip;
                    if (strIp.Length > 0)
                        gamesvr.SetIPAddress(strIp);
                }

                if (!gamesvr.Connected())
                {
                    gamesvr.Connect(1);

                    Action<SPSNetEventArgs, object> newAction = (args, userData) =>
                        {
                            ActionParam param = userData as ActionParam;
                            gamesvr.ReadySend(CsShare.SPSCmd.CMD_Login_Game, param, null);
                        };

                    Local.Instance.SvrMgr.AddNetEventNextStep("game", NetEventType.Connect_Success, newAction, ActParam);
                    return true;
                }
                else
                {
                    gamesvr.ReadySend(CsShare.SPSCmd.CMD_Login_Game, ActParam, null);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
