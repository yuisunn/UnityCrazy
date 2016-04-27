using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using System.Collections;
using System.Threading;

namespace SPSGame
{
    public class RoleData
    {

    }

    public class RoleDataSet
    {

    }

    public class CreateRoleParams
    {

    }

    public class DeleteRoleParams
    {

    }

    public class DeleteRoleResult
    {

    }

    public class EnterGameParams
    {

    }

    public class StageRoleManage : SPSStageBase
    {
        public RoleDataSet roleDataSet;
        public static NextStageEventHandler NextStep;

        public StageRoleManage()
            : base("selectrole")
        {

        }

        public override void AfterStageBegin()
        {
            ActionParam param = new ActionParam();
            param["GameStage"] = "selectrole";
            Local.Instance.CallUnityAction(UnityActionDefine.ChangeStage, param);
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
        //    //如果已经有这个账号的角色列表，不重新获取
        //    OnRoleDataSetReq();
        //}

        //public override void EndStage()
        //{
        //    base.EndStage();
        //    //不清角色数据
        //}

        ////请求角色列表
        //public void OnRoleDataSetReq()
        //{

        //}

        //public void OnRcvRoleDataSet(RoleDataSet data)
        //{
        //    //显示角色管理场景和UI
        //    //显示账号下的角色
        //}

        //public void OnCreateRoleReq(CreateRoleParams data)
        //{

        //}

        //public void OnCreateRoleAck(RoleData data)
        //{
        //    //显示新的角色
        //}

        //public void OnDeleteRoleReq(DeleteRoleParams data)
        //{
        //    //请求删除角色
        //}

        //public void OnDeleteRoleAck(DeleteRoleResult data)
        //{

        //}

        //public void OnEnterGameReq(EnterGameParams data)
        //{
        //    //请求进入游戏
        //    EndStage();
        //    StageManager.Instance.BeginStage("game");
        //}

        //public void OnReturnToSelectLine()
        //{
        //    EndStage();
        //    StageManager.Instance.BeginStage("selectline");
        //}

        //public void OnReturnToLogin()
        //{
        //    EndStage();
        //    StageManager.Instance.BeginStage("login");
        //}

    } 
}

