using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using System.Collections;
using System.Threading;

namespace SPSGame
{
    public class LineDataSet
    {
        public int result = 0;
        public string reason = "";
    }

    public class EnterLineAck
    {
        public int order = 0;
        public int result = 0;
        public string reason = "";
    }

    public class StageSelectLine : SPSStageBase
    {
        public LineDataSet lineDataSet;
        public static NextStageEventHandler NextStep;

        public StageSelectLine()
            :base("selectline")
        {

        }

        public override void AfterStageBegin()
        {
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
        //    //如果线路数据是不久前获得的，不需要再次获取
        //    OnLineDataReq();
        //}

        //public override void EndStage()
        //{
        //    base.EndStage();
        //}

        //void OnLineDataReq()
        //{
        //    //请求各线路的数据
        //}

        //void OnLineDataAck(LineDataSet data)
        //{
        //    //显示各线路数据
        //    //ShowLineInfo(data);
        //}

        ////order = 线路编号
        //void OnEnterLineReq(int order)
        //{
        //    //能否进入
        //    //连接服务器，需要ip地址，端口

        //    //发送请求进入线路的消息
        //    //ReqEnterLine
        //}

        //void OnEnterLineAck(EnterLineAck data)
        //{
        //    //进入成功则清除当前阶段，准备进入下一阶段，
        //    //如失败则显示原因在界面上
        //    EndStage();
        //    StageManager.Instance.BeginStage("rolemanage");
        //}

        //public void OnReturnToLogin()
        //{
        //    EndStage();
        //    StageManager.Instance.BeginStage("login");
        //}

    } 
}

