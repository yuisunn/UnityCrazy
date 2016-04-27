using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class MyPlayerMove
    {
        public void OnEngineStopMove(ActionParam actionParam)
        {
            if (Local.Instance.SceneMgr.MySceneID == 0)
                return;

            ParamStopMove dd = actionParam["StopPlayer"] as ParamStopMove;
            if (null == dd)
                return;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null == myplayer)
                return;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;

            if (scene.SceneType == Unity.ESceneType.Tower)
            {
                dd.CurrZ = (short)myplayer.Z;
            }

            DebugMod.Log("engine stop move");

            myplayer.SetPos(dd.CurrX, dd.CurrZ);
            myplayer.SetMoveDest(dd.CurrX, dd.CurrZ);

            MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
            if (null != fight)
            {
                fight.ResumeAutoFight();
            }

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;

            //LocatSprite();
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_StopMove, actionParam, null);
        }

        public void OnEngineMoveTo(ActionParam actionParam)
        {
            if (Local.Instance.SceneMgr.MySceneID == 0)
                return;

            ParamMoveTo dd = actionParam["ParamMoveTo"] as ParamMoveTo;
            if (null == dd)
                return;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null == myplayer)
                return;

            if (myplayer.SingLogic != null)
                myplayer.SingLogic.CancelSing();

            MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
            if (null != fight)
            {
                fight.CancelAutoFight();
            }
            //DebugMod.Log(string.Format("OnLogicMoveTo {0}:{1}|{2}:{3}", dd.CurrX, dd.CurrZ, dd.DestX, dd.DestZ));

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;

            DebugMod.Log("engine start move");

            if (scene.SceneType == Unity.ESceneType.Tower)
            {
                dd.CurrZ = (short)myplayer.Z;
                dd.DestZ = dd.CurrZ;
            }
            myplayer.SetPos(dd.CurrX, dd.CurrZ);
            myplayer.SetMoveDest(dd.DestX, dd.DestZ);

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;

            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_MoveTo, actionParam, null);

        }

        public void WriteMsgMoveTo(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            ParamMoveTo dd = actionParam["ParamMoveTo"] as ParamMoveTo;
            if (null == dd)
                return;

            string strPos1 = string.Format("{0},{1}", dd.CurrX, dd.CurrZ);
            string strPos2 = string.Format("{0},{1}", dd.DestX, dd.DestZ);
            writer.writeString(strPos1);
            writer.writeString(strPos2);
        }

        public void WriteMsgStopMove(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            ParamStopMove dd = actionParam["StopPlayer"] as ParamStopMove;
            if (null == dd)
                return;

            string strPos = string.Format("{0},{1}", dd.CurrX, dd.CurrZ);
            writer.writeString(strPos);
        }

        public void OnEngineChangeFloor(ActionParam actionParam)
        {
            if (Local.Instance.SceneMgr.MySceneID == 0)
                return;

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;

            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_ChangeFloor, actionParam, null);
        }

        public void WriteMsgChangeFloor(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            short x = (short)actionParam["DestX"];
            short y = (short)actionParam["DestY"];
            short z = (short)actionParam["DestZ"];
            writer.writeShort(x);
            writer.writeShort(y);
            writer.writeShort(z);
        }

        public void OnEngineChangeMap(ActionParam actionParam)
        {
            int sceneid = (int)actionParam["SceneID"];

            GameScene scene = Local.Instance.SceneMgr.GetGameScene(sceneid);
            if (null == scene)
            {
                DebugMod.LogError("无效的场景id:" + sceneid);
                return;
            }

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;

            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_ChangeMap, actionParam, null);
        }

        public void WriteMsgChangeMap(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            int sceneid = (int)actionParam["SceneID"];
            writer.writeInt32(sceneid);
        }

        public bool OnEngineLoadSceneFinish(ActionParam actionParam)
        {
            var tmp = actionParam["SceneID"];
            if (null == tmp)
                return false;
            int sceneid = (int)tmp;

            var tmp2 = actionParam["Result"];
            if (null == tmp2)
                return false;
            int result = (int)tmp2;

            if (result != 1)
                return false;

            return Local.Instance.SceneMgr.OnLoadSceneFinish(sceneid);
        }

        public void WriteMsgLoadSceneFinish(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            int seq = (int)actionParam["SceneSeq"];
            writer.writeInt32(seq);
        }

        public void OnEngineLeaveBattle()
        {
            if (Local.Instance.SceneMgr.MySceneID == 0)
                return;

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;

            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_LeaveBattle, null, null);
        }

        public void OnAckLeaveBattle()
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            myplayer.OnDisapear();

            ActionParam param = new ActionParam();
            param["ButtonEventType"] = (int)Unity.ButtonEventType.LeaveBattle;
            Local.Instance.CallUnityAction(UnityActionDefine.ButtonEvent, param);
        }

        public void OnLogicStopMove()
        {
            Local.Instance.GetMyPlayer.StopMove();

            ParamStopMove dd = new ParamStopMove();
            dd.CurrX = (short)Local.Instance.GetMyPlayer.X;
            dd.CurrZ = (short)Local.Instance.GetMyPlayer.Z;

            ActionParam actionParam = new ActionParam();
            actionParam["StopPlayer"] = dd;

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_StopMove, actionParam, null);
        }

        public void OnLogicMoveTo()
        {
            //DebugMod.Log("logic move to");
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            myplayer.MoveSprite();

            //DebugMod.Log(string.Format("NotifyMoveTo,{0}:{1}->{2}:{3}", (short)myplayer.X, (short)myplayer.Z, (short)myplayer.DirX, (short)myplayer.DirZ));

            ParamMoveTo dd = new ParamMoveTo();
            dd.CurrX = (short)myplayer.X;
            dd.CurrZ = (short)myplayer.Z;
            dd.DestX = (short)myplayer.DestX;
            dd.DestZ = (short)myplayer.DestZ;
            ActionParam actionParam = new ActionParam();
            actionParam["ParamMoveTo"] = dd;

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_MoveTo, actionParam, null);
        }
    }
}
