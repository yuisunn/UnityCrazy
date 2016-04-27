using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare;

namespace SPSGame
{
    public class CMD_NotifyError : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            ActParam["ActionId"] = package.DataReader.getInt();
            ActParam["Reason"] = package.DataReader.getInt();
            ActParam["Msg"] = package.DataReader.readString();
            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                SPSCmd cmd = (SPSCmd)ActParam["ActionId"];
                int reason = (int)ActParam["Reason"];
                string msg = ActParam["Msg"] as string;

                switch (cmd)
                {
                    case SPSCmd.CMD_Enter_Game:
                        ShowEnterGameError(reason, msg);
                        break;
                    case SPSCmd.CMD_CS_ChangeMap:
                        ShowChangeSceneError(reason, msg);
                        break;
                }
                return true;
            }
            catch(Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        protected void ShowChangeSceneError(int reason, string msg)
        {
            if (reason == 1)
            {
                Local.Instance.ShowUIMsg("场景人满，请尝试切换到其它线路", MsgPos.Dialog);
            }
        }

        protected void ShowEnterGameError(int reason, string msg)
        {
            EnterGameError error = (EnterGameError)reason;
            switch(error)
            {
                case EnterGameError.CharNotExist:
                    Local.Instance.ShowUIMsg("角色数据错误", MsgPos.Dialog);
                    break;
                case EnterGameError.CharNotExistDB:
                    Local.Instance.ShowUIMsg("角色数据错误", MsgPos.Dialog);
                    break;
                case EnterGameError.DBServerError:
                    Local.Instance.ShowUIMsg("服务器状态异常", MsgPos.Dialog);
                    break;
                case EnterGameError.LineFull:
                    Local.Instance.ShowUIMsg("服务器人数已满", MsgPos.Dialog);
                    break;
                case EnterGameError.NetDataError:
                    Local.Instance.ShowUIMsg("服务器状态异常", MsgPos.Dialog);
                    break;
                case EnterGameError.NoLine:
                    Local.Instance.ShowUIMsg("服务器状态异常", MsgPos.Dialog);
                    break;
                case EnterGameError.NoLogin:
                    Local.Instance.ShowUIMsg("未登录", MsgPos.Dialog);
                    break;
            }
        }
    }
}
