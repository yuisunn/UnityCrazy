using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;
using System.Collections.Generic;
using System.Linq;
using SPSGame.CsShare.Data;

namespace SPSGame.Unity
{
    public class UIWndBackPack : UIWndBase
    {
        /// <summary>
        /// 未解锁的道具格子
        /// </summary>
        public GameObject itemLock = null;
        /// <summary>
        /// 道具格子
        /// </summary>
        public GameObject itemPrefeb = null;
        /// <summary>
        /// 所有物品面板
        /// </summary>
        public UIGrid itemGrid = null;
        /// <summary>
        /// 装备面板
        /// </summary>
        public UIGrid equipmentItem = null;
        /// <summary>
        /// 卷轴面板
        /// </summary>
        public UIGrid scrollItem = null;
        /// <summary>
        /// 碎片面板
        /// </summary>
        public UIGrid pieceItem = null;
        /// <summary>
        /// 其他面板
        /// </summary>
        public UIGrid otherItem = null;

        /// <summary>
        /// 进度条
        /// </summary>
        public UIProgressBar progress = null;
        /// <summary>
        /// 返回
        /// </summary>
        public GameObject backBtn = null;
        /// <summary>
        /// 综合标签
        /// </summary>
        public GameObject allTab = null;
        /// <summary>
        /// 武器标签
        /// </summary>
        public GameObject equipTab = null;
        /// <summary>
        /// 卷轴标签
        /// </summary>
        public GameObject scrollTab = null;
        /// <summary>
        /// 碎片标签
        /// </summary>
        public GameObject fragmentTab = null;
        /// <summary>
        /// 杂货标签
        /// </summary>
        public GameObject otherTab = null;
		/// <summary>
        /// 整理按钮
        /// </summary>
        public GameObject tidyBtn = null;
        /// <summary>
        /// 是否是第一次生成背包物品栏
        /// </summary>
        private bool isOnceAll = false;
        private bool isOnceEuip = false;
        private bool isOnceScorll = false;
        private bool isOnceClip = false;
        private bool isOnceOther = false;
        /// <summary>
        /// 初始化背包物品栏数量
        /// </summary>
        [HideInInspector]
        public int sum = 50;
        public int sumLock = 30;

        List<UIItemView> cacheItemList = new List<UIItemView>();
        List<UIItemView> equipItemList = new List<UIItemView>();
        List<UIItemView> scorllItemList = new List<UIItemView>();
        List<UIItemView> pieceItemList = new List<UIItemView>();
        List<UIItemView> otherItemList = new List<UIItemView>();

        public List<UIItemView> allItemLock = new List<UIItemView>();
        protected override void Awake()
        {
            base.Awake();
            ListenOnClick(allTab, OnClickTab);
            ListenOnClick(equipTab, OnClickTab);
            ListenOnClick(scrollTab, OnClickTab);
            ListenOnClick(fragmentTab, OnClickTab);
            ListenOnClick(otherTab, OnClickTab);
            ListenOnClick(backBtn, OnClickTab);
			ListenOnClick(tidyBtn, OnClickTidyBtn);

            itemPrefeb.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = "";
            itemPrefeb.transform.FindChild("LevelSpr").GetComponent<UISprite>().spriteName = "";
            itemLock.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = "";

            //事件监听
            EventManager.Register<EventPackInfo>(PackInfo);

            //默认选择综合页签
            OnClickTab(allTab);
           

            //向服务器发送数据请求
            ActionParam p = new ActionParam();
            LogicMain.Instance.CallLogicAction(LogicActionDefine.GetItemInfoList, p);
        }
        /// <summary>
        /// 从服务器接收到的背包信息
        /// </summary>
        /// <param name="e"></param>
        void PackInfo(EventPackInfo e)
        {
            //将服务器数据传送到客户端显示层
            //mItemMgr.GetServerInfo(e.type, e.info);

            //显示到背包界面上
            UIWndBackPack backPack = UIManager.Instance.GetWindow<UIWndBackPack>();
          
            if (null == backPack)
            {
                return;
            }
            //List<ItemData> itemData = mItemMgr.getItem(e.type);

            //if (null == itemData)
            //{
            //    return;
            //}
            backPack.SetItemViewList(e.type, e.info);
        }

		/// <summary>
        /// 整理按钮的方法
        /// </summary>
        /// <param name="obj"></param>
        void OnClickTidyBtn(GameObject obj) 
        {
            //对背包物品进行排序
            ActionParam param = new ActionParam();
            LogicMain.Instance.CallLogicAction(LogicActionDefine.ItemResort, param);
        }
		
        /// <summary>
        /// 切换页签
        /// </summary>
        /// <param name="obj"></param>
        void OnClickTab(GameObject go)
        {
            if (go == backBtn)
            {
                Hide();
            }
            else if (go == allTab)
            {
                allTab.GetComponent<UISprite>().spriteName = "yeqiananniuxuanzhong";
                equipTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                scrollTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                fragmentTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                otherTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";

                U3DMod.SetActive(itemGrid.gameObject, true);
                U3DMod.SetActive(equipmentItem.gameObject, false);
                U3DMod.SetActive(scrollItem.gameObject, false);
                U3DMod.SetActive(pieceItem.gameObject, false);
                U3DMod.SetActive(otherItem.gameObject, false);
                if (!isOnceAll)
                {
                    InitBackPack(sum, -1);
                    LoadLockCell(sumLock);
                    isOnceAll = true;
                }
            }
            else if (go == equipTab)
            {
                allTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                equipTab.GetComponent<UISprite>().spriteName = "yeqiananniuxuanzhong";
                scrollTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                fragmentTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                otherTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";

                U3DMod.SetActive(itemGrid.gameObject, false);
                U3DMod.SetActive(equipmentItem.gameObject, true);
                U3DMod.SetActive(scrollItem.gameObject, false);
                U3DMod.SetActive(pieceItem.gameObject, false);
                U3DMod.SetActive(otherItem.gameObject, false);
                if (!isOnceEuip)
                {
                    InitBackPack(sum, 0);
                    isOnceEuip = true;
                }

                //向服务器发送数据请求
                ActionParam p = new ActionParam();
                p["Type"] = (short)1;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.GetBagTabItemInfo, p);
            }
            else if (go == scrollTab)
            {
                allTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                equipTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                scrollTab.GetComponent<UISprite>().spriteName = "yeqiananniuxuanzhong";
                fragmentTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                otherTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";

                U3DMod.SetActive(itemGrid.gameObject, false);
                U3DMod.SetActive(equipmentItem.gameObject, false);
                U3DMod.SetActive(scrollItem.gameObject, true);
                U3DMod.SetActive(pieceItem.gameObject, false);
                U3DMod.SetActive(otherItem.gameObject, false);
                if (!isOnceScorll)
                {
                    InitBackPack(sum, 1);
                    isOnceScorll = true;
                }

                //向服务器发送数据请求
                ActionParam p = new ActionParam();
                p["Type"] = (short)2;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.GetBagTabItemInfo, p);
            }
            else if (go == fragmentTab)
            {
                allTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                equipTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                scrollTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                fragmentTab.GetComponent<UISprite>().spriteName = "yeqiananniuxuanzhong";
                otherTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";

                U3DMod.SetActive(itemGrid.gameObject, false);
                U3DMod.SetActive(equipmentItem.gameObject, false);
                U3DMod.SetActive(scrollItem.gameObject, false);
                U3DMod.SetActive(pieceItem.gameObject, true);
                U3DMod.SetActive(otherItem.gameObject, false);
                if (!isOnceClip)
                {
                    InitBackPack(sum, 2);
                    isOnceClip = true;
                }

                //向服务器发送数据请求
                ActionParam p = new ActionParam();
                p["Type"] = (short)3;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.GetBagTabItemInfo, p);
            }
            else if (go == otherTab)
            {
                allTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                equipTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                scrollTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                fragmentTab.GetComponent<UISprite>().spriteName = "yeqiananniuweixuanzhong";
                otherTab.GetComponent<UISprite>().spriteName = "yeqiananniuxuanzhong";

                U3DMod.SetActive(itemGrid.gameObject, false);
                U3DMod.SetActive(equipmentItem.gameObject, false);
                U3DMod.SetActive(scrollItem.gameObject, false);
                U3DMod.SetActive(pieceItem.gameObject, false);
                U3DMod.SetActive(otherItem.gameObject, true);
                if (!isOnceOther)
                {
                    InitBackPack(sum, 3);
                    isOnceOther = true;
                }

                //向服务器发送数据请求
                ActionParam p = new ActionParam();
                p["Type"] = (short)9;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.GetBagTabItemInfo, p);
            }
        }
        /// <summary>
        /// 拖拽开始
        /// </summary>
        void OndragStart(GameObject obj)
        {
            obj.GetComponent<UISprite>().depth = 4;
        }
        void Ondrop(GameObject obj,GameObject obj1) 
        {
           string temp = obj.GetComponent<UISprite>().spriteName;
           obj.GetComponent<UISprite>().spriteName = obj1.GetComponent<UISprite>().spriteName;
           obj.GetComponent<UISprite>().spriteName = temp;
        }
        /// <summary>
        /// 拖拽结束
        /// </summary>
        void OndragEnd(GameObject obj) 
        {
            
        }
        /// <summary>
        /// 监听图标点击事件
        /// </summary>
        /// <param name="obj"></param>
        void OnClickIcon(GameObject obj) 
        {
            if (obj.GetComponent<UISprite>().spriteName == "") 
            {
                return;
            }

            GameObject iconParent = obj.transform.parent.gameObject;
            UIItemView itemView = iconParent.GetComponent<UIItemView>();

            if (null == itemView)
            {
                return;
            }

            long itemId = itemView.itemId;

            //创建悬浮界面
            UIEquipSuspension itemInfo = UIManager.Instance.GetWindow<UIEquipSuspension>();

            if (null == itemInfo)
            {
                itemInfo = UIManager.Instance.ShowWindow<UIEquipSuspension>();
            }

            //修改界面的显示深度
            itemInfo.GetComponent<UIPanel>().depth = itemGrid.transform.parent.GetComponent<UIPanel> ().depth + 1;

            itemInfo.ShowItemDetailInfo(itemId);

            itemInfo.SetPlane(0);
            
        }
        /// <summary>
        /// 监听未解锁格子的点击事件
        /// </summary>
        public void OnClickLockCell(GameObject obj)
        {
            //创建悬浮界面
            UIWndLock itemInfo = UIManager.Instance.ShowWindow<UIWndLock>();
            //if (null == itemInfo)
            //{
            //    itemInfo = UIManager.Instance.ShowWindow<UIWndLock>();
            //}

            itemInfo.GetComponent<UIPanel>().depth = 3;
        }

        /// <summary>
        /// 初始化背包格子
        /// </summary>
        /// <param name="_sum"></param>
        /// <param name="num"></param>
        public void InitBackPack(int _sum, int num)
        {
            if (num == 0)
            {
                for (int i = 0; i < _sum; ++i)
                {
                    UIItemView item;
                    GameObject go;
                    GameObject icon;

                    go = NGUITools.AddChild(equipmentItem.gameObject, itemPrefeb);
                    U3DMod.SetActive(go, true);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    icon = go.transform.FindChild("Icon").gameObject;
                    item = go.GetComponent<UIItemView>();
                    equipItemList.Add(item);

                    ListenOnClick(icon, OnClickIcon);
                }

                equipmentItem.Reposition();
            }
            else if (num == 1)
            {
                for (int i = 0; i < _sum; ++i)
                {
                    UIItemView item;
                    GameObject go;
                    GameObject icon;

                    go = NGUITools.AddChild(scrollItem.gameObject, itemPrefeb);
                    U3DMod.SetActive(go, true);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    icon = go.transform.FindChild("Icon").gameObject;
                    item = go.GetComponent<UIItemView>();
                    scorllItemList.Add(item);

                    ListenOnClick(icon, OnClickIcon);
                }

                scrollItem.Reposition();
            }
            else if (num == 2)
            {
                for (int i = 0; i < _sum; ++i)
                {
                    UIItemView item;
                    GameObject go;
                    GameObject icon;

                    go = NGUITools.AddChild(pieceItem.gameObject, itemPrefeb);
                    U3DMod.SetActive(go, true);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    icon = go.transform.FindChild("Icon").gameObject;
                    item = go.GetComponent<UIItemView>();
                    pieceItemList.Add(item);

                    ListenOnClick(icon, OnClickIcon);
                }

                pieceItem.Reposition();
            }
            else if (num == 3)
            {
                for (int i = 0; i < _sum; ++i)
                {
                    UIItemView item;
                    GameObject go;
                    GameObject icon;

                    go = NGUITools.AddChild(otherItem.gameObject, itemPrefeb);
                    U3DMod.SetActive(go, true);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    icon = go.transform.FindChild("Icon").gameObject;
                    item = go.GetComponent<UIItemView>();
                    otherItemList.Add(item);

                    ListenOnClick(icon, OnClickIcon);
                }

                otherItem.Reposition();
            }
            else
            {
                for (int i = 0; i < _sum; ++i)
                {
                    UIItemView item;
                    GameObject go;
                    GameObject icon;

                    go = NGUITools.AddChild(itemGrid.gameObject, itemPrefeb);
                    U3DMod.SetActive(go, true);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    icon = go.transform.FindChild("Icon").gameObject;
                    item = go.GetComponent<UIItemView>();
                    cacheItemList.Add(item);

                    ListenOnClick(icon, OnClickIcon);
                }

                itemGrid.Reposition();
            }
        }

        /// <summary>
        /// 初始化未解锁的格子
        /// </summary>
        /// <param name="_sum"></param>
        public void LoadLockCell(int _sum)
        {
            for (int i = 0; i < _sum; ++i)
            {
                UIItemView item;
                GameObject go;
                GameObject icon;

                go = NGUITools.AddChild(itemGrid.gameObject, itemLock);
                U3DMod.SetActive(go, true);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                icon = go.transform.FindChild("Icon").gameObject;
                item = go.GetComponent<UIItemView>();
                allItemLock.Add(item);

                ListenOnClick(item.gameObject, OnClickLockCell);
            }
            itemGrid.Reposition();
        }
        public void OpenLockCell()
        {
            for (int i = 0; i < 5; i++)
            {
                allItemLock[i].GetComponent<UISprite>().spriteName = "beibaozhengchang";
            }
        }

        /// <summary>
        ///  显示
        /// </summary>
        /// <param name="_datas"></param>
        public void SetItemViewList(short type, List<Dictionary<string, string>> _datas)
        {
            if (1 == type)
            {
                foreach (UIItemView view in equipItemList)
                {
                    view.Clear();
                }
            }
            else if (2 == type)
            {
                foreach (UIItemView view in scorllItemList)
                {
                    view.Clear();
                }
            }
            else if (3 == type)
            {
                foreach (UIItemView view in pieceItemList)
                {
                    view.Clear();
                }
            }
            else if (9 == type)
            {
                foreach (UIItemView view in otherItemList)
                {
                    view.Clear();
                }
            }

            foreach (Dictionary<string, string> data in _datas)
            {
                int pos = short.Parse(data["Location"]) - 1;
                long itemId = long.Parse(data["ItemID"]);
                int itemNo = int.Parse(data["ItemNo"]);
                short num = short.Parse(data["Num"]);
                short quality = short.Parse(data["Quality"]);
                bool isEquiped = (data["IsEquip"] == "true" ? true : false);

                //从配置文件中取到道具的spriteName
                Dictionary<string, string> req = new Dictionary<string, string>();
                req.Add("ID", itemNo.ToString());
                List<Dictionary<string, string>> res = DataManager.Instance.mIconData.GetRowDataByMultiColValue(req);

                if (null == res || 0 == res.Count)
                {
                    continue;
                }

                string spriteName = res[0]["SpriteName"];

                if (pos >= sum)
                {
                    continue;
                }

                if (0 == type)
                {
                    UIItemView view = cacheItemList[pos];
                    view.init(itemId, num, quality, spriteName, isEquiped);
                }
                else if (1 == type || 101 == type)
                {
                    UIItemView view = equipItemList[pos];
                    view.init(itemId, num, quality, spriteName, isEquiped);
                }
                else if (2 == type || 102 == type)
                {
                    UIItemView view = scorllItemList[pos];
                    view.init(itemId, num, quality, spriteName, isEquiped);
                }
                else if (3 == type || 103 == type)
                {
                    UIItemView view = pieceItemList[pos];
                    view.init(itemId, num, quality, spriteName, isEquiped);
                }
                else if (9 == type || 109 == type)
                {
                    UIItemView view = otherItemList[pos];
                    view.init(itemId, num, quality, spriteName, isEquiped);
                }
            }
            
        }
       
        /// <summary>
        /// 删除
        /// </summary>
        void OnDestroy()
        {
            //取消事件监听
            EventManager.Remove<EventPackInfo>(PackInfo);
        }
      
    }
}

