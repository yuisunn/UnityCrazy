using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;
using System.Collections.Generic;
using System.Linq;
using SPSGame.CsShare.Data;

namespace SPSGame.Unity
{
    public class UIWndEquipShow : UIWndBase
    {

        /// <summary>
        /// 道具格子
        /// </summary>
        public GameObject itemPrefeb = null;
        /// <summary>
        /// 所有物品面板
        /// </summary>
        public UIPanel allList = null;
        /// <summary>
        /// 所有物品面板Grid
        /// </summary>
        public UIGrid itemGrid = null;
        /// <summary>
        /// 返回
        /// </summary>
        public GameObject backBtn = null;

        List<UIItemView> cacheItemList = new List<UIItemView>();

        private long changeEquipId = 0;
        private short equipLocation = 0;

        [HideInInspector]
        public int cellNum = 50;

        protected override void Awake()
        {
            base.Awake();
            
            //事件监听
            EventManager.Register<EventShowChooseEquipInfo>(ShowChooseEquipInfoEvent);

            ListenOnClick(backBtn, OnClickBack);

            CreateItemCell(cellNum);
        }

        public void SetChangeEquipIdAndLocation(long equipId, short LocationNo)
        {
            changeEquipId = equipId;
            equipLocation = LocationNo;
        }

        /// <summary>
        /// 展示可更换的装备
        /// </summary>
        public void ShowChooseEquipInfoEvent(EventShowChooseEquipInfo e)
        {
            SetItemViewList(e.info);

            Show();
        }

        public void ShowChooseEquipInfo (long equipId)
        {
            ActionParam param = new ActionParam();
            param["EquipId"] = equipId;

            LogicMain.Instance.CallLogicAction(LogicActionDefine.GetBagEquip, param);
        }

        /// <summary>
        /// 创建物品格子
        /// </summary>
        void CreateItemCell (int cellNum)
        {
            GameObject go = null;
            GameObject icon = null;
            UIItemView itemView = null;
            for (int i = 0; i < cellNum; ++i)
            {
                go = NGUITools.AddChild(itemGrid.gameObject, itemPrefeb);
                itemPrefeb.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = "";
                U3DMod.SetActive(go, true);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                icon = go.transform.FindChild("Icon").gameObject;
                itemView = go.GetComponent<UIItemView>();
                cacheItemList.Add(itemView);

                ListenOnClick(icon, OnClickIcon);
            }

            itemGrid.Reposition();

            //int currentCellNum = cacheItemList.Count;

            //if (currentCellNum < cellNum)
            //{
            //    GameObject go = null;
            //    GameObject icon = null;
            //    UIItemView itemView = null;
            //    for (int i = 0; i < cellNum - currentCellNum; ++i)
            //    {
            //        go = NGUITools.AddChild(itemGrid.gameObject, itemPrefeb);
            //        itemPrefeb.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = "";
            //        U3DMod.SetActive(go, true);
            //        go.transform.localPosition = Vector3.zero;
            //        go.transform.localScale = Vector3.one;
            //        icon = go.transform.FindChild("Icon").gameObject;
            //        itemView = go.GetComponent<UIItemView>();
            //        cacheItemList.Add(itemView);

            //        ListenOnClick(icon, OnClickIcon);
            //    }

            //    itemGrid.Reposition();
            //}
            //else if (currentCellNum > cellNum)
            //{
            //    for (int i = cellNum; i < currentCellNum; ++i)
            //    {
            //        cacheItemList[i].gameObject.SetActive(false);
            //    }
            //}
        }

        /// <summary>
        /// 设置装备格子
        /// </summary>
        void SetItemViewList(List<Dictionary<string, string>> datas)
        {
            foreach (UIItemView view in cacheItemList)
            {
                if (!view.gameObject.activeSelf)
                {
                    view.gameObject.SetActive(true);
                }

                view.Clear();
            }

            foreach (Dictionary<string, string> data in datas)
            {
                int pos = short.Parse(data["Location"]) - 1;
                long equipId = long.Parse(data["EquipID"]);
                int equipNo = int.Parse(data["EquipNo"]);
                short quality = short.Parse(data["Quality"]);

                if (pos >= cacheItemList.Count)
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

                UIItemView view = cacheItemList[pos];
                view.ShowPlayerEquip(equipId, 1, quality, spriteName, true);
            }
        }

        public void OnClickBack (GameObject obj)
        {
            Hide();
        }

        public void OnClickIcon (GameObject obj)
        {
            UIItemView itemView = obj.transform.parent.gameObject.GetComponent<UIItemView>();

            if (null == itemView)
            {
                return;
            }

            if (0 >= itemView.itemId)  //表示此装备栏中没有装备,此种情况不应该出现
            {
                return;
            }

            else //表示此装备栏中有装备
            {
                long itemId = itemView.itemId;

                //创建悬浮界面
                UIEquipSuspension itemInfo = UIManager.Instance.GetWindow<UIEquipSuspension>();

                if (null == itemInfo)
                {
                    itemInfo = UIManager.Instance.ShowWindow<UIEquipSuspension>();
                }

                //修改界面的显示深度
                itemInfo.GetComponent<UIPanel>().depth = allList.depth + 1;

                itemInfo.SetChangeEquipIdAndLocation(changeEquipId, equipLocation);
                itemInfo.ShowItemDetailInfo(itemId);
                itemInfo.CloseWndHandler = Hide;
                itemInfo.SetPlane(2);
            }
        }

        public override void Destroy()
        {
            EventManager.Remove<EventShowChooseEquipInfo>(ShowChooseEquipInfoEvent);

            base.Destroy();
        }
    }
}

