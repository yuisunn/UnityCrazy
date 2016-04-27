using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;


namespace SPSGame.Unity
{

    public class UIEquipSuspension : UIWndBase
    {
        /// <summary>
        /// 装备名称
        /// </summary>
        public UILabel equmentNameLab = null;
        /// <summary>
        /// 战斗力
        /// </summary>
        public UILabel fightingNumberLab = null;
        /// <summary>
        /// 强化等级
        /// </summary>
        public UILabel strenthLevelLab = null;
        /// <summary>
        /// 装备类型
        /// </summary>
        public UILabel equmentTypeLab = null;
        /// <summary>
        /// 出售价格
        /// </summary>
        public UILabel sellPriceLab = null;
        /// <summary>
        /// 图标名称
        /// </summary>
        public UISprite iconSpr = null;
        /// <summary>
        /// 星级图标列表
        /// </summary>
        public GameObject[] starLevelLab = null;
        /// <summary>
        /// 基础属性列表
        /// </summary>
        public UILabel[] basePropertyLab = null;
        /// <summary>
        /// 装备效果列表
        /// </summary>
        public UILabel[] equipmentEffectLab = null;
        /// <summary>
        /// 装备效果名称
        /// </summary>
        public UILabel[] equipmentEffectNameLab = null;
        /// <summary>
        /// 装备升级后的颜色变化
        /// </summary>
        public Color purple = new Color(0.36f, 0.12f, 0.12f, 1);
        public Color orange = new Color(0.94f, 0.38f, 0.08f, 1);

        public GameObject backPackPanle = null;
        public GameObject playPanle = null;
        public GameObject rolePanle = null;

        public GameObject streathBtn_backPack = null;
        public GameObject destroyBtn_backPack = null;
        public GameObject streathBtn_play = null;
        public GameObject changeBtn_play = null;
        public GameObject UnLoadBtn_play = null;
        public GameObject LoadBtn_role = null;

        public GameObject backBtn = null;
        //缓存服务器信息
        private Dictionary<string, string> dicInfo = new Dictionary<string,string>();
        //存储此时展示详细信息的物品的ID
        private long detailItemId = 0;
        private long changeEquipId = 0;
        private short equipLocation = 0;

        protected override void Awake()
        {
            base.Awake();

            ListenOnClick(changeBtn_play, OnClickChange);
            ListenOnClick(streathBtn_backPack, OnClickUpgrade);
            ListenOnClick(streathBtn_play, OnClickUpgrade);
            ListenOnClick(UnLoadBtn_play, OnClickUnLoad);
            ListenOnClick(destroyBtn_backPack, OnClickDestroy);
            ListenOnClick(backBtn, OnClickBack);
            ListenOnClick(LoadBtn_role, OnClickLoad);

            SetPlane(0);

            //事件监听
            EventManager.Register<EventItemInfo>(ItemInfo);
        }

        /// <summary>
        /// 设置显示按钮的逻辑
        /// </summary>
        /// <param name="type"></param>
        public void SetPlane(int type)
        {
            if (0 == type)
            {
                U3DMod.SetActive(backPackPanle, true);
                U3DMod.SetActive(playPanle, false);
                U3DMod.SetActive(rolePanle, false);
            }
            else if (1 == type)
            {
                U3DMod.SetActive(backPackPanle, false);
                U3DMod.SetActive(playPanle, true);
                U3DMod.SetActive(rolePanle, false);
            }
            else
            {
                U3DMod.SetActive(backPackPanle, false);
                U3DMod.SetActive(playPanle, false);
                U3DMod.SetActive(rolePanle, true);
            }
        }

        public void ShowItemDetailInfo (long itemId)
        {
            detailItemId = itemId;

            //每次窗口激活时向逻辑层发送请求物品详细信息的LogicAction
            ActionParam param = new ActionParam();
            param["ItemId"] = itemId;
            LogicMain.Instance.CallLogicAction(LogicActionDefine.GetItemDetailInfo, param);
        }

        public void SetChangeEquipIdAndLocation(long equipId, short LocationNo)
        {
            changeEquipId = equipId;
            equipLocation = LocationNo;
        }

        /// <summary>
        /// 接受服务器的物品信息 并解析
        /// </summary>
        /// <param name="e"></param>
        void ItemInfo(EventItemInfo e) 
        {
            dicInfo = e.info;
            if (e.info == null)
            {
                return;
            }
            else 
            {
                string strName = dicInfo["Name"];
                string iconSpriteName = dicInfo["ID"];
                string itemType = dicInfo["Type"];
                string sellPrice = dicInfo["Price"];
                //string fingt = dicInfo["Fingt"];
                //string strebtNum = dicInfo["Strength"];
                int starSpriteNum = int.Parse( dicInfo["Quality"]);
                //int propretyNum = int.Parse(dicInfo["BasePropretyNum"]);
                //string propretyStr = dicInfo["BaseProprety"];
                //int effectLength = int.Parse(dicInfo["EffectLength"]);
                //string effectStr = dicInfo["EffectStr"];
                //string effectName = dicInfo["EffectName"];

                //缓存属性的信息
                //string[] propretyStrs = propretyStr.Split(',');

                ////缓存效果信息
                //string[] effectStrs = effectStr.Split(',');
                //string[] effectNames = effectName.Split(',');


                //从配置文件中取到道具的spriteName
                Dictionary<string, string> req = new Dictionary<string, string>();
                req.Add("ID", iconSpriteName);
                List<Dictionary<string, string>> res = DataManager.Instance.mIconData.GetRowDataByMultiColValue(req);
                string spriteName = res[0]["SpriteName"];

                SetEquipmentName(strName, spriteName, itemType, sellPrice);

                SetEquipmentStarLevel(starSpriteNum);
                //SetFighting(fingt, strebtNum);
                //SetBaseEquipmentProperty(propretyStrs, propretyNum);
                //SetEquipmentEffect(effectStrs, effectNames, effectLength);
            }

            Show();
        }
        /// <summary>
        /// 设置装备名称、显示图标、装备类型、出售价格 最多8个字 4-6为宜
        /// </summary>
        /// <param name="strName"></param>
        public void SetEquipmentName(string strName, string iconName, string equmeType, string sell)
        {
            if (strName.Length <= 8)
            {
                equmentNameLab.text = strName;
                iconSpr.spriteName = iconName;

                switch (equmeType)
                {
                    case "1" :
                    {
                        equmentTypeLab.text = "装备";
                        break;
                    }
                    case "2":
                    {
                        equmentTypeLab.text = "卷轴";
                        break;
                    }
                    case "3":
                    {
                        equmentTypeLab.text = "碎片";
                        break;
                    }
                    case "4":
                    {
                        equmentTypeLab.text = "任务";
                        break;
                    }
                    case "5":
                    {
                        equmentTypeLab.text = "宝石";
                        break;
                    }
                    case "6":
                    {
                        equmentTypeLab.text = "药品";
                        break;
                    }
                    case "7":
                    {
                        equmentTypeLab.text = "材料";
                        break;
                    }
                    case "8":
                    {
                        equmentTypeLab.text = "礼包";
                        break;
                    }
                    case "9":
                    {
                        equmentTypeLab.text = "其他";
                        break;
                    }
                    default :
                    {
                        break;
                    }
                        
                }
                sellPriceLab.text = sell;
            }
        }
        /// <summary>
        /// 设置战斗力
        /// </summary>
        public void SetFighting(string fingtNum, string strenthNum)
        {
            fightingNumberLab.text = fingtNum;
            strenthLevelLab.text = strenthNum;
        }
        /// <summary>
        /// 设置装备的星级
        /// </summary>
        public void SetEquipmentStarLevel(int starNumber)
        {
            if (starNumber == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    U3DMod.SetActive(starLevelLab[i], true);
                }

                iconSpr.spriteName = "bai";
            }
            if (starNumber == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    U3DMod.SetActive(starLevelLab[i], true);
                }
                iconSpr.spriteName = "lv";
            }
            if (starNumber == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    U3DMod.SetActive(starLevelLab[i], true);
                }
                iconSpr.spriteName = "lan";
            }
            if (starNumber == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    U3DMod.SetActive(starLevelLab[i], true);
                }
                iconSpr.spriteName = "zi";
            }
            if (starNumber == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    U3DMod.SetActive(starLevelLab[i], true);
                }
                iconSpr.spriteName = "cheng";
            }
            if (starNumber == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    U3DMod.SetActive(starLevelLab[i],true);
                }
                iconSpr.spriteName = "hong";
            }
        }

        /// <summary>
        /// 设置装备基础属性
        /// </summary>
        public void SetBaseEquipmentProperty(string[] property, int propertyNum)
        {
            for (int i = 0; i < propertyNum; i++)
            {
                basePropertyLab[i].text = property[i];
            }
        }
        /// <summary>
        /// 设置装备效果  效果名称（主动/被动）
        /// </summary>
        public void SetEquipmentEffect(string[] effectName, string[] effectStr, int effectLength)
        {
            for (int i = 0; i < effectLength; i++)
            {
                equipmentEffectNameLab[i].text = effectName[i];
                equipmentEffectLab[i].text = effectStr[i];
            }
        }
        /// <summary>
        /// 按钮监听事件
        /// </summary>
        /// <param name="obj"></param>
        void OnClickChange(GameObject obj)
        {
            //显示装备选择界面
            UIWndEquipShow uiEquipShow = UIManager.Instance.GetWindow<UIWndEquipShow>();

            if (null == uiEquipShow)
            {
                uiEquipShow = UIManager.Instance.ShowWindow<UIWndEquipShow>();
            }

            //修改界面的显示深度
            uiEquipShow.GetComponent<UIPanel>().depth = this.gameObject.GetComponent<UIPanel>().depth + 1;
            uiEquipShow.allList.depth = uiEquipShow.GetComponent<UIPanel>().depth + 1;

            uiEquipShow.ShowChooseEquipInfo(detailItemId);
            uiEquipShow.SetChangeEquipIdAndLocation(changeEquipId, equipLocation);

            Hide();
        }
        void OnClickUpgrade(GameObject obj)
        {

        }
      
        void OnClickUnLoad(GameObject obj)
        {
            //暂时用来测试卸下逻辑

            //向服务器发送换装备的消息
            ActionParam param = new ActionParam();
            param["BeforeEquipId"] = detailItemId;
            param["NewEquipId"] = (long)0;
            param["EquipPlanNo"] = (short)1;
            param["EquipLocationNo"] = equipLocation;

            LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeEquip, param);

            Hide();
        }
        void OnClickDestroy(GameObject obj)
        {
			//向服务器发送消息 
            ActionParam param = new ActionParam();
            param["Num"] = (short)1;
            param["ItemID"] = detailItemId;
            param["IsUse"] = 0;

            LogicMain.Instance.CallLogicAction(LogicActionDefine.UseItem, param);

            //关闭界面
            Hide();
        }

        void OnClickLoad(GameObject obj)
        {
            //向服务器发送换装备的消息
            ActionParam param = new ActionParam();
            param["BeforeEquipId"] = changeEquipId;
            param["NewEquipId"] = detailItemId;
            param["EquipPlanNo"] = (short)1;
            param["EquipLocationNo"] = equipLocation;

            LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeEquip, param);

            changeEquipId = 0;
            equipLocation = 0;

            CloseWndHandler();
            Hide();
            CloseWndHandler = null;
        }

        void OnClickBack(GameObject go)
        {
            if (go == backBtn)
            {
                Hide();
            }
        }
        public override void Destroy()
        {
            EventManager.Remove<EventItemInfo>(ItemInfo);

            base.Destroy();
        }
    }
}