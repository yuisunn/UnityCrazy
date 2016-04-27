using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using System.Collections;
using System.Threading;
using SPSGame.CsShare;
using SPSGame.Tools;

namespace SPSGame
{
    public class LoginAckData
    {
        public int result = 0;
        public string reason = "";
    }

    public class StageLogin : SPSStageBase
    {
        public static NextStageEventHandler NextStep;

        public StageLogin()
            :base("login")
        {

        }
        public override void AfterStageBegin()
        {
            try
            {
                GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
                if (null != gamesvr)
                {
                    gamesvr.Close();
                }

                ActionParam param = new ActionParam();
                param["GameStage"] = "login";
                Local.Instance.CallUnityAction(UnityActionDefine.ChangeStage, param);
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public override void BeforeStageEnd()
        {

        }

        public override void OnUpdate()
        {

        }

        //public override void BeginStage()
        //{
        //    base.BeginStage();
        //    //show scene and ui
        //    UIManager.Instance.ShowLoginUI();
        //}

        //public override void EndStage()
        //{
        //    base.EndStage();
        //    StageManager.Instance.BeginStage("selectline");
        //}

        //public override void OnInit()
        //{
        //    RegisterCmd("login", (int)SPSCmd.CMD_LOGIN_ACK, OnSC_LoginAck);
        //}

        //public void OnLoginReq(string nametxt, string pwdtxt)
        //{
        //    //请求登录
        //    DebugMod.Log("onloginreq");
        //}

    }
    
}
