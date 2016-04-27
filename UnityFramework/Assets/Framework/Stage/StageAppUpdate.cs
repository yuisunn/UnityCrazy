using SPSGame;
using System.Collections;

namespace SPSGame
{
    public class StageAppUpdate : SPSStageBase
    {

        public StageAppUpdate()
            :base("update")
        {

        }

        ///// <summary>
        ///// 是否需要更新資源，对应的事件
        ///// </summary>
        //public static NextStageEventHandler NextStep;

        public override void AfterStageBegin()
        {
            ActionParam param = new ActionParam();
            param["GameStage"] = "update";
            Local.Instance.CallUnityAction(UnityActionDefine.ChangeStage, param);
            WaitEndStage("login");
        }

        public override void BeforeStageEnd()
        {

        }

        public override void OnUpdate()
        {

        }

        //public static IEnumerator CheckAppUpdate()
        //{
        //    if (null != NextStep)
        //    {
        //        NextStep(null, new NextStageEventArgs() { StageType = 0 });
        //    }

        //    yield break;
        //}
    }
    
}