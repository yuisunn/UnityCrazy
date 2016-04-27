using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class CMD_CharList : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;


            List<NetDataPlayerBase> list = new List<NetDataPlayerBase>();
            package.DataReader.Msg2NetDataList<NetDataPlayerBase>(list);

            ActParam["list"] = list;

            DebugMod.Log("收到角色列表,数量=" + list.Count);
            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                List<NetDataPlayerBase> list = ActParam["list"] as List<NetDataPlayerBase>;
                if (null == list)
                    return false;

                MyPlayer myplayer = Local.Instance.GetMyPlayer;
                if (null != myplayer)
                {
                    myplayer.OnRcvCharList(list);
                    if (myplayer.IfRcvAllPlayerList())
                    {
                        DebugMod.Log("通知渲染层进入选择角色阶段");
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
