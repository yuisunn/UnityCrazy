using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using SPSGame.Tools;

namespace SPSGame
{
    public class GameDynamicData
    {

    }

    public class RoleDetail
    {

    }

    public class RoleDetailList
    {

    }

    public class StageGame : SPSStageBase
    {
        public RoleDataSet roleDataSet;
        public static NextStageEventHandler NextStep;

        public StageGame()
            :base("gaming")
        {
            
        }
        public override void AfterStageBegin()
        {
            ActionParam param = new ActionParam();
            param["GameStage"] = "gaming";
            DebugMod.Log("进入游戏");
            Local.Instance.CallUnityAction(UnityActionDefine.ChangeStage, param);

            ////请求游戏动态数据
            //OnGameDynamicDataReq();
            ////获取角色详细信息（1至n个角色）
            //OnRoleDetailReq();
        }

        public override void BeforeStageEnd()
        {

        }

        public override void OnUpdate()
        {

        }

        //public void OnGameDynamicDataReq()
        //{

        //}

        //public void OnGameDynamicDataAck(GameDynamicData data)
        //{
        //    //保存地图数据
        //}

        //public void OnRoleDetailReq()
        //{

        //}

        //public void OnRoleDetailAck(RoleDetailList data)
        //{
        //    string currMap = "";
        //    //保存角色详细数据
        //    //进入场景
        //    SceneManager.Instance.OnEnterGameMap(currMap);
        //}

        public void OnReturnToRoleManage()
        {
            WaitEndStage("rolemanage");
        }

        public void OnReturnToSelectLine()
        {
            WaitEndStage("selectline");
        }

        public void OnReturnToLogin()
        {
            WaitEndStage("login");
        }
    }
}
