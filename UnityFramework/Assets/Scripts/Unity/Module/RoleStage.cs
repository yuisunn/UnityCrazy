using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class RoleStage : StageBase
    {
        public delegate void DataViewDelegate(RoleData data);

        public class RoleData
        {
            public int roleID = -1;
            public string roleName = null;

            public string resName;
            public string desc;
            public int[] data;
            public string[] gradeName;
            public string[] roleSpriteName;
            public int hardLevel = 1;
            public Vector3 postion = Vector3.zero;
            public Quaternion rotation = Quaternion.identity;
            public float roleHeight;

            public bool isShow = false;
            public GameObject role3DRes = null;
            public VoidDelegate Load3DRes = null;
            public DataViewDelegate OnFocus = null;

            public bool haveChar = false;

            public string GetCurrentGradeName( int grade )
            {
                if( grade< gradeName.Length )
                {
                    return gradeName[grade];
                }
                else
                {
                    DebugMod.Log("can't find gradename of grade: " + grade);
                    return "";
                }
            }

            public void Show(VoidDelegate cb,bool _show)
            {
                isShow = _show;

                if (role3DRes != null)
                    U3DMod.SetActive(role3DRes, isShow);
                else
                {
                    if (isShow)
                    {
                        Load3D(cb);
                    }
                }
            }

            void Load3D( VoidDelegate cb )
            {
                string sourceName = PathMod.GetPureName(resName);
                AssetBundleManager.Instance.LoadMonsterByLoader(resName, sourceName, (go) =>
                {
                    role3DRes = U3DMod.Clone(go as GameObject);
                    role3DRes.transform.position = postion;
                    role3DRes.transform.rotation = rotation;

                    CPreviewModel cpm = role3DRes.AddComponent<CPreviewModel>();
                    cpm.OnClickHandler = OnFocusMe;
                    cpm.InitAllEffect(roleName);
                    cb();
                });
            }

            void OnFocusMe()
            {
                OnFocus(this);
            }
        }

        public class CharInfo
        {
            public long charID;
            public string firstName;
            public string lastName;
            public int charClass;
            public int charGrade;
            public int vipLevel;
            public int charLevel;
            public int mapID;
            //public UIHudCharInfo charInfoHud = null;
        }

        bool mInited = false;

        Dictionary<int,RoleData> mRoleDic = new Dictionary<int,RoleData>();
        Dictionary<int,CharInfo> mCharDic = new Dictionary<int,CharInfo>();

        public override void StartStage()
        {
            base.StartStage();

            EventManager.Register<EventSendChar>(GetChar);
            EventManager.Register<EventDeleteChar>(DeleteChar);
            EventManager.Register<EventActionFinish>(ActionProcess);

            CameraManager.Instance.Set3DCamera(new Vector3(0.73f, 3.37f, -6.67f), Quaternion.Euler(13.2f, 2.2f, 1.5f),64);
            
            ChangeScene();
            Logicer.ChangeStageResult(1, "selectrole");
        }

        void ActionProcess( EventActionFinish e )
        {
            UIManager.Instance.HideWindow<UIWaiting>();
        }


        void GetChar(EventSendChar e)
        {
            CharInfo ci = new CharInfo();
            ci.charID = e.charID;
            ci.firstName = e.firstName;
            ci.lastName = e.lastName;
            ci.charClass = e.charClass;
            ci.charGrade = e.charGrage;
            ci.vipLevel = e.vipLevel;
            ci.charLevel = e.charLevel;
            ci.mapID = e.mapID;

            mCharDic[ci.charClass] = ci;

            if (mRoleDic.ContainsKey(ci.charClass))
            {
                mRoleDic[ci.charClass].haveChar = true;
            }

            UIWndRoleFrame uifr = UIManager.Instance.GetWindow<UIWndRoleFrame>();
            if(uifr!=null)
                uifr.SetInfo(ci.charClass, mRoleDic[ci.charClass].GetCurrentGradeName(ci.charGrade-1), ci.charLevel, 1);

            UIWndCreateRole uicr = UIManager.Instance.GetWindow<UIWndCreateRole>();
            if(uicr != null && uicr.isShow())
            {
                uicr.InCharExist(true);
                uicr.charLastNameLabel.text = ci.lastName;
            }
//             ci.charInfoHud = UIManager.Instance.CreateHud<UIHudCharInfo>();
//             ci.charInfoHud.SetInfo(e.lastName, e.vipLevel);
//             if (mRoleDic.ContainsKey(ci.charClass))
//             {
//                 Vector3 posextra = new Vector3(0, mRoleDic[ci.charClass].roleHeight, 0);
//                 Vector3 pos = CameraManager.Instance.cameraRF.WorldToViewportPoint(mRoleDic[ci.charClass].postion + posextra);
//                 pos.z = 0;
//                 pos = UICamera.currentCamera.ViewportToWorldPoint(pos);
// 
//                 ci.charInfoHud.transform.position = pos;
//             }       
        }

        void DeleteChar( EventDeleteChar e )
        {
            if (mCharDic.ContainsKey(e.charClass))
            {
//                 if (mCharDic[e.charClass].charInfoHud!= null)
//                     U3DMod.Destroy(mCharDic[e.charClass].charInfoHud.gameObject);
                UIWndRoleFrame uifr = UIManager.Instance.GetWindow<UIWndRoleFrame>();
                if (uifr != null)
                    uifr.SetInfo(e.charClass, mRoleDic[e.charClass].GetCurrentGradeName(0), -1, 1);

                UIWndCreateRole uicr = UIManager.Instance.GetWindow<UIWndCreateRole>();
                if (uicr != null && uicr.isShow())
                {
                    uicr.InCharExist(false);
                    uicr.charLastNameLabel.text = "请输入名字";
                }

                mCharDic.Remove(e.charClass);
            }
        }

        void ChangeScene()
        {
            if (!mInited)
                InitData();

            UIWndRoleFrame uifr = UIManager.Instance.ShowWindow<UIWndRoleFrame>();
            uifr.ViewDataHandler = FocusRole;
            UIWndCreateRole cr = UIManager.Instance.ShowWindow<UIWndCreateRole>();
            cr.InCharExist(true);
            cr.roleNameLabel.text = "名字";
            cr.SetProperty(0, 0, 0, 0, 0);
            cr.SetRoleNameShow("名字");
            cr.SetMasterName("名字", "名字", "名字");
            cr.Hide();

            foreach( int key in mRoleDic.Keys )
            {
                uifr.SetInfo(mRoleDic[key].roleID, mRoleDic[key].roleName, -1, 1);
            }

            UILoading ul = UIManager.Instance.ShowLoading();
            SceneManager.Instance.LoadGameScene("denglu", ul, Next);
        }

        void Next()
        {
            UnityMain.Instance.Timers.AddTimer(Init3DRes, .1f);
            
        }


        void InitData()
        {
            RoleData oc1 = new RoleData();
            oc1.roleID = 1;
            oc1.roleName = "船长";
            oc1.desc = "力量型英雄，七大洋的统帅，能肉能输出能辅助的全能型角色,无可争议的团队领袖";
            oc1.resName = "roledenglu1.unity3d";
            oc1.gradeName = new string[3] { "船长", "铁钩船长", "霸王船长" };
            oc1.roleSpriteName = new string[3] { "chuanzhang_cr1", "chuanzhang_cr2", "chuanzhang_cr3" };
            oc1.data = new int[5] { 6, 3, 9, 7, 5 };
            oc1.postion = new Vector3(-0.3f, 0, -0.6f);
            oc1.rotation = Quaternion.Euler(0, 180, 0);
            oc1.roleHeight = 2;
            oc1.OnFocus = OnFocusRole;
            mRoleDic[oc1.roleID] = oc1;

            RoleData oc2 = new RoleData();
            oc2.roleID = 3;
            oc2.roleName = "剑圣";
            oc2.desc = "敏捷型英雄，著名的用剑高手，剑技华丽，拥有最为强大的物理输出能力";
            oc2.resName = "roledenglu5.unity3d";
            oc2.gradeName = new string[3] { "剑圣", "剑痴", "剑神" };
            oc2.roleSpriteName = new string[3] { "jiansheng_cr1", "jiansheng_cr2", "jiansheng_cr3" };
            oc2.data = new int[5] { 10, 8, 5, 2, 5 };
            oc2.postion = new Vector3(2.028f, 0.023f, 5.87f); 
            oc2.rotation = Quaternion.Euler(0, 180, 0);
            oc2.roleHeight = 2f;
            oc2.OnFocus = OnFocusRole;
            mRoleDic[oc2.roleID] = oc2;

            RoleData oc3 = new RoleData();
            oc3.roleID = 2;
            oc3.roleName = "圣骑";
            oc3.desc = "力量型英雄，神的使者，可在前排当肉盾的奶妈，拥有各种治疗和支援能力";
            oc3.resName = "roledenglu6.unity3d";
            oc3.gradeName = new string[3] { "骑士", "圣骑士", "全能骑士" };
            oc3.roleSpriteName = new string[3] { "qishi_cr1", "qishi_cr1", "qishi_cr1" };
            oc3.data = new int[5] { 5, 3, 8, 4, 10 };
            oc3.postion = new Vector3(1.955f, 0.014f, -1.699f);
            oc3.rotation = Quaternion.Euler(0, 180, 0);
            oc3.roleHeight = 1.4f;
            oc3.OnFocus = OnFocusRole;
            mRoleDic[oc3.roleID] = oc3;

            RoleData oc4 = new RoleData();
            oc4.roleID = 4;
            oc4.roleName = "小黑";
            oc4.desc = "敏捷型英雄，曾经堕落黑暗的精灵游侠，强力的远程输出，拥有各种远程牵制手段";
            oc4.resName = "roledenglu4.unity3d";
            oc4.gradeName = new string[3] { "小黑", "游侠", "黑暗游侠" };
            oc4.roleSpriteName = new string[3] { "xiaohei_cr1", "xiaohei_cr1", "xiaohei_cr1" };
            oc4.data = new int[5] { 8, 5, 5, 6, 6 };
            oc4.postion = new Vector3(0.29f, 0.36f, 2.83f);
            oc4.rotation = Quaternion.Euler(0, 180, 0);
            oc4.roleHeight = 2.3f;
            oc4.OnFocus = OnFocusRole;
            mRoleDic[oc4.roleID] = oc4;

            RoleData oc5 = new RoleData();
            oc5.roleID = 6;
            oc5.roleName = "冰女";
            oc5.desc = "智力型英雄，强大的女祭司，擅长使用冰系魔法控制敌人，将敌人玩弄于股掌之间";
            oc5.resName = "roledenglu3.unity3d";
            oc5.gradeName = new string[3]{ "冰女", "霜女", "冰霜女神" };
            oc5.roleSpriteName = new string[3] { "bingnv_cr1", "bingnv_cr1", "bingnv_cr1" };
            oc5.data = new int[5] { 7, 4, 4, 9, 6 };
            oc5.postion = new Vector3(-1.9f, 0f, 3.38f);
            oc5.rotation = Quaternion.Euler(0, 180, 0);
            oc5.roleHeight = 2f;
            oc5.OnFocus = OnFocusRole;
            mRoleDic[oc5.roleID] = oc5;

            RoleData oc6 = new RoleData();
            oc6.roleID = 5;
            oc6.roleName = "火女";
            oc6.desc = "智力型英雄，闻名遐迩的魔导师，熟练掌握着使用火焰的技巧，拥有各种大范围的魔法输出能力";
            oc6.resName = "roledenglu2.unity3d";
            oc6.gradeName = new string[3]{ "火女", "炎女", "烈焰女神" };
            oc6.roleSpriteName = new string[3] {"huonv_cr1","huonv_cr2","huonv_cr3" };
            oc6.data = new int[5] { 9, 10, 3, 5, 3 };
            oc6.postion = new Vector3(-0.3f, 2f, 4.16f);
            oc6.rotation = Quaternion.Euler(0, 180, 0);
            oc6.roleHeight = 1f;
            oc6.OnFocus = OnFocusRole;
            mRoleDic[oc6.roleID] = oc6;

            mInited = true;
        }


        void Init3DRes()
        {
            foreach( int oc in mRoleDic.Keys.ToArray() )
            {
                mRoleDic[oc].Show(OnLoadTaskDown,true);
            }
        }

        void OnLoadTaskDown()
        {
            foreach (int oc in mRoleDic.Keys.ToArray())
            {
                if (mRoleDic[oc].role3DRes == null)
                    return;
            }
            CameraManager.Instance.InitPath(new Transform[6] { 
                                                                mRoleDic[1].role3DRes.transform, 
                                                                mRoleDic[2].role3DRes.transform, 
                                                                mRoleDic[3].role3DRes.transform, 
                                                                mRoleDic[4].role3DRes.transform, 
                                                                mRoleDic[5].role3DRes.transform, 
                                                                mRoleDic[6].role3DRes.transform
                                                               });
            UIManager.Instance.DestroyLoading();
        }

        void FocusRole( int roleid )
        {
            if(mRoleDic.ContainsKey(roleid))
            {
                OnFocusRole(mRoleDic[roleid]);
            }
        }

        void OnFocusRole(RoleData role)
        {
            UIWndCreateRole cr = UIManager.Instance.GetWindow<UIWndCreateRole>();
            if (cr != null && cr.isShow())
                return;

            CPreviewModel pmodel = role.role3DRes.GetComponent<CPreviewModel>();
            pmodel.PlayShow(role.roleName,true);

            U3DMod.SetActive(UIManager.Instance.hudRoot,false);

            UIManager.Instance.HideWindow<UIWndRoleFrame>();

            cr = UIManager.Instance.ShowWindow<UIWndCreateRole>();
            cr.InCharExist(true);
            cr.roleNameLabel.text = role.roleName;
            cr.SetProperty(role.data[0], role.data[1], role.data[2], role.data[3], role.data[4]);
            cr.SetRoleNameShow(role.desc);
            cr.SetMasterName(role.gradeName[0],role.gradeName[1],role.gradeName[2]);
            cr.SetRoleScriteName(role.roleSpriteName[0],role.roleSpriteName[1],role.roleSpriteName[2]);
            cr.PlaySound(role.roleID);
            cr.OnShowActionHandler = () =>
            {
                pmodel.PlayAction();
            };
            cr.OnHideHandler = () =>
            {
                UIManager.Instance.ShowWindow<UIWndRoleFrame>();
                pmodel.PlayShow(role.roleName, false);
               
            };

            cr.OnSelectRoleHandler = () =>
            {
                Logicer.EnterGame(mCharDic[role.roleID].charID, 12);
            };

            cr.OnDeleteCharHandler = () =>
            {
                UIManager.Instance.ShowWindow<UIWaiting>();
                Logicer.DeleteChar(mCharDic[role.roleID].charID);
//                     if (mCharDic.ContainsKey(role.roleID))
//                         mCharDic[role.roleID].charInfoHud.Show();

            };
            cr.OnCreateCharHandler = () =>
            {
                UIManager.Instance.ShowWindow<UIWaiting>();
                Logicer.CreateChar(cr.charFirstNameIcon.spriteName, cr.charLastNameLabel.text, (short)role.roleID);
//                     if (mCharDic.ContainsKey(role.roleID))
//                         mCharDic[role.roleID].charInfoHud.Show();
            };

            if( mCharDic.ContainsKey(role.roleID ) )
            {
                cr.InCharExist(true);
                cr.charLastNameLabel.text = mCharDic[role.roleID].lastName;
            }
            else
            {
                cr.InCharExist(false);
                cr.charLastNameLabel.text = "请输入名字";
            }
        }

        public override void EndStage()
        {
            EventManager.Remove<EventSendChar>(GetChar);
            EventManager.Remove<EventDeleteChar>(DeleteChar);
            EventManager.Remove<EventActionFinish>(ActionProcess);

            UIManager.Instance.ClearAllWindow();
            
            mRoleDic.Clear();
//             foreach(int key in mCharDic.Keys.ToArray())
//             {
//                 if (mCharDic[key].charInfoHud != null)
//                     U3DMod.Destroy(mCharDic[key].charInfoHud.gameObject);
//             }

            mCharDic.Clear();

            UIManager.Instance.ClearAllHud();
            CameraManager.Instance.RemovePath();
        }
    }
}
