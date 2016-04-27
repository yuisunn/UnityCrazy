using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.CsShare;
using SPSGame.CsShare.Data;
using SPSGame.Tools;
using SPSGame.Unity;

namespace SPSGame
{
    public class SceneManager
    {
        protected int m_MySceneID = 0;
        protected int m_MySceneSeq = 0;
        protected int m_ChangeSceneSeq;     //正在切换到的场景
        protected int m_ChangeSceneID;      //正在切换到的场景

        public int MySceneID { get { return m_MySceneID; } }
        public int MySceneSeq { get { return m_MySceneSeq; } }
        public int ChangeSceneSeq { get { return m_ChangeSceneSeq; } }
        public int ChangeSceneID { get { return m_ChangeSceneID; } }

        protected Dictionary<int, GameScene> mObjDic = new Dictionary<int, GameScene>();    //<sceneid,>

        public void InitAllGameScene()
        {
            //从静态数据和动态数据里读取场景信息，创建所有场景对象，但不加载资源
            string configinfo = Local.Instance.CallUnityReadFileFunc("Scene");
            DataUnit SceneData = new DataUnit(configinfo);
            List<int> allId = SceneData.GetAllID();
            SPSGame.Unity.ESceneType type = SPSGame.Unity.ESceneType.UnKnown;
            foreach(var sceneid in allId)
            {
                string typestr = SceneData.GetDataById(sceneid, "类型");
                int typeint = -1;
                if (int.TryParse(typestr, out typeint))
                {
                    type = (ESceneType)typeint;
                }
                else
                    type = ESceneType.UnKnown;

                GameScene scene = new GameScene(sceneid, type);
                mObjDic[sceneid] = scene;
            }
        }
            
        public void OnReset()
        {
            m_ChangeSceneID = 0;
            m_ChangeSceneSeq = 0;

            GameScene scene = GetMyScene();
            if (null != scene)
            {
                scene.OnLeave();
            }

            m_MySceneID = 0;
            m_MySceneSeq = 0;
            //Local.Instance.GetMyPlayer.MyInfo.SceneSeq = 0;
        }

        public GameScene GetGameScene(int sceneid)
        {
            GameScene scene = null;
            mObjDic.TryGetValue(sceneid, out scene);
            return scene;
        }

        public GameScene GetMyScene()
        {
            GameScene scene = null;
            mObjDic.TryGetValue(m_MySceneID, out scene);
            return scene;
        }

        public void OnUpdate()
        {
            long tickNow = DateTime.Now.Ticks / 10000;
            Local.Instance.GetMyPlayer.OnUpdate(tickNow);
            foreach (var obj in mObjDic)
            {
                if (obj.Value.Enable)
                {
                    obj.Value.OnUpdate(tickNow);
                }
            }
        }

        public void OnRcvPlayerInfo(int sceneSeq, List<OtherPlayerInfo> listData)
        {
            GameScene myscene = GetMyScene();
            if (null == myscene)
                return;

            foreach (var player in listData)
            {
                if (sceneSeq != m_MySceneSeq)
                    continue;
                myscene.RcvPlayerInfo(player);
            }
        }

        public void OnRcvMonsterInfo(int sceneSeq, List<MonsterInfo> listData)
        {
            GameScene myscene = GetMyScene();
            if (null == myscene)
                return;

            foreach(var monster in listData)
            {
                if (sceneSeq != m_MySceneSeq)
                    continue;
                myscene.RcvMonsterInfo(monster);
            }
        }

        public void OnPlayerOut(long charid, int sceneSeq)
        {
            if (sceneSeq != m_MySceneSeq)
                return;
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (myplayer.CharID == charid)  //不处理自己
                return;

            GameScene scene = GetMyScene();
            if (null != scene)
            {
                scene.PlayerOut(charid);
            }
        }

        public void OnMonsterOut(int seq, int sceneSeq)
        {
            if (sceneSeq != m_MySceneSeq)
                return;
            GameScene scene = GetMyScene();
            if (null != scene)
            {
                scene.MonsterOut(seq);
            }
        }


        public void OnRcvNearbyPlayerID(int sceneSeq, List<NearbyPlayerID> listData)
        {
            if (sceneSeq != m_MySceneSeq)
                return;
            GameScene myscene = GetMyScene();
            if (null == myscene)
                return;
            myscene.OnRcvNearbyPlayerID(listData);
        }

        public void OnRcvNearbyMonsterID(int sceneSeq, List<NearbyMonsterID> listData)
        {
            if (sceneSeq != m_MySceneSeq)
                return;
            GameScene myscene = GetMyScene();
            if (null == myscene)
                return;
            myscene.OnRcvNearbyMonsterID(listData);
        }

        public void ForceSetPlayerPos(long charid, short x, short y, short z, bool visible)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (charid == myplayer.CharID)
            {
                myplayer.ForceSetPos(x, y, z, visible);
            }
            else
            {
                GameScene scene = GetMyScene();
                if (null != scene)
                {
                    PlayerObj player = scene.playerMng.GetPlayer(charid);
                    if (null != player)
                    {
                        player.ForceSetPos(x, y, z, visible);
                    }
                }
            }
        }

        public void ForceSetMonsterPos(int seq, short x, short z)
        {
            GameScene scene = GetMyScene();
            if (null == scene)
                return;
            MonsterObj monster = scene.monsterMng.GetMonster(seq);
            if (null != monster)
                monster.ForceSetPos(x, (short)monster.Y, z, true);
        } 

        public bool OnLoadSceneFinish(int sceneId)
        {
            DebugMod.Log(string.Format("OnLoadSceneFinish {0}", sceneId));

            GameScene scene = Local.Instance.SceneMgr.GetGameScene(sceneId);
            if (null == scene)
                return false;

            scene.OnLoadSceneFinish();

            if (scene.Enable)   //已经在逻辑上进入了场景
            {
                DebugMod.Log(string.Format("scene.Enable"));
                return true;
            }

            if (sceneId != Local.Instance.SceneMgr.ChangeSceneID)
            {
                DebugMod.Log(string.Format("sceneId != Local.Instance.SceneMgr.ChangeSceneID"));
                return false;
            }

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return false;

            ActionParam param = new ActionParam();
            param["SceneSeq"] = Local.Instance.SceneMgr.ChangeSceneSeq;
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_LoadSceneFinish, param, null);

            DebugMod.Log(string.Format("CMD_CS_LoadSceneFinish, {0}", Local.Instance.SceneMgr.ChangeSceneSeq));
            return true;
        }

        public bool OnRcvChangeSceneBegin(int sceneId, int sceneSeq)
        {
            DebugMod.Log(string.Format("OnRcvChangeSceneBegin, {0}:{1}", sceneId, sceneSeq));

            if (m_ChangeSceneID > 0)    //已经在切换场景
            {
                DebugMod.LogError("已经在切换场景");
            }

            GameScene scene = GetGameScene(sceneId);
            if (null == scene)
                return false;

            m_ChangeSceneID = sceneId;
            m_ChangeSceneSeq = sceneSeq;

            //离开原来场景
            GameScene oldscene = GetMyScene();
            if (null != oldscene)
            {
                oldscene.OnLeave();
            }
            m_MySceneID = 0;
            m_MySceneSeq = 0;
            //Local.Instance.GetMyPlayer.MyInfo.SceneSeq = 0;

            //加载新的场景
            scene.LoadScene(sceneId);
            return true;
        }

        public void OnRcvSceneSize(int sceneId, short width, short height, short layer)
        {
            GameScene scene = GetGameScene(sceneId);
            if (null == scene)
                return;
            scene.OnRcvSize(width, height, layer);
        }

        public void OnRcvChangeSceneEnd(int sceneId, int sceneSeq)
        {
            DebugMod.Log(string.Format("OnRcvChangeSceneEnd, {0}:{1}", sceneId, sceneSeq));

            GameScene scene = GetGameScene(sceneId);
            if (null == scene)
                return;

            Local.Instance.GetMyPlayer.SetSceneSeq(scene);

            m_MySceneID = sceneId;
            m_MySceneSeq = sceneSeq;
            m_ChangeSceneID = 0;
            m_ChangeSceneSeq = 0;
            //Local.Instance.GetMyPlayer.MyInfo.SceneSeq = sceneSeq;

            //m_ChangeSceneID > 0说明之前已经通过OnRcvChangeSceneBegin加载了场景
            //刚进入游戏时则不会调用OnRcvChangeSceneBegin
            scene.OnEnter(sceneSeq);
        }
    }
}
