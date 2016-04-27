using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SPSGame.Tools;
using SPSGame;

namespace SPSGame.Unity
{

    public class HeroInfo
    {
        public int level;
        public int exp;

        public int strength;
        public int agility;
        public int wit;
        public int strengthUp;
        public int agilityUp;
        public int witUp;

        public int maxHealth;
        public int physicPower;
        public int magicPower;
        public int physicDefance;
        public int magicDefance;
        public int physicCrit;
        public int CritPower;

    }

    public class PlayerInfo
    {
        public int money;
        public int level;
    }

    public class GameStage : StageBase
    {
        GPlayer mHero = null;

        //Dictionary<int, ItemInfo> mItemDic = new Dictionary<int, ItemInfo>();

        ESceneType mSceneType = ESceneType.UnKnown;

        Dictionary<int, Transform> mLayerDic = new Dictionary<int, Transform>();
        
        public override void StartStage()
        {
            base.StartStage();
            
            EventManager.Register<EventLoadScene>(OnEventLoadScene);
            EventManager.Register<EventLoadSprite>(OnEventLoadSprite);
            EventManager.Register<EventMoveSprite>(OnEventMoveSprite);
            EventManager.Register<EventShowSprite>(OnEventShowSprite);
            EventManager.Register<EventLocatSprite>(OnEventLocatSprite);
            EventManager.Register<EventActSprite>(OnEventActSprite);
            EventManager.Register<EventHurtSprite>(OnEventHurtSprite);
            EventManager.Register<EventKillSprite>(OnEventKillSprite);
            EventManager.Register<EventButtonEvent>(OnEventButtonEvent);
            EventManager.Register<EventSetSpriteInfo>(OnEventSetSpriteInfo);

            EventManager.Register<EventPlayerControlAble>(OnEventPlayerControl);
            EventManager.Register<EventPlayerControlSpeed>(OnEventPlayerControl);

            EventManager.Register<EventLoadCreatureBullet>(OnEventLoadCreature);
            EventManager.Register<EventLoadCreatureHang>(OnEventLoadCreature);
            EventManager.Register<EventKillCreature>(OnEventKillCreature);

            EventManager.Register<EventShowHud>(OnEventShowSpriteHud);

            UIManager.Instance.ShowLoading();

            Logicer.ChangeStageResult(1, "gaming");
        }

        void OnEventLoadCreature( System.EventArgs e )
        {
            if (e is EventLoadCreatureBullet)
            {
                EventLoadCreatureBullet EB = (EventLoadCreatureBullet)e;
                GCreatureBullet bullet = ObjectManager.Instance.LoadCreature(ECreatureType.Bullet, EB.creatureID) as GCreatureBullet;
                bullet.creatureID = EB.creatureID;
                bullet.resID = EB.resID;
                GSprite sc = ObjectManager.Instance.GetSprite(EB.spriteID);
                GSprite tar = ObjectManager.Instance.GetSprite(EB.targetSpriteID);
                bullet.sprite = sc;
                bullet.target = tar;
                bullet.moveSpeed = EB.moveSpeed;
                bullet.maxlife = EB.lifeMax;
                bullet.Init();
                bullet.isShow = true;
            }
            else if (e is EventLoadCreatureCross)
            {
                EventLoadCreatureCross EB = (EventLoadCreatureCross)e;
                GCreatureCross bullet = ObjectManager.Instance.LoadCreature(ECreatureType.Cross, EB.creatureID) as GCreatureCross;
                bullet.creatureID = EB.creatureID;
                bullet.resID = EB.resID;
                bullet.maxlife = EB.lifeMax;
                bullet.direction = EB.direction;
                GSprite sc = ObjectManager.Instance.GetSprite(EB.spriteID);
                bullet.sprite = sc;
                bullet.hitOnce = EB.hitOnce;

            }
            else if (e is EventLoadCreatureHang)
            {
                EventLoadCreatureHang EB = (EventLoadCreatureHang)e;
                GCreatureHang hang = ObjectManager.Instance.LoadCreature(ECreatureType.Hang, EB.creatureID) as GCreatureHang;           
                GSprite sc = ObjectManager.Instance.GetSprite(EB.spriteID);
                hang.creatureID = EB.creatureID;
                hang.resID = EB.resID;
                hang.sprite = sc;
                hang.maxlife = EB.life;
                hang.Init();
                hang.isShow = true;
            }
            else if (e is EventLoadCreaturePoint)
            {
                EventLoadCreaturePoint EB = (EventLoadCreaturePoint)e;
                GCreaturePoint point = ObjectManager.Instance.LoadCreature(ECreatureType.Point, EB.creatureID) as GCreaturePoint;
                point.creatureID = EB.creatureID;
                point.resID = EB.resID;
                point.maxlife = EB.life;
                point.Init();
                point.isShow = true;
            }
        }

        void OnEventKillCreature( EventKillCreature e )
        {
            ObjectManager.Instance.KillObject(e.creatureID);
        }

        void OnEventPlayerControl(System.EventArgs e)
        {
            if( mHero == null )
            {
                DebugMod.LogError("Player is not create yet!");
            }

            if( e is EventPlayerControlAble )
            {
                mHero.SetControlAble(((EventPlayerControlAble)e).controlAble);
            }
            else if( e is EventPlayerControlSpeed )
            {
                mHero.SetControlSpeed(((EventPlayerControlSpeed)e).controlSpeed);
            }
        }

        void OnEventButtonEvent( EventButtonEvent e )
        {
            switch( e.buttonEvent )
            {
                case ButtonEventType.LeaveBattle:
                    UIManager.Instance.HideWindow<UIWndGaming>();
                    UIWndTowerScene ts = UIManager.Instance.ShowWindow<UIWndTowerScene>();
                    ts.Resume();
                    break;
                default:
                    break;
            }
        }

        #region Scene Control
        void OnEventLoadScene( EventLoadScene e )
        {
            UIManager.Instance.ClearAllWindow();
            string sceneres = DataManager.Instance.GetSceneRes(e.sceneID);
            ESceneType type = DataManager.Instance.GetSceneType(e.sceneID);

            UILoading ul = UIManager.Instance.ShowLoading();
            SceneManager.Instance.LoadGameScene(sceneres, ul, () =>
                {
                    mSceneType = type;

                    InitCamera();
                    InitSceneUI(e.heroResID);
                    InitScene3DData();

                    UIManager.Instance.DestroyLoading();
                    Logicer.ChangeSceneResult(1, e.sceneID);
                });
        }

        void InitCamera()
        {
            switch (mSceneType)
            {
                case ESceneType.MainCity:
                    CameraManager.Instance.Set3DCamera(new Vector3(0, 20, -40f), Quaternion.Euler(30, 0, 0), 30);
                    CameraManager.Instance.RemoveSwiper();
                    mLayerDic.Clear();
                    break;
                case ESceneType.Tower:
                    CameraManager.Instance.Set3DCamera(new Vector3(0, -8.31f, -39.2f), Quaternion.Euler(3, 0, 0), 30);
                    CameraManager.Instance.InitSwiper(-10f, 26, -100, 100);
                    CameraManager.Instance.SetTarget(null);
                    break;
                case ESceneType.Boss:
                    CameraManager.Instance.Set3DCamera(new Vector3(0, 16, -25f), Quaternion.Euler(30, 0, 0), 30);
                    CameraManager.Instance.RemoveSwiper();
                    mLayerDic.Clear();
                    break;
            }
        }
        
        void InitScene3DData()
        {
            switch (mSceneType)
            {
                case ESceneType.Tower:
                    mLayerDic.Clear();
                    GameObject scene = U3DMod.Find("Scene");
                    Transform t = scene.transform;
                    if (scene != null)
                    {
                        for (int i = 0; i < t.childCount; ++i)
                        {
                            int idx = t.GetChild(i).gameObject.name.IndexOf('_');
                            if (idx >= 0)
                            {
                                string layerstr = t.GetChild(i).gameObject.name.Substring(idx + 1);
                                int layer = 0;
                                if (!string.IsNullOrEmpty(layerstr) && int.TryParse(layerstr, out layer))
                                    mLayerDic[layer] = t.GetChild(i);
                            }
                        }
                    }
                    DebugMod.Log("Find layer cout = " + mLayerDic.Count);
                    break;
                default:
                    break;
            }
        }

        #endregion Scene Control


        #region Sprite Control
        void OnEventLoadSprite(EventLoadSprite e)
        {
            GSprite spr = ObjectManager.Instance.LoadSprite(e.spriteid, e.resid, (ESpriteType)e.type, e.postion, e.direction);
            if ((ESpriteType)e.type == ESpriteType.Player)
            {
                mHero = spr as GPlayer;
                if (mSceneType == ESceneType.MainCity)
                    CameraManager.Instance.SetTarget(mHero);
            }
            spr.moveSpeed = 3.5f;
            spr.CalculatePositionHandler = CalculatePostion;
            spr.Show( e.isShow );
        }

        void OnEventShowSprite( EventShowSprite e )
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if( spr == null )
            {
                DebugMod.LogError("can't find sprite:" + e.spriteID + " when show sprite");
                return;
            }
            spr.Show(e.isShow);
        }

        void OnEventMoveSprite( EventMoveSprite e )
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogError("can't find sprite:" + e.spriteID + " when move sprite");
                return;
            }
            spr.destPosition = e.position;
            spr.destDirection = e.direction;
            spr.moveSpeed = e.speed;
            spr.direction = e.direction;
            //Debug.Log("onMove  " + e.position);
        }

        void OnEventSetSpriteInfo( EventSetSpriteInfo e )
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogError("can't find sprite:" + e.spriteID + " when Set sprite Info");
                return;
            }

            spr.SetSpriteInfo( new SpriteInfo(e.spriteName, e.spriteLevel, e.maxHealth, e.currentHealth));
        }

        void OnEventLocatSprite(EventLocatSprite e)
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogError("can't find sprite:" + e.spriteID + " when locat sprite");
                return;
            }
            spr.position = e.position;
            if (e.direction != Vector3.zero)
                spr.direction = e.direction;
            spr.destPosition = e.position;
            if (e.direction != Vector3.zero)
                spr.destDirection = e.direction;
            spr.SetUp3DRes();
        }

        void OnEventActSprite( EventActSprite e )
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogError("can't find sprite:" + e.spriteID + " when act sprite");
                return;
            }
            spr.ChangeAction((EAnimType)e.actionID,e.speed);
        }

        void OnEventHurtSprite( EventHurtSprite e )
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogWarning("can't find sprite:" + e.spriteID + " when hurt sprite");
                return;
            }
            spr.OnHurt(e.hurtType, e.hurtNumber);
        }

        void OnEventKillSprite( EventKillSprite e)
        {
            if (e.spriteID == -1)
                ObjectManager.Instance.ClearAllSprite();
            else
                ObjectManager.Instance.KillObject(e.spriteID);
        }

        void OnEventShowSpriteHud(EventShowHud e)
        {
            GSprite spr = ObjectManager.Instance.GetSprite(e.spriteID);
            if (spr == null)
            {
                DebugMod.LogWarning("can't find sprite:" + e.spriteID + " when show sprite hud");
                return;
            }
            spr.ShowInfoUI(e.isShow);
        }
        #endregion Sprite Control

        #region UI Control
        void InitSceneUI( int heroresid )
        {
            UIWndGaming gameui = UIManager.Instance.ShowWindow<UIWndGaming>();

            gameui.OnOpenSelectLineHandler = () =>
            {
                UIManager.Instance.ShowWindow<UIWndSelectLine>();
            };

            gameui.OnOpenCharDetailHandler = () =>
            {
                UIWndCharDetial detailui = UIManager.Instance.ShowWindow<UIWndCharDetial>();

                detailui.SetResInfo(heroresid);
                //detailui.SetRoleBaseProperty(10, 10, 15, 2f, 3f, 4.6f);//temp
                //detailui.SetRoleDetailProperty(100, 20, 30, 50, 40, 26, 35);//temp

                detailui.CloseWndHandler = () => detailui.Show(false);

                detailui.OpenEquipHandler = () =>
                {
                    UIWndEquipmentPlay ep = UIManager.Instance.ShowWindow<UIWndEquipmentPlay>();
                    //detailui.Hide();
                    ep.CloseWndHandler = () =>
                    {
                        //detailui.Show();
                        ep.Show(false);
                    };
                };

                detailui.OpenFashionHandler = () =>
                {
                    UIWndFashion fs = UIManager.Instance.ShowWindow<UIWndFashion>();
                    //detailui.Hide();
                    fs.CloseWndHandler = () =>
                    {
                        //detailui.Show();
                        fs.Show(false);
                    };
                };

                detailui.OpenFlagHandler = () =>
                {
                    UIWndFlag fg = UIManager.Instance.ShowWindow<UIWndFlag>();
                    //detailui.Hide();
                    fg.CloseWndHandler = () =>
                    {
                        //detailui.Show();
                        fg.Show(false);
                    };
                };

            };


            UIJoyStick stick = UIManager.Instance.GetWindow<UIJoyStick>();
            if (stick == null)
            {
                stick = UIManager.Instance.ShowWindow<UIJoyStick>();
                stick.On_JoystickMove += MovePlayer;
                stick.On_JoystickMoveEnd += MovePlayerEnd;
            }

            gameui.Init(mSceneType);

            if (mSceneType == ESceneType.Tower)
            {
                stick.SetType(UIJoyStick.EStickType.Corss);

                UIWndTowerScene towerui = UIManager.Instance.ShowWindow<UIWndTowerScene>();

                towerui.OnPressUpImageHandler = () =>
                {
                    Ray ray = CameraManager.Instance.camera3D.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray);
                    if (hits.Length > 0)
                    {
                        Transform layerTrs = null;
                        Vector3 pos = Vector3.zero;
                        for (int i = hits.Length - 1; i >= 0; --i)
                        {
                            if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Layer"))
                            {
                                layerTrs = hits[i].transform;
                                pos = hits[i].point;
                            }                             
                        }

                        if (layerTrs != null)
                        {
                            foreach (int layer in mLayerDic.Keys)
                            {
                                if (mLayerDic[layer] == layerTrs)
                                {
                                    Logicer.ChangeFloorTo(mHero.spriteID, pos.x, layer, 0);
                                    gameui.Show(true);
                                    towerui.Hide();
                                }
                            }

                            towerui.Resume();
                        }
                    }
                };

                towerui.Init(heroresid);

                gameui.MoveHandler = MovePlayerLeftRight;
                gameui.Show(false);

                gameui.LeaveBattleHandler = () =>
                {
                    Logicer.LeaveBattle();
                };
            }
            else
            {
                stick.SetType(UIJoyStick.EStickType.Stick);
            }


        }

        #endregion UI Control

//         void LoadDragModel(int modelresid, UnityAction<Object> cb)
//         {
//             string filename = DataManager.Instance.GetModelRes(modelresid);
//             string sourcename = PathMod.GetPureName(filename);
// 
//             AssetBundleManager.Instance.LoadMonsterByLoader(filename, sourcename, (go) =>
//             {
//                 GameObject obj = U3DMod.Clone(go as GameObject);
//                 obj.transform.rotation = Quaternion.Euler(0, -180, 0);
//                 cb(obj);
//             });
        //         }


        Vector3 CalculatePostion(Vector3 innerposition)
        {
            Vector3 showpostion = Vector3.zero;
            switch (mSceneType)
            {
                case ESceneType.MainCity:
                    showpostion = innerposition;

//                     Ray ray = new Ray(innerposition+new Vector3(0,100,0), new Vector3(0, -1, 0));
// 
//                     RaycastHit hit;
//                     if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Layer")))     
//                     {      
//                         showpostion.y = hit.point.y;                                        
//                     }
//                        
                    break;
                case ESceneType.Boss:
                    showpostion = innerposition;
                    break;
                case ESceneType.Tower:
                    showpostion.x = innerposition.x;
                    int layer = (int)(innerposition.y);
                    int channel = (int)(innerposition.z);
                    if (!mLayerDic.ContainsKey(layer))
                    {
                        DebugMod.LogError("can't find layer index: " + layer);
                        return innerposition;
                    }
                    else
                    {
                        if (mLayerDic[layer] == null)
                        {
                            DebugMod.LogError("can't find layer " + layer);
                        }

                        showpostion.y = mLayerDic[layer].position.y;

                    }
                    if (channel == 0)
                        showpostion.z = mLayerDic[layer].position.z - 1.4f;
                    else if (channel == 1)
                        showpostion.z = mLayerDic[layer].position.z;
                    else if (channel == 2)
                        showpostion.z = mLayerDic[layer].position.z + 1.4f;

                    break;
            }
            return showpostion;
        }

        #region Player Control
        void MovePlayerLeftRight( bool isleft,bool state )
        {
            if( mSceneType == ESceneType.Tower )
            {
                if (state)
                    mHero.ControlMove(isleft ? new Vector2(-1, 0) : new Vector2(1, 0), true);
                else
                    mHero.ControlMove(Vector2.zero, mSceneType == ESceneType.Tower, true);
            }
        }

        void MovePlayer( MovingStick move )
        {
     
            if(mHero!=null)
            {

                if (mSceneType == ESceneType.MainCity || mSceneType== ESceneType.Boss)
                {
                    mHero.ControlMove(move.stickAxis,false);
                    
                }
                else if( mSceneType == ESceneType.Tower )
                {
                    if( Mathf.Abs( move.stickAxis.x)>=0.9f )
                    {
                        mHero.ControlMove( new Vector2(move.stickAxis.x,0),true);
                    }
                    else
                    {
                        mHero.ControlMove(new Vector2(0, 0), true);
                    }
                }
            }
        }

        void MovePlayerEnd(MovingStick move)
        {
            if (mHero != null)
            {
                mHero.ControlMove(Vector2.zero, mSceneType== ESceneType.Tower, true);
            }
        }
        #endregion Player Control

        public override void EndStage()
        {
            base.EndStage();
            EventManager.Remove<EventLoadScene>(OnEventLoadScene);
            EventManager.Remove<EventLoadSprite>(OnEventLoadSprite);
            EventManager.Remove<EventMoveSprite>(OnEventMoveSprite);
            EventManager.Remove<EventShowSprite>(OnEventShowSprite);
            EventManager.Remove<EventLocatSprite>(OnEventLocatSprite);
            EventManager.Remove<EventActSprite>(OnEventActSprite);
            EventManager.Remove<EventHurtSprite>(OnEventHurtSprite);
            EventManager.Remove<EventKillSprite>(OnEventKillSprite);
            EventManager.Remove<EventButtonEvent>(OnEventButtonEvent);
            EventManager.Remove<EventSetSpriteInfo>(OnEventSetSpriteInfo);

            EventManager.Remove<EventPlayerControlAble>(OnEventPlayerControl);
            EventManager.Remove<EventPlayerControlSpeed>(OnEventPlayerControl);

            EventManager.Remove<EventLoadCreatureBullet>(OnEventLoadCreature);
            EventManager.Remove<EventLoadCreatureHang>(OnEventLoadCreature);

            EventManager.Remove<EventKillCreature>(OnEventKillCreature);
            EventManager.Remove<EventShowHud>(OnEventShowSpriteHud);

            ObjectManager.Instance.ClearAllSprite();
            UIManager.Instance.ClearAllWindow();
            UIManager.Instance.ClearAllHud();
            CameraManager.Instance.RemoveSwiper();
            CameraManager.Instance.SetTarget(null);
        }
    }
}