using UnityEngine;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class UIWndCharDetial : UIWndBase
    {
        public UITexture texture = null;
        public GameObject closeBtn = null;
        //public GameObject fashionBtn;
        //public GameObject[] equipmentBtn = null;
        public GameObject FlagBtn = null;

        public UILabel forceLab = null;
        public UILabel speedLab = null;
        public UILabel witLab = null;
        public UILabel forceUpLab = null;
        public UILabel speedUpLab = null;
        public UILabel witUpLab = null;

        public UILabel maxHpLab = null;
        public UILabel maxATKLab = null;
        public UILabel magicProtectLab = null;
        public UILabel PhysicProtectedLab = null;
        public UILabel magicResistLab = null;
        public UILabel PhysicCritLab = null;
        public UILabel CritHitLab = null;

        public UIItemView[] itemViews = null;

        public GameObject textrueModle = null;

        public GameObject ZB1 = null;
        public GameObject ZB2 = null;
        public GameObject ZB3 = null;
        //public GameObject ZB4;

        public GameObject zbBtn1 = null;
        public GameObject zbBtn2 = null;
        public GameObject zbBtn3 = null;
        public GameObject LeftBtn = null;
        public GameObject RightBtn = null;
        //public GameObject zbBtn4;
        //玩家是否是战斗状态
        private bool isFightingSatte = false;
        //记录开始拖拽的鼠标位置
        private float starPos = 0;
        //记录角度的变化
        private float angle = 180;
        //角色模型

        GameObject mRTmodel = null;
        Camera mRTCamera = null;

        private int mHeroResID = -1;

        public VoidDelegate OpenFashionHandler = null;
        public VoidDelegate OpenEquipHandler = null;
        public VoidDelegate OpenFlagHandler = null;


        protected override void Awake()
        {
            base.Awake();
            ListenOnClick(closeBtn, OnClickClose);
           // ListenOnClick(fashionBtn, OnClickFashion);
            //for (int i = 0; i < equipmentBtn.Length; i++)
            //{
            //    ListenOnClick(equipmentBtn[i], OnClickEquipment);
            //}


            for (int i = 0; i < itemViews.Length; ++i )
            {
                GameObject icon = itemViews[i].gameObject.transform.FindChild("Icon").gameObject;
                icon.GetComponent<UISprite>().spriteName = "";

                ListenOnClick(itemViews[i].gameObject.transform.FindChild("Icon").gameObject, OnClickEquipment);
            }

            ListenOnClick(FlagBtn, OnClickFlag);
            ListenOnDragStart(textrueModle, OnDragModleStart);
            ListenOnDrag(textrueModle, OnDragModle);
            ListenOnClick(zbBtn1, OnClickZB);
            ListenOnClick(zbBtn2, OnClickZB);
            ListenOnClick(zbBtn3, OnClickZB);
            ListenOnClick(LeftBtn, OnClickBtn);
            ListenOnClick(RightBtn, OnClickBtn);

            //事件监听
            EventManager.Register<EventRoleBaseProperty>(RoleBasePropertyEvent);
            EventManager.Register<EventRoleDetailProperty>(RoleDetailPropertyEvent);
            EventManager.Register<EventShowPLayerEquipInfo>(ShowEquipInfo);

            ActionParam param = new ActionParam();
            LogicMain.Instance.CallLogicAction(LogicActionDefine.ShowPlayerAttr, param);

            ActionParam param2 = new ActionParam();
            param2["EquipPlanNo"] = (short)1;
            LogicMain.Instance.CallLogicAction(LogicActionDefine.GetEquipInfoList, param2);
        }

        protected override void Start()
        {
            base.Start();
            CreateModel();
        }

        public void SetResInfo( int heroresid )
        {
            mHeroResID = heroresid;
        }

        void CreateModel()
        {
            string filename = DataManager.Instance.GetModelRes(mHeroResID);
            string sourcename = PathMod.GetPureName(filename);

            AssetBundleManager.Instance.LoadMonsterByLoader(filename, sourcename, (go) =>
            {
                mRTmodel = U3DMod.Clone(go as GameObject);
                mRTmodel.transform.position = new Vector3(100, 0, 0);
                mRTmodel.transform.rotation = Quaternion.Euler(0, -180, 0);

                GameObject obj = new GameObject("_RT Camera");
                mRTCamera = obj.AddComponent<Camera>();
                obj.transform.position = new Vector3(100, 1, -5);
                mRTCamera.cullingMask = 1;
                mRTCamera.orthographic = true;
                mRTCamera.orthographicSize = 1.3f;

                RenderTexture rd = new RenderTexture(400, 600, 1);
                mRTCamera.targetTexture = rd;
                texture.mainTexture = rd;
            });
        }

        /// <summary>
        /// 点击时装按钮
        /// </summary>
        /// <param name="obj"></param>
        void OnClickFashion(GameObject obj) 
        {
            if (OpenFashionHandler != null)
                OpenFashionHandler();
        }
        /// <summary>
        /// 点击装备框
        /// </summary>
        /// <param name="obj"></param>
        void OnClickEquipment(GameObject obj) 
        {
            //if (OpenEquipHandler != null)
            //    OpenEquipHandler();

            UIItemView itemView = obj.transform.parent.gameObject.GetComponent<UIItemView>();

            if (null == itemView)
            {
                return;
            }

            int location = -1;
            for (int i = 0; i < itemViews.Length; ++i)
            {
                if (itemViews[i] == itemView)
                {
                    location = i + 1;
                }
            }

            if (0 >= itemView.itemId)  //表示此装备栏中没有装备
            {
                //显示装备选择界面
                UIWndEquipShow uiEquipShow = UIManager.Instance.GetWindow<UIWndEquipShow> ();

                if (null == uiEquipShow)
                {
                    uiEquipShow = UIManager.Instance.ShowWindow<UIWndEquipShow>();
                }

                //修改界面的显示深度
                uiEquipShow.GetComponent<UIPanel>().depth = this.gameObject.GetComponent<UIPanel>().depth + 1;
                uiEquipShow.allList.depth = uiEquipShow.GetComponent<UIPanel>().depth + 1;

                //设置换的装备所属的格子
                uiEquipShow.SetChangeEquipIdAndLocation((long)0, (short)location);

                uiEquipShow.ShowChooseEquipInfo(0);
            }
            else //表示此装备栏中有装备
            {
                if (-1 == location)
                {
                    return;
                }

                long itemId = itemView.itemId;

                ////创建装备选择界面
                //UIWndEquipShow uiEquipShow = UIManager.Instance.GetWindow<UIWndEquipShow>();

                //if (null == uiEquipShow)
                //{
                //    uiEquipShow = UIManager.Instance.ShowWindow<UIWndEquipShow>();
                //    UIManager.Instance.HideWindow<UIWndEquipShow>();
                //}

                ////设置换的装备所属的格子
                //uiEquipShow.SetChangeEquipIdAndLocation(itemId, (short)location);


                //创建悬浮界面
                UIEquipSuspension itemInfo = UIManager.Instance.GetWindow<UIEquipSuspension>();

                if (null == itemInfo)
                {
                    itemInfo = UIManager.Instance.ShowWindow<UIEquipSuspension>();
                }

                //修改界面的显示深度
                itemInfo.GetComponent<UIPanel>().depth = this.gameObject.GetComponent<UIPanel>().depth + 1;

                itemInfo.ShowItemDetailInfo(itemId);
                itemInfo.SetChangeEquipIdAndLocation(itemId, (short)location);
                itemInfo.SetPlane(1);
            }
        
        }
        /// <summary>
        /// 点击战旗
        /// </summary>
        /// <param name="obj"></param>
        void OnClickFlag(GameObject obj) 
        {
            if (OpenFlagHandler != null)
                OpenFlagHandler();
        }
        /// <summary>
        /// 监听开始拖拽
        /// </summary>
        /// <param name="del"></param>
        void OnDragModleStart(GameObject del)
        {
            starPos = Input.mousePosition.x;
        }
        /// <summary>
        /// 监听拖拽
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="o"></param>
        void OnDragModle(GameObject obj, Vector2 o)
        {
            //鼠标当前帧的X坐标
            float nextPos = UICamera.lastEventPosition.x;
            //当前帧和上一帧的滑动距离
            float distance = nextPos - starPos;
            //每一帧应转动的角度
            float onceAngle= distance * 0.18f * 5 / Mathf.PI;
            angle -= onceAngle;
            //向右滑动
            if (nextPos - starPos > 0)
            {
                //模型逆时针旋转
                mRTmodel.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            }
            //向左滑动
            else if (nextPos - starPos < 0)
            {
                //模型顺时针旋转
                mRTmodel.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            }
            starPos = nextPos;
        }
        /// <summary>
        /// 装备面板的切换
        /// </summary>
        /// <param name="obj"></param>
        void OnClickZB(GameObject obj) 
        {
            if (!isFightingSatte)
            {
                if (obj == zbBtn1)
                {
                    U3DMod.SetActive(ZB1, true);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, false);
                    obj.GetComponent<UISprite>().spriteName = "hong1";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                }
                else if (obj == zbBtn2)
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, true);
                    U3DMod.SetActive(ZB3, false);
                    obj.GetComponent<UISprite>().spriteName = "hong2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                }
                else if (obj == zbBtn3)
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, true);
                    obj.GetComponent<UISprite>().spriteName = "hong3";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                }
            }
        }


        /// <summary>
        /// 面板切换
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBtn(GameObject obj) 
        {
            if (obj == LeftBtn)
            {
                if (U3DMod.isActive(ZB1)) 
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, true);
                    zbBtn3.GetComponent<UISprite>().spriteName = "hong3";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                }
              else if (U3DMod.isActive(ZB2))
                {
                    U3DMod.SetActive(ZB1, true);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, false);
                    zbBtn1.GetComponent<UISprite>().spriteName = "hong1";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                } 
               else if (U3DMod.isActive(ZB3))
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, true);
                    U3DMod.SetActive(ZB3, false);
                    zbBtn2.GetComponent<UISprite>().spriteName = "hong2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                }
            }
            if(obj == RightBtn)
            {
                if (U3DMod.isActive(ZB1))
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, true);
                    U3DMod.SetActive(ZB3, false);
                    zbBtn2.GetComponent<UISprite>().spriteName = "hong2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                }
                else if (U3DMod.isActive(ZB2))
                {
                    U3DMod.SetActive(ZB1, false);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, true);
                    zbBtn3.GetComponent<UISprite>().spriteName = "hong3";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn1.GetComponent<UISprite>().spriteName = "lv1";
                }
               else if (U3DMod.isActive(ZB3))
                {
                    U3DMod.SetActive(ZB1, true);
                    U3DMod.SetActive(ZB2, false);
                    U3DMod.SetActive(ZB3, false);
                    zbBtn1.GetComponent<UISprite>().spriteName = "hong1";
                    zbBtn2.GetComponent<UISprite>().spriteName = "lv2";
                    zbBtn3.GetComponent<UISprite>().spriteName = "lv3";
                }
            }
        }

        /// <summary>
        /// 是否是战斗状态 设置装备方案按钮的颜色
        /// </summary>
        /// <param name="isCanClick"></param>
        public void SetZbBtnColor(bool isFingting)
        {
            if (isFingting)
            {
                zbBtn1.GetComponent<UISprite>().color = new Color(0.28f, 0.27f, 0.27f, 1);
                zbBtn2.GetComponent<UISprite>().color = new Color(0.28f, 0.27f, 0.27f, 1);
                zbBtn3.GetComponent<UISprite>().color = new Color(0.28f, 0.27f, 0.27f, 1);
            }
            else
            {
                zbBtn1.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                zbBtn2.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                zbBtn3.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            }
        }
        public override void Destroy()
        {
            if (mRTCamera != null)
                U3DMod.Destroy(mRTCamera.gameObject);
            if (mRTmodel != null)
                U3DMod.Destroy(mRTmodel);

            EventManager.Remove<EventRoleBaseProperty>(RoleBasePropertyEvent);
            EventManager.Remove<EventRoleDetailProperty>(RoleDetailPropertyEvent);
            EventManager.Remove<EventShowPLayerEquipInfo>(ShowEquipInfo);

            base.Destroy();
        }

        /// <summary>
        /// 设置角色基本属性
        /// </summary>
        /// <param name="roleBaseProDic"></param>
        public void SetRoleBaseProperty(int force,int speed,int wit,float forceUp, float speedUp,float witUp ) 
        {
            forceLab.text = "力量："+force.ToString();
            speedLab.text = "敏捷：" + speed.ToString();
            witLab.text = "智力：" + wit.ToString();
            forceUpLab.text = "力量成长：" + forceUp.ToString();
            speedUpLab.text = "敏捷成长：" + speedUp.ToString();
            witUpLab.text = "智力成长：" + witUp.ToString();
        }


        /// <summary>
        /// 设置角色详细属性
        /// </summary>

        public void SetRoleDetailProperty(int maxHp, int physicATK, int magicATK, int physicProtect, int magicProtect, int physicCrit, int critHit) 
        {
            maxHpLab.text ="最大生命：" + maxHp.ToString();
            maxATKLab.text = "物理攻击：" + physicATK.ToString();
            magicProtectLab.text = "魔法强度：" + magicATK.ToString();
            PhysicProtectedLab.text = "物理护甲：" + physicProtect.ToString();
            magicResistLab.text = "魔法抗性：" + magicProtect.ToString();
            PhysicCritLab.text = "物理暴击：" + physicCrit.ToString();
            CritHitLab.text = "暴击伤害：" + critHit.ToString(); 
        }


        /// <summary>
        /// 设置装备图标
        /// </summary>
        public void  SetEquipmentIcon(string icon1,string icon2,string icon3, string icon4, string icon5, string icon6)
        {
            //icons[0].spriteName = icon1;
            //icons[1].spriteName = icon2;
            //icons[2].spriteName = icon3;
            //icons[3].spriteName = icon4;
            //icons[4].spriteName = icon5;
            //icons[5].spriteName = icon6;
        } 
     

        /// <summary>
        /// 设置装备格子
        /// </summary>
        void SetItemViewList (List<Dictionary<string, string>> datas)
        {
            foreach (Dictionary<string, string> data in datas)
            {
                int pos = short.Parse(data["Location"]) - 1;
                long equipId = long.Parse(data["EquipID"]);
                int equipNo = int.Parse(data["EquipNo"]);
                short quality = short.Parse(data["Quality"]);
                bool isEquiped = (data["IsEquip"] == "1" ? true : false);
                if (pos >= 6)
                {
                    continue;
                }

                //从配置文件中取到道具的spriteName
                Dictionary<string, string> req = new Dictionary<string, string>();
                req.Add("ID", equipNo.ToString());
                List<Dictionary<string, string>> res = DataManager.Instance.mIconData.GetRowDataByMultiColValue(req);

                if (null == res || 0 == res.Count)
                {
                    continue;
                }

                string spriteName = res[0]["SpriteName"];

                UIItemView view = itemViews[pos];
                view.ShowPlayerEquip(equipId, 1, quality, spriteName, isEquiped);
            }
        }

       /// <summary>
       /// 设置人物基础属性的事件
       /// </summary>
       /// <param name="e"></param>
        void RoleBasePropertyEvent (EventRoleBaseProperty e)
        {
            SetRoleBaseProperty(e.forceLab, e.speedLab, e.witLab, e.forceUpLab, e.speedUpLab, e.witUpLab);
        }

        /// <summary>
        /// 设置人物详细属性的事件
        /// </summary>
        /// <param name="e"></param>
        void RoleDetailPropertyEvent (EventRoleDetailProperty e)
        {
            SetRoleDetailProperty(e.maxHpLab, e.phyATKLab, e.magicATKLab, e.physicProtectedLab, e.magicProtectLab, e.physicCritLab, e.critHitLab);
        }

        /// <summary>
        /// 显示人物的装备信息
        /// </summary>
        void ShowEquipInfo(EventShowPLayerEquipInfo e)
        {
            SetItemViewList(e.info);
        }
    }
}