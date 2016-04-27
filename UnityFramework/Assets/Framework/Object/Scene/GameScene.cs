using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.Unity;

namespace SPSGame
{
    public class GameScene
    {
        public FacilityManager facilityMng = new FacilityManager();
        public GroundItemManager groundItemMng = new GroundItemManager();
        public MonsterManager monsterMng = new MonsterManager();
        public NpcManager npcMng = new NpcManager();
        public PlayerManager playerMng = new PlayerManager();
        public HurtObjManager hurtManager = new HurtObjManager();
        public SfxObjManager sfxManager = new SfxObjManager();

        public int SceneID = 0;
        public int SeqId = 0;
        protected bool mLoaded = false;
        public bool Loaded { get { return mLoaded; } }

        protected ESceneType mSceneType = ESceneType.UnKnown;
        public ESceneType SceneType { get { return mSceneType; } }

        protected bool mEnable = false;
        public bool Enable { get { return mEnable; } }

        protected short mWidth = 0;
        protected short mHeight = 0;
        protected short mLayer = 0;
        public short Width { get { return mWidth; } }
        public short Height { get { return mHeight; } }
        public short Layer { get { return mLayer; } }
        public bool IsTower { get { return mSceneType == ESceneType.Tower; } }
        public GameScene(int sceneId, ESceneType type)
        {
            SceneID = sceneId;
            mEnable = false;
            mSceneType = type;
        }

        public void OnRcvSize(short width, short height, short layer)
        {
            mWidth = width;
            mHeight = height;
            mLayer = layer;
        }

        //调了OnEnter说明逻辑上这个场景已经生效了，只是不一定加载了资源
        //从这时起会update场景内所有对象

        public void OnEnter(int seq)
        {
            SeqId = seq;
            mEnable = true;

            //加载玩家当前所在地图(后台）
            if (!mLoaded)
            {
                LoadScene(SceneID);
            }
            else
            {
                OnLoadedAndEnable();
            }
        }

        public void OnLeave()
        {
            SeqId = 0;
            mEnable = false;
            mLoaded = false;
            playerMng.ClearAll();
            monsterMng.ClearAll();
            Local.Instance.GetMyPlayer.OnLeaveScene();
        }

        public void LoadScene(int sceneid)
        {
            DebugMod.Log(string.Format("LoadScene {0}", sceneid));
            ActionParam param = new ActionParam();
            param["SceneID"] = sceneid;
            param["ResID"] = (int)Local.Instance.GetMyPlayer.MyInfo.CharClass;
            DebugMod.Log("加载场景" + sceneid);
            Local.Instance.CallUnityAction(UnityActionDefine.LoadScene, param);
        }

        public void OnLoadSceneFinish()
        {
            mLoaded = true;

            if (mEnable)
            {
                OnLoadedAndEnable();
            }
        }

        public virtual void OnUpdate(long tickNow)
        {
            facilityMng.OnUpdate(tickNow);
            groundItemMng.OnUpdate(tickNow);
            monsterMng.OnUpdate(tickNow);
            npcMng.OnUpdate(tickNow);
            playerMng.OnUpdate(tickNow);
            hurtManager.OnUpdate(tickNow);
            sfxManager.OnUpdate(tickNow);
        }

        public void OnLoadedAndEnable()
        {
            DebugMod.Log(string.Format("OnLoadedAndEnable"));

            playerMng.EnablePlayers();
            monsterMng.EnableMonsters();

            Local.Instance.GetMyPlayer.LoadSprite();
        }

        public void MyPlayerHide()
        {

        }

        public void OnRcvNearbyMonsterID(List<NearbyMonsterID> listData)
        {
            monsterMng.OnRcvNearbyID(this, listData, this.mEnable && this.mLoaded);
        }

        public void RcvMonsterInfo(MonsterInfo info)
        {
            monsterMng.RcvMonsterInfo(info);
        }

        public void MonsterOut(int seqid)
        {
            monsterMng.MonsterOut(seqid);
        }

        public void OnRcvNearbyPlayerID(List<NearbyPlayerID> listData)
        {
            playerMng.OnRcvNearbyID(this, listData, this.mEnable && this.mLoaded);
        }

        public void RcvPlayerInfo(OtherPlayerInfo info)
        {
            playerMng.RcvPlayerInfo(info);
        }

        public void PlayerOut(long charid)
        {
            playerMng.PlayerOut(charid);
        }
               
        public LiveSprite GetLiveSprite(long id, bool isplayer)
        {
            if (isplayer)
            {
                if (id == Local.Instance.GetMyPlayer.CharID)
                    return Local.Instance.GetMyPlayer;
                else
                    return playerMng.GetPlayer(id);
            }
            else
                return monsterMng.GetMonster((int)id);
        }

    }
}
