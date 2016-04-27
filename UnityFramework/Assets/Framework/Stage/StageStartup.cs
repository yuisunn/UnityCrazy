using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using System.Collections;
using System.Threading;
using SPSGame.Tools;

namespace SPSGame
{

    public class StageStartup : SPSStageBase
    {
        public StageStartup()
            :base("startup")
        {

        }
        public override void AfterStageBegin()
        {
            ShowStartVideo();

            ActionParam param = new ActionParam();
            param["GameStage"] = "startup";
            Local.Instance.CallUnityAction(UnityActionDefine.ChangeStage, param);
        }

        public virtual void ShowStartVideo()
        {

        }

        public virtual bool IfStartVideoEnd()
        {
            return true;
        }

        public override void BeforeStageEnd()
        {

        }

        public override void OnUpdate()
        {
            //DebugMod.Log("StageStartup update.");
            if (IfStartVideoEnd())
                WaitEndStage("update");
        }
    } 
}

