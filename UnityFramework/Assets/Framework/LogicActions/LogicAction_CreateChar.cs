using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class LogicAction_CreateChar : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
                if (null == gamesvr)
                    return false;
                if (!gamesvr.Connected())
                {
                    Local.Instance.ShowUIMsg("未连接服务器", MsgPos.Dialog);
                    Local.Instance.CallActionFinish(LogicActionDefine.CreateChar, 0);
                    return false;
                }
                
                //<string>("FirstName"));	//前缀名，玩家不能修改，只能随机，前缀名是个对应图标的代号例如"a01"，在表现层显示的是"a01"所对应的图标而不是"a01"
                //<string>("LastName"));	//后缀名，可随机也可由玩家自己起名
                //<short>("CharClass"));	//职业类型，1~N
                gamesvr.ReadySend(CsShare.SPSCmd.CMD_Create_Character, ActParam, null);
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
