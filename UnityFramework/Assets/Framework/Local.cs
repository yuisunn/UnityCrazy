using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    class Local
    {
        private static Local _Instance;
        public static Local Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new Local();
                return _Instance;
            }
        }

        protected int mGameServerID;
        public int GameServerID
        {
            get{return mGameServerID;}
        }

        public ServerManager SvrMgr { get; set; }
        public GameModuleManager ModuleMgr { get; set; }
        public StageManager StageMgr  { get; set; }
        public SceneManager SceneMgr { get; set; }

        public Action<UnityActionDefine, ActionParam> ActionCallbackUnity { get; set; }

        public Func<string, string> funcReadFileCallBackUnity { get; set; }

        protected MyPlayer mMyPlayer = null;
        public MyPlayer GetMyPlayer { get { return mMyPlayer; } }

        public Local()
        {
            mMyPlayer = new MyPlayer(1);
        }
      
        public bool CallUnityAction(UnityActionDefine actionType, ActionParam param)
        {
            try
            {
                if (null == ActionCallbackUnity)
                    return false;

                ActionCallbackUnity(actionType, param);
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        public string CallUnityReadFileFunc (string fileName)
        {
            try
            {
                return funcReadFileCallBackUnity(fileName);
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return null;
            }
        }


        //public void RegisterCmd(string serverName, CsShare.SPSCmd ActionID, MethodActionProcess m)
        //{
        //    if (null == SvrMgr)
        //        return;
        //    SvrMgr.RegisterCmdRcver(serverName, ActionID, m);
        //}

        //public void RegisterNetEvent(string serverName, NetEventType netEventType, MethodNetEventRcver m)
        //{
        //    if (null == SvrMgr)
        //        return;
        //    SvrMgr.RegisterNetEventRcver(serverName, netEventType, m);
        //}

        public GameModuleBase GetModule(string moduleName)
        {
            return ModuleMgr.GetModule(moduleName);
        }
		
		 public void ShowUIMsg(string msg, MsgPos pos)
        {
            ActionParam param = new ActionParam();
            param["Msg"] = msg;
            param["Pos"] = (int)pos;
            Local.Instance.CallUnityAction(UnityActionDefine.ShowMsg, param);
        }
        public void CallActionFinish(LogicActionDefine actiontype, object arg)
        {
            ActionParam param = new ActionParam();
            param["Action"] = (int)actiontype;
            param["Param"] = arg;
            Local.Instance.CallUnityAction(UnityActionDefine.ActionFinish, param);
        }

        public void OnEnterGame(int serverId)
        {
            mGameServerID = serverId;
            DebugMod.Log("进入游戏成功");

            Local.Instance.StageMgr.BeginStage("gaming");
        }

        //internal object GetMyPlayer()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
