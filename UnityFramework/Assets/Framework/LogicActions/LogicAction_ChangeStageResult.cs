using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_ChangeStageResult : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var stageNameTemp = ActParam["GameStage"];
                if (null == stageNameTemp)
                    return false;

                int result = ActParam.Get<int>("Result");

                string stageName = stageNameTemp.ToString().ToLower();
                switch (stageName)
                {
                    case "startup":
                        break;
                    case "update":
                        break;
                    case "login":
                        break;
                    case "selectrole":
                        OnSelectRoleResult(result);
                        break;
                    case "gaming":
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        protected void OnSelectRoleResult(int result)
        {
            if (result == 0)
                return;

            //ActionParam param2 = new ActionParam();
            //param2["CharID"] = 222;
            //param2["FirstName"] = "a01";
            //param2["LastName"] = "小微微";
            //param2["CharClass"] = 1;
            //param2["VipLevel"] = 3;
            //param2["CharLevel"] = 4;
            //param2["Map"] = 111;
            //param2["Cloth"] = 222;
            //Local.Instance.CallUnityAction(UnityActionDefine.SendChar, param2);

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null != myplayer)
            {
                    //通知渲染层进入选择角色阶段
                Dictionary<long, NetDataPlayerBase> playerList = myplayer.getPlayerList();
                foreach (var pair in playerList)
                {
                    NetDataPlayerBase playerbase = pair.Value;
                    ActionParam param = new ActionParam();
                    param["CharID"] = playerbase.CharID;
                    param["FirstName"] = playerbase.FirstName;
                    param["LastName"] = playerbase.LastName;
                    param["CharClass"] = playerbase.CharClass;
                    param["CharGrade"] = 1;
                    param["VipLevel"] = playerbase.VipLevel;
                    param["CharLevel"] = playerbase.Level;
                    param["Map"] = playerbase.Map;
                    param["Cloth"] = playerbase.Cloth;
                    Local.Instance.CallUnityAction(UnityActionDefine.SendChar, param);
                }
            }
        }
    }
}
