using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class MyItemBag
    {
        protected class TabItemInfo
        {
            public long itemId;
            public short useLevel;
            public short itemQuality;
            public int itemNo;
            public short num;
            public short location;
            public bool isEquiped;
        }

        class ItemUseLevelComparer : IComparer<TabItemInfo>
        {
            public int Compare(TabItemInfo nodeA, TabItemInfo nodeB)
            {
                return nodeB.useLevel - nodeA.useLevel;
            }
        }

        class ItemQualityComparer : IComparer<TabItemInfo>
        {
            public int Compare(TabItemInfo nodeA, TabItemInfo nodeB)
            {
                return nodeB.itemQuality - nodeA.itemQuality;
            }
        }

        class ItemNoComparer : IComparer<TabItemInfo>
        {
            public int Compare(TabItemInfo nodeA, TabItemInfo nodeB)
            {
                if (nodeA.itemNo != nodeB.itemNo)
                {
                    return nodeA.itemNo - nodeB.itemNo;
                }
                else
                {
                    return (int)(nodeA.itemId - nodeB.itemId);
                }
            }
        }

        public MyPlayerEquip playerEquip;

        bool isGetServerBagItemInfo = false; //是否已经获取了服务器的背包物品信息
        bool isGetServerBagEquipInfo = false; //是否已经获取了服务器的背包装备信息

        protected List<EquipInfo> playerBagEquipInfoList = null; //表示角色的背包装备信息
        protected List<ItemInfo> playerBagItemInfoList = null;  //表示角色的背包物品信息

        protected List<TabItemInfo> equipTabInfoList = null; // 装备分类物品信息缓存
        protected List<TabItemInfo> scrollTabInfoList = null; //卷轴分类物品信息缓存
        protected List<TabItemInfo> pieceTabInfoList = null; //碎片分类物品信息缓存
        protected List<TabItemInfo> otherTabInfoList = null; //其他分类物品信息缓存

        ItemUseLevelComparer useLevelComparer = new ItemUseLevelComparer();
        ItemQualityComparer qualityComparer = new ItemQualityComparer();
        ItemNoComparer itemNoComparer = new ItemNoComparer();

        public bool IsGetServerBagItemInfo
        {
            get
            {
                return isGetServerBagItemInfo;
            }
        }

        public bool IsGetServerBagEquipInfo
        {
            get
            {
                return isGetServerBagEquipInfo;
            }
        }

        public void Reset ()
        {
            isGetServerBagItemInfo = false;
            isGetServerBagEquipInfo = false;

            playerBagEquipInfoList.Clear();
            playerBagEquipInfoList = null;

            playerBagItemInfoList.Clear();
            playerBagItemInfoList = null;

            equipTabInfoList.Clear();
            equipTabInfoList = null;

            scrollTabInfoList.Clear();
            scrollTabInfoList = null;

            pieceTabInfoList.Clear();
            pieceTabInfoList = null;

            otherTabInfoList.Clear();
            otherTabInfoList = null;
        }

        /// <summary>
        /// 填充玩家全部背包物品信息
        /// </summary>
        /// <param name="itemInfoList"></param>
        /// <param name="equipInfoList"></param>
        public void OnRcvPlayerBagItemInfo(List<ItemInfo> itemInfoList, List<EquipInfo> equipInfoList)
        {
            if (null == itemInfoList || null == equipInfoList)
            {
                return;
            }

            if (!isGetServerBagItemInfo)
            {
                isGetServerBagItemInfo = true;
            }

            if (!IsGetServerBagEquipInfo)
            {
                isGetServerBagEquipInfo = true;
            }

            playerBagEquipInfoList = equipInfoList;
            playerBagItemInfoList = itemInfoList;
        }

        /// <summary>
        /// 当只收到玩家背包装备信息时调用
        /// </summary>
        public void OnRcvPlayerOnlyBagEquipInfo(List<EquipInfo> equipInfoList)
        {
            if (null == equipInfoList)
            {
                return;
            }

            if (!IsGetServerBagEquipInfo)
            {
                isGetServerBagEquipInfo = true;
            }

            playerBagEquipInfoList = equipInfoList;
        }

        /// <summary>
        /// 增加玩家背包物品信息
        /// </summary>
        /// <param name="itemInfoList"></param>
        public void AddPlayerBagItemInfo (List<ItemInfo> itemInfoList, out ActionParam param)
        {
            param = new ActionParam();

            if (!isGetServerBagItemInfo)
            {
                return;
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();
            foreach (ItemInfo item in itemInfoList)
            {
                bool isFindItem = false;

                foreach (ItemInfo playerItem in playerBagItemInfoList)
                {
                    if (item.ID == playerItem.ID)
                    {
                        playerItem.HeapNum = item.HeapNum;
                        isFindItem = true;

                        //创建传给界面的数据
                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        paramDic.Add("ItemID", playerItem.ID.ToString());
                        paramDic.Add("ItemNo", playerItem.ItemNo.ToString());
                        paramDic.Add("Num", playerItem.HeapNum.ToString());
                        paramDic.Add("Quality", playerItem.ItemQuality.ToString());
                        paramDic.Add("Location", playerItem.StorageLocationNo.ToString());
                        paramDic.Add("IsEquip", "false");

                        paramList.Add(paramDic);

                        break;
                    }
                }

                if (!isFindItem)
                {
                    playerBagItemInfoList.Add(item);

                    //创建传给界面的数据
                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", item.ID.ToString());
                    paramDic.Add("ItemNo", item.ItemNo.ToString());
                    paramDic.Add("Num", item.HeapNum.ToString());
                    paramDic.Add("Quality", item.ItemQuality.ToString());
                    paramDic.Add("Location", item.StorageLocationNo.ToString());
                    paramDic.Add("IsEquip", "false");

                    paramList.Add(paramDic);
                }
            }

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;
        }

        /// <summary>
        /// 增加玩家背包装备信息
        /// </summary>
        /// <param name="equipInfoList"></param>
        public void AddPlayerBagEquipInfo (List<EquipInfo> equipInfoList, out ActionParam param)
        {
            param = new ActionParam();

            if (!isGetServerBagEquipInfo)
            {
                return;
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();
            foreach (EquipInfo equip in equipInfoList)
            {
                playerBagEquipInfoList.Add(equip);

                //创建传给界面的数据
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", equip.ID.ToString());
                paramDic.Add("ItemNo", equip.EquipNo.ToString());
                paramDic.Add("Num", "1");
                paramDic.Add("Quality", equip.EquipQuality.ToString());
                paramDic.Add("Location", equip.StorageLocationNo.ToString());
                paramDic.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                paramList.Add(paramDic);
            }

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;
        }

        /// <summary>
        /// 减少玩家背包物品信息
        /// </summary>
        public bool ReducePlayerBagItemInfo(long itemId, short itemNum, out ActionParam param)
        {
            param = new ActionParam();

            if (!isGetServerBagItemInfo)
            {
                return true;
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            bool isZero = false;
            ItemInfo itemInfo = null;

            foreach (ItemInfo item in playerBagItemInfoList)
            {
                if (itemId == item.ID)
                {
                    if (itemNum <= 0)
                    {
                        isZero = true;
                    }
                    
                    item.HeapNum = itemNum;

                    itemInfo = item;

                    break;
                }
            }

            if ( null != itemInfo)
            {
                if (isZero)
                {
                    playerBagItemInfoList.Remove(itemInfo);
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", itemInfo.ID.ToString());
                paramDic.Add("ItemNo", itemInfo.ItemNo.ToString());
                paramDic.Add("Num", itemInfo.HeapNum.ToString());
                paramDic.Add("Quality", itemInfo.ItemQuality.ToString());
                paramDic.Add("Location", itemInfo.StorageLocationNo.ToString());
                paramDic.Add("IsEquip", "false");

                paramList.Add(paramDic);

                param["Type"] = (short)0;
                param["PackInfo"] = paramList;

                return true;
            }

            if (0 != itemNum)
            {
                return false;
            }

            EquipInfo equipInfo = null;
            foreach (EquipInfo equip in playerBagEquipInfoList)
            {
                if (itemId == equip.ID)
                {
                    equipInfo = equip;
                    break;
                }
            }

            if (null == equipInfo)
            {
                return false;
            }

            playerBagEquipInfoList.Remove(equipInfo);

            Dictionary<string, string> equipParamDic = new Dictionary<string, string>();
            equipParamDic.Add("ItemID", equipInfo.ID.ToString());
            equipParamDic.Add("ItemNo", equipInfo.EquipNo.ToString());
            equipParamDic.Add("Num", "0");
            equipParamDic.Add("Quality", equipInfo.EquipQuality.ToString());
            equipParamDic.Add("Location", equipInfo.StorageLocationNo.ToString());
            equipParamDic.Add("IsEquip", equipInfo.IsBelongequipPlan1.ToString ().ToLower ());

            paramList.Add(equipParamDic);

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;

            return true;
            
        }
        public bool ChangeBagEquipInfo (List<EquipInfo> equipInfoList)
        {
            if (!isGetServerBagEquipInfo)
            {
                return true;
            }

            ActionParam param = new ActionParam();
            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (EquipInfo changedEquip in equipInfoList)
            {
                bool isFindEquip = false;

                for (int i = 0; i < playerBagEquipInfoList.Count; ++i )
                {
                    if (changedEquip.ID == playerBagEquipInfoList[i].ID)
                    {
                        isFindEquip = true;
                        playerBagEquipInfoList[i] = changedEquip;
                        break;
                    }
                }

                if (!isFindEquip)
                {
                    return false;
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", changedEquip.ID.ToString());
                paramDic.Add("ItemNo", changedEquip.EquipNo.ToString());
                paramDic.Add("Num", "1");
                paramDic.Add("Quality", changedEquip.EquipQuality.ToString());
                paramDic.Add("Location", changedEquip.StorageLocationNo.ToString());
                paramDic.Add("IsEquip", changedEquip.IsBelongequipPlan1.ToString().ToLower());

                paramList.Add(paramDic);
            }

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;

            //调用UnityAction将结果返回给界面
            Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, param);

            if (null != equipTabInfoList)
            {
                ActionParam tabParam = new ActionParam();
                List<Dictionary<string, string>> tabParamList = new List<Dictionary<string, string>>();

                foreach (EquipInfo changedEquip in equipInfoList)
                {
                    bool isFindTab = false;

                    foreach (TabItemInfo tabInfo in equipTabInfoList)
                    {
                        if (changedEquip.ID == tabInfo.itemId)
                        {
                            isFindTab = true;
                            tabInfo.isEquiped = changedEquip.IsBelongequipPlan1;
                            break;
                        }
                    }

                    if (!isFindTab)
                    {
                        return false;
                    }

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", changedEquip.ID.ToString());
                    paramDic.Add("ItemNo", changedEquip.EquipNo.ToString());
                    paramDic.Add("Num", "1");
                    paramDic.Add("Quality", changedEquip.EquipQuality.ToString());
                    paramDic.Add("Location", changedEquip.StorageLocationNo.ToString());
                    paramDic.Add("IsEquip", changedEquip.IsBelongequipPlan1.ToString().ToLower());

                    tabParamList.Add(paramDic);
                }

                tabParam["Type"] = (short)101;
                tabParam["PackInfo"] = tabParamList;

                //调用UnityAction将结果返回给界面
                Local.Instance.CallUnityAction(UnityActionDefine.BackPackInfo, tabParam);
            }

            return true;
        }

        public bool OnRcvPlayerItemResortInfo (List<ItemLocationChange> itemLocationChangeList, out ActionParam param)
        {
            param = new ActionParam();
            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (ItemLocationChange locationChange in itemLocationChangeList)
            {
               bool isOldLocationHaveNewItem = false;
               ItemInfo itemInfo = null;

               foreach (ItemInfo item in playerBagItemInfoList)
               {
                   if (locationChange.ItemId == item.ID)
                   {
                       itemInfo = item;

                       foreach (ItemLocationChange otherChange in itemLocationChangeList)
                       {
                           if (otherChange.newStorageLocationNo == item.StorageLocationNo)
                           {
                               isOldLocationHaveNewItem = true;
                               break;
                           }
                       }

                       break;
                   }
               }

               if (null != itemInfo)
               {
                   if (0 != locationChange.Num)
                   {

                       if (isOldLocationHaveNewItem)
                       {
                           Dictionary<string, string> paramDic = new Dictionary<string, string>();

                           paramDic.Add("ItemID", itemInfo.ID.ToString());
                           paramDic.Add("ItemNo", itemInfo.ItemNo.ToString());
                           paramDic.Add("Num", locationChange.Num.ToString());
                           paramDic.Add("Quality", itemInfo.ItemQuality.ToString());
                           paramDic.Add("Location", locationChange.newStorageLocationNo.ToString());
                           paramDic.Add("IsEquip", "false");

                           paramList.Add(paramDic);
                       }
                       else
                       {
                           //如果原来的个子没有数据，需要将里面的显示删除
                           Dictionary<string, string> paramDicDel = new Dictionary<string, string>();
                           paramDicDel.Add("ItemID", itemInfo.ID.ToString());
                           paramDicDel.Add("ItemNo", itemInfo.ItemNo.ToString());
                           paramDicDel.Add("Num", "0");
                           paramDicDel.Add("Quality", itemInfo.ItemQuality.ToString());
                           paramDicDel.Add("Location", itemInfo.StorageLocationNo.ToString());
                           paramDicDel.Add("IsEquip", "false");

                           paramList.Add(paramDicDel);

                           Dictionary<string, string> paramDicNew = new Dictionary<string, string>();
                           paramDicNew.Add("ItemID", itemInfo.ID.ToString());
                           paramDicNew.Add("ItemNo", itemInfo.ItemNo.ToString());
                           paramDicNew.Add("Num", locationChange.Num.ToString());
                           paramDicNew.Add("Quality", itemInfo.ItemQuality.ToString());
                           paramDicNew.Add("Location", locationChange.newStorageLocationNo.ToString());
                           paramDicNew.Add("IsEquip", "false");

                           paramList.Add(paramDicNew);
                       }

                       //改变物品在背包中的位置
                       itemInfo.StorageLocationNo = locationChange.newStorageLocationNo;

                       short oldLocation = itemInfo.HeapNum;

                       //改变物品的数量
                       itemInfo.HeapNum = locationChange.Num;

                       //当物品数量改变时，更改分页签缓存
                       if (oldLocation != locationChange.Num)
                       {
                           UpdateTabItemInfo(itemInfo);
                       }
                   }
                   else
                   {
                       if (!isOldLocationHaveNewItem)
                       {
                           //如果原来的个子没有数据，需要将里面的显示删除
                           Dictionary<string, string> paramDicDel = new Dictionary<string, string>();
                           paramDicDel.Add("ItemID", itemInfo.ID.ToString());
                           paramDicDel.Add("ItemNo", itemInfo.ItemNo.ToString());
                           paramDicDel.Add("Num", "0");
                           paramDicDel.Add("Quality", itemInfo.ItemQuality.ToString());
                           paramDicDel.Add("Location", itemInfo.StorageLocationNo.ToString());
                           paramDicDel.Add("IsEquip", "false");

                           paramList.Add(paramDicDel);
                       }

                       //当物品数量改变时，更改分页签缓存
                       if (itemInfo.HeapNum != 0)
                       {
                           itemInfo.HeapNum = 0;
                           UpdateTabItemInfo(itemInfo);
                       }

                       playerBagItemInfoList.Remove(itemInfo);
                   }


                   continue;
               }

               bool isFindItem = false;
               foreach (EquipInfo equip in playerBagEquipInfoList)
               {
                   if (locationChange.ItemId == equip.ID)
                   {
                       isFindItem = true;

                       foreach (ItemLocationChange otherChange in itemLocationChangeList)
                       {
                           if (otherChange.newStorageLocationNo == equip.StorageLocationNo)
                           {
                               isOldLocationHaveNewItem = true;
                               break;
                           }
                       }

                       if (isOldLocationHaveNewItem)
                       {
                           //生成相应装备的ActionParam
                           Dictionary<string, string> paramDic = new Dictionary<string, string>();
                           paramDic.Add("ItemID", equip.ID.ToString());
                           paramDic.Add("ItemNo", equip.EquipNo.ToString());
                           paramDic.Add("Num", "1");
                           paramDic.Add("Quality", equip.EquipQuality.ToString ());
                           paramDic.Add("Location", locationChange.newStorageLocationNo.ToString());
                           paramDic.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                           paramList.Add(paramDic);
                       }
                       else
                       {
                           //如果原来的个子没有数据，需要将里面的显示删除
                           Dictionary<string, string> paramDicDel = new Dictionary<string, string>();
                           paramDicDel.Add("ItemID", equip.ID.ToString());
                           paramDicDel.Add("ItemNo", equip.EquipNo.ToString());
                           paramDicDel.Add("Num", "0");
                           paramDicDel.Add("Quality", equip.EquipQuality.ToString());
                           paramDicDel.Add("Location", equip.StorageLocationNo.ToString());
                           paramDicDel.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                           paramList.Add(paramDicDel);

                           Dictionary<string, string> paramDicNew = new Dictionary<string, string>();
                           paramDicNew.Add("ItemID", equip.ID.ToString());
                           paramDicNew.Add("ItemNo", equip.EquipNo.ToString());
                           paramDicNew.Add("Num", locationChange.Num.ToString());
                           paramDicNew.Add("Quality", equip.EquipQuality.ToString());
                           paramDicNew.Add("Location", locationChange.newStorageLocationNo.ToString());
                           paramDicNew.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                           paramList.Add(paramDicNew);
                       }

                       //改变装备在背包中的位置
                       equip.StorageLocationNo = locationChange.newStorageLocationNo;

                       break;
                   }
               }

               if (!isFindItem)
               {
                   return false;
               }
            }

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;

            return true;
        }

        /// <summary>
        /// 得到玩家背包信息ActionParam
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetPlayerBagInfoParam(out ActionParam param)
        {
            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (ItemInfo item in playerBagItemInfoList)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", item.ID.ToString ());
                paramDic.Add("ItemNo", item.ItemNo.ToString ());
                paramDic.Add("Num", item.HeapNum.ToString ());
                paramDic.Add("Quality", item.ItemQuality.ToString ());
                paramDic.Add("Location", item.StorageLocationNo.ToString ());
                paramDic.Add("IsEquip", "false");

                paramList.Add(paramDic);
            }

            foreach (EquipInfo equip in playerBagEquipInfoList)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", equip.ID.ToString ());
                paramDic.Add("ItemNo", equip.EquipNo.ToString());
                paramDic.Add("Num", "1");
                paramDic.Add("Quality", equip.EquipQuality.ToString ());
                paramDic.Add("Location", equip.StorageLocationNo.ToString ());
                paramDic.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                paramList.Add(paramDic);
            }

            param = new ActionParam();

            if (0 == paramList.Count)
            {
                return false;
            }

            param["Type"] = (short)0;
            param["PackInfo"] = paramList;

            return true;
        }

        /// <summary>
        /// 获取某一类别物品信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetBagTabItemInfo (short type, out ActionParam param)
        {
            param = new ActionParam();

            
            if (1 == type)
            {
                if (null == equipTabInfoList)
                {
                    equipTabInfoList = new List<TabItemInfo>();

                    foreach (EquipInfo equip in playerBagEquipInfoList)
                    {
                        TabItemInfo tabItemInfo = new TabItemInfo();
                        tabItemInfo.itemId = equip.ID;
                        tabItemInfo.itemNo = equip.EquipNo;
                        tabItemInfo.itemQuality = equip.EquipQuality;
                        tabItemInfo.num = 1;
                        tabItemInfo.isEquiped = equip.IsBelongequipPlan1;

                        equipTabInfoList.Add(tabItemInfo);
                    }
                }

                equipTabInfoList.Sort(useLevelComparer);
                equipTabInfoList.Sort(qualityComparer);
                equipTabInfoList.Sort(itemNoComparer);

                List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

                short itemLocation = 1;
                foreach (TabItemInfo tabInfo in equipTabInfoList)
                {
                    tabInfo.location = itemLocation;

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", tabInfo.itemId.ToString());
                    paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                    paramDic.Add("Num", tabInfo.num.ToString());
                    paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                    paramDic.Add("Location", itemLocation.ToString());
                    paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                    paramList.Add(paramDic);

                    ++itemLocation;
                }

                param["Type"] = (short)1;
                param["PackInfo"] = paramList;
                
            }
            else if (2 == type)
            {
                if (null == scrollTabInfoList)
                {
                    scrollTabInfoList = new List<TabItemInfo>();

                    foreach (ItemInfo item in playerBagItemInfoList)
                    {
                        if (2 == item.ItemType)
                        {
                            TabItemInfo tabItemInfo = new TabItemInfo();
                            tabItemInfo.itemId = item.ID;
                            tabItemInfo.itemNo = item.ItemNo;
                            tabItemInfo.itemQuality = item.ItemQuality;
                            tabItemInfo.num = item.HeapNum;
                            tabItemInfo.isEquiped = false;

                            scrollTabInfoList.Add(tabItemInfo);
                        }
                    }
                }

                scrollTabInfoList.Sort(useLevelComparer);
                scrollTabInfoList.Sort(qualityComparer);
                scrollTabInfoList.Sort(itemNoComparer);

                List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

                short itemLocation = 1;
                foreach (TabItemInfo tabInfo in scrollTabInfoList)
                {
                    tabInfo.location = itemLocation;

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", tabInfo.itemId.ToString());
                    paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                    paramDic.Add("Num", tabInfo.num.ToString());
                    paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                    paramDic.Add("Location", itemLocation.ToString());
                    paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                    paramList.Add(paramDic);

                    ++itemLocation;
                }

                param["Type"] = type;
                param["PackInfo"] = paramList;
                
            }
            else if (3 == type)
            {
                if (null == pieceTabInfoList)
                {
                    pieceTabInfoList = new List<TabItemInfo>();

                    foreach (ItemInfo item in playerBagItemInfoList)
                    {
                        if (3 == item.ItemType)
                        {
                            TabItemInfo tabItemInfo = new TabItemInfo();
                            tabItemInfo.itemId = item.ID;
                            tabItemInfo.itemNo = item.ItemNo;
                            tabItemInfo.itemQuality = item.ItemQuality;
                            tabItemInfo.num = item.HeapNum;
                            tabItemInfo.isEquiped = false;

                            pieceTabInfoList.Add(tabItemInfo);
                        }
                    }
                }

                pieceTabInfoList.Sort(useLevelComparer);
                pieceTabInfoList.Sort(qualityComparer);
                pieceTabInfoList.Sort(itemNoComparer);

                List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

                short itemLocation = 1;
                foreach (TabItemInfo tabInfo in pieceTabInfoList)
                {
                    tabInfo.location = itemLocation;

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", tabInfo.itemId.ToString());
                    paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                    paramDic.Add("Num", tabInfo.num.ToString());
                    paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                    paramDic.Add("Location", itemLocation.ToString());
                    paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                    paramList.Add(paramDic);

                    ++itemLocation;
                }

                param["Type"] = type;
                param["PackInfo"] = paramList;
                
            }
            else if (9 == type)
            {
                if (null == otherTabInfoList)
                {
                    otherTabInfoList = new List<TabItemInfo>();

                    foreach (ItemInfo item in playerBagItemInfoList)
                    {
                        if (9 == item.ItemType)
                        {
                            TabItemInfo tabItemInfo = new TabItemInfo();
                            tabItemInfo.itemId = item.ID;
                            tabItemInfo.itemNo = item.ItemNo;
                            tabItemInfo.itemQuality = item.ItemQuality;
                            tabItemInfo.num = item.HeapNum;
                            tabItemInfo.isEquiped = false;

                            otherTabInfoList.Add(tabItemInfo);
                        }
                    }
                }

                otherTabInfoList.Sort(useLevelComparer);
                otherTabInfoList.Sort(qualityComparer);
                otherTabInfoList.Sort(itemNoComparer);

                List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

                short itemLocation = 1;
                foreach (TabItemInfo tabInfo in otherTabInfoList)
                {
                    tabInfo.location = itemLocation;

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", tabInfo.itemId.ToString());
                    paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                    paramDic.Add("Num", tabInfo.num.ToString());
                    paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                    paramDic.Add("Location", itemLocation.ToString());
                    paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                    paramList.Add(paramDic);

                    ++itemLocation;
                }

                param["Type"] = type;
                param["PackInfo"] = paramList;
            }

            return true;
        }

        /// <summary>
        /// 更新背包分类物品信息
        /// </summary>
        /// <returns></returns>
        public bool AddBagTabItemInfo (List<ItemInfo> itemInfoList, out List<ActionParam> paramList)
        {
            paramList = new List<ActionParam>();

            List<Dictionary<string, string>> scrollParamList = null;
            List<Dictionary<string, string>> pieceParamList = null;
            List<Dictionary<string, string>> otherParamList = null;

            foreach (ItemInfo item in itemInfoList)
            {
                if (2 == item.ItemType)
                {
                    if (null == scrollTabInfoList)
                    {
                        continue;
                    }

                    if (null == scrollParamList)
                    {
                        scrollParamList = new List<Dictionary<string, string>>();
                    }

                    bool isFindTab = false;
                    short location = 0;
                    foreach (TabItemInfo tab in scrollTabInfoList)
                    {
                        if (item.ID == tab.itemId)
                        {
                            isFindTab = true;
                            tab.num = item.HeapNum;

                            location = tab.location;
                            break;
                        }
                    }

                    if (!isFindTab)
                    {
                        TabItemInfo tabInfo = new TabItemInfo();
                        tabInfo.itemId = item.ID;
                        tabInfo.itemNo = item.ItemNo;
                        tabInfo.itemQuality = item.ItemQuality;
                        tabInfo.num = item.HeapNum;
                        tabInfo.location = (short)(scrollTabInfoList[scrollTabInfoList.Count - 1].location + 1);
                        tabInfo.isEquiped = false;

                        location = tabInfo.location;

                        scrollTabInfoList.Add(tabInfo);
                    }

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", item.ID.ToString());
                    paramDic.Add("ItemNo", item.ItemNo.ToString());
                    paramDic.Add("Num", item.HeapNum.ToString());
                    paramDic.Add("Quality", item.ItemQuality.ToString());
                    paramDic.Add("Location", location.ToString());
                    paramDic.Add("IsEquip", "false");

                    scrollParamList.Add(paramDic);
                }
                else if (3 == item.ItemType)
                {
                    if (null == pieceTabInfoList)
                    {
                        continue;
                    }

                    if (null == pieceParamList)
                    {
                        pieceParamList = new List<Dictionary<string, string>>();
                    }

                    bool isFindTab = false;
                    short location = 0;
                    foreach (TabItemInfo tab in pieceTabInfoList)
                    {
                        if (item.ID == tab.itemId)
                        {
                            isFindTab = true;
                            tab.num = item.HeapNum;

                            location = tab.location;
                            break;
                        }
                    }

                    if (!isFindTab)
                    {
                        TabItemInfo tabInfo = new TabItemInfo();
                        tabInfo.itemId = item.ID;
                        tabInfo.itemNo = item.ItemNo;
                        tabInfo.itemQuality = item.ItemQuality;
                        tabInfo.num = item.HeapNum;
                        tabInfo.location = (short)(pieceTabInfoList[pieceTabInfoList.Count - 1].location + 1);
                        tabInfo.isEquiped = false;

                        location = tabInfo.location;

                        pieceTabInfoList.Add(tabInfo);
                    }

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", item.ID.ToString());
                    paramDic.Add("ItemNo", item.ItemNo.ToString());
                    paramDic.Add("Num", item.HeapNum.ToString());
                    paramDic.Add("Quality", item.ItemQuality.ToString());
                    paramDic.Add("Location", location.ToString());
                    paramDic.Add("IsEquip", "false");

                    pieceParamList.Add(paramDic);

                }
                else if (9 == item.ItemType)
                {
                    if (null == otherTabInfoList)
                    {
                        continue;
                    }

                    if (null == otherParamList)
                    {
                        otherParamList = new List<Dictionary<string, string>>();
                    }

                    bool isFindTab = false;
                    short location = 0;
                    foreach (TabItemInfo tab in otherTabInfoList)
                    {
                        if (item.ID == tab.itemId)
                        {
                            isFindTab = true;
                            tab.num = item.HeapNum;

                            location = tab.location;
                            break;
                        }
                    }

                    if (!isFindTab)
                    {
                        TabItemInfo tabInfo = new TabItemInfo();
                        tabInfo.itemId = item.ID;
                        tabInfo.itemNo = item.ItemNo;
                        tabInfo.itemQuality = item.ItemQuality;
                        tabInfo.num = item.HeapNum;
                        tabInfo.location = (short)(otherTabInfoList[otherTabInfoList.Count - 1].location + 1);
                        tabInfo.isEquiped = false;

                        location = tabInfo.location;

                        otherTabInfoList.Add(tabInfo);
                    }

                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("ItemID", item.ID.ToString());
                    paramDic.Add("ItemNo", item.ItemNo.ToString());
                    paramDic.Add("Num", item.HeapNum.ToString());
                    paramDic.Add("Quality", item.ItemQuality.ToString());
                    paramDic.Add("Location", location.ToString());
                    paramDic.Add("IsEquip", "false");

                    otherParamList.Add(paramDic);
                }
            }

            if (null != scrollParamList)
            {
                ActionParam scrollActionParam = new ActionParam();
                scrollActionParam["Type"] = (short)102;
                scrollActionParam["PackInfo"] = scrollParamList;

                paramList.Add(scrollActionParam);
            }

            if (null != pieceParamList)
            {
                ActionParam pieceActionParam = new ActionParam();
                pieceActionParam["Type"] = (short)103;
                pieceActionParam["PackInfo"] = pieceParamList;

                paramList.Add(pieceActionParam);
            }

            if (null != otherParamList)
            {
                ActionParam otherActionParam = new ActionParam();
                otherActionParam["Type"] = (short)109;
                otherActionParam["PackInfo"] = otherParamList;

                paramList.Add(otherActionParam);
            }

            if (null == scrollParamList && null == pieceParamList && null == otherParamList)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新背包分类装备信息
        /// </summary>
        /// <returns></returns>
        public bool AddBagTabEquipInfo(List<EquipInfo> equipInfoList, out ActionParam param)
        {
            param = new ActionParam();

            if (null == equipTabInfoList)
            {
                return false;
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (EquipInfo equip in equipInfoList)
            {
                short location = 0;
               
                TabItemInfo tabInfo = new TabItemInfo();
                tabInfo.itemId = equip.ID;
                tabInfo.itemNo = equip.EquipNo;
                tabInfo.itemQuality = equip.EquipQuality;
                tabInfo.num = 1;
                tabInfo.location = (short)(equipTabInfoList[equipTabInfoList.Count - 1].location + 1);
                tabInfo.isEquiped = equip.IsBelongequipPlan1;

                location = tabInfo.location;

                equipTabInfoList.Add(tabInfo);
                

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", equip.ID.ToString());
                paramDic.Add("ItemNo", equip.EquipNo.ToString());
                paramDic.Add("Num", "1");
                paramDic.Add("Quality", equip.EquipQuality.ToString());
                paramDic.Add("Location", location.ToString());
                paramDic.Add("IsEquip", equip.IsBelongequipPlan1.ToString ().ToLower ());

                paramList.Add(paramDic);
            }

            param["Type"] = (short)101;
            param["PackInfo"] = paramList;

            return true;
        }

        /// <summary>
        /// 更新背包分类信息
        /// </summary>
        /// <returns></returns>
        public bool ReduceBagTabItemInfo (long itemId, short num, out ActionParam param)
        {
            param = new ActionParam();

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            short type = 0;

            bool isFindItem = false;

            foreach (ItemInfo item in playerBagItemInfoList)
            {
                if (itemId == item.ID)
                {
                    isFindItem = true;

                    type = item.ItemType;

                    break;
                }
            }

            if (!isFindItem)
            {
                foreach (EquipInfo equip in playerBagEquipInfoList)
                {
                    if (itemId == equip.ID)
                    {
                        isFindItem = true;

                        type = (short)1;

                        break;
                    }
                }
            }

            if (!isFindItem)
            {
                return false;
            }

            if (1 == type)
            {
                if (null == equipTabInfoList)
                {
                    return false;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in equipTabInfoList)
                {
                    if (itemId == tab.itemId)
                    {
                        tab.num = num;

                        tabInfo = tab;
                        break;
                    }
                }

                if (null == tabInfo)
                {
                    return false;
                }

                if (0 == tabInfo.num)
                {
                    equipTabInfoList.Remove(tabInfo);
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", tabInfo.itemId.ToString());
                paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                paramDic.Add("Num", "0");
                paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                paramDic.Add("Location", tabInfo.location.ToString());
                paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                paramList.Add(paramDic);

                param["Type"] = (short)101;
                param["PackInfo"] = paramList;

                return true;
            }
            else if (2 == type)
            {
                if (null == scrollTabInfoList)
                {
                    return false;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in scrollTabInfoList)
                {
                    if (itemId == tab.itemId)
                    {
                        tab.num = num;

                        tabInfo = tab;
                        break;
                    }
                }

                if (null == tabInfo)
                {
                    return false;
                }

                if (0 == tabInfo.num)
                {
                    scrollTabInfoList.Remove(tabInfo);
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", tabInfo.itemId.ToString());
                paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                paramDic.Add("Num", tabInfo.num.ToString());
                paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                paramDic.Add("Location", tabInfo.location.ToString());
                paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                paramList.Add(paramDic);

                param["Type"] = (short)102;
                param["PackInfo"] = paramList;

                return true;
            }
            else if (3 == type)
            {
                if (null == pieceTabInfoList)
                {
                    return false;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in pieceTabInfoList)
                {
                    if (itemId == tab.itemId)
                    {
                        tab.num = num;

                        tabInfo = tab;
                        break;
                    }
                }

                if (null == tabInfo)
                {
                    return false;
                }

                if (0 == tabInfo.num)
                {
                    pieceTabInfoList.Remove(tabInfo);
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", tabInfo.itemId.ToString());
                paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                paramDic.Add("Num", tabInfo.num.ToString());
                paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                paramDic.Add("Location", tabInfo.location.ToString());
                paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                paramList.Add(paramDic);

                param["Type"] = (short)103;
                param["PackInfo"] = paramList;

                return true;
            }
            else if (9 == type)
            {
                if (null == otherTabInfoList)
                {
                    return false;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in otherTabInfoList)
                {
                    if (itemId == tab.itemId)
                    {
                        tab.num = num;

                        tabInfo = tab;
                        break;
                    }
                }

                if (null == tabInfo)
                {
                    return false;
                }

                if (0 == tabInfo.num)
                {
                    otherTabInfoList.Remove(tabInfo);
                }

                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", tabInfo.itemId.ToString());
                paramDic.Add("ItemNo", tabInfo.itemNo.ToString());
                paramDic.Add("Num", tabInfo.num.ToString());
                paramDic.Add("Quality", tabInfo.itemQuality.ToString());
                paramDic.Add("Location", tabInfo.location.ToString());
                paramDic.Add("IsEquip", tabInfo.isEquiped.ToString ().ToLower ());

                paramList.Add(paramDic);

                param["Type"] = (short)109;
                param["PackInfo"] = paramList;

                return true;
            }

            return false;
        }

        void UpdateTabItemInfo (ItemInfo item)
        {
            if (2 == item.ItemType)
            {
                if (null == scrollTabInfoList)
                {
                    return;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in scrollTabInfoList)
                {
                    if (item.ID == tab.itemId)
                    {
                        tab.num = item.HeapNum;
                        tabInfo = tab;
                        break;
                    }
                }

                if (null != tabInfo)
                {
                    if (0 == tabInfo.num)
                    {
                        scrollTabInfoList.Remove(tabInfo);
                    }
                }
            }
            else if (3 == item.ItemType)
            {
                if (null == pieceTabInfoList)
                {
                    return;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in pieceTabInfoList)
                {
                    if (item.ID == tab.itemId)
                    {
                        tab.num = item.HeapNum;
                        tabInfo = tab;
                        break;
                    }
                }

                if (null != tabInfo)
                {
                    if (0 == tabInfo.num)
                    {
                        scrollTabInfoList.Remove(tabInfo);
                    }
                }
            }
            else if (9 == item.ItemType)
            {
                if (null == otherTabInfoList)
                {
                    return;
                }

                TabItemInfo tabInfo = null;
                foreach (TabItemInfo tab in otherTabInfoList)
                {
                    if (item.ID == tab.itemId)
                    {
                        tab.num = item.HeapNum;
                        tabInfo = tab;
                        break;
                    }
                }

                if (null != tabInfo)
                {
                    if (0 == tabInfo.num)
                    {
                        scrollTabInfoList.Remove(tabInfo);
                    }
                }
            }
            
        }

        //void UpdateTabEquiInfo(EquipInfo equip)
        //{
        //    TabItemInfo tabInfo = null;
        //    foreach (TabItemInfo tab in equipTabInfoList)
        //    {
        //        if (equip.ID == tab.itemId)
        //        {
        //            tab.num = item.HeapNum;
        //            tabInfo = tab;
        //            break;
        //        }
        //    }

        //    if (null != tabInfo)
        //    {
        //        if (0 == tabInfo.num)
        //        {
        //            scrollTabInfoList.Remove(tabInfo);
        //        }
        //    }
        //}

        /// <summary>
        /// 得到玩家物品或装备详细信息ActionParam
        /// </summary>
        /// <param name="itemId"></param>
        public bool GetPlayerItemDetailInfoParam (long itemId, out ActionParam param)
        {
            param = null;

            if (!isGetServerBagItemInfo && !isGetServerBagEquipInfo)
            {
                return false;
            }

            param = new ActionParam();

            bool isFindItem = false;

            if (isGetServerBagItemInfo)
            {
                foreach (ItemInfo item in playerBagItemInfoList)
                {
                    if (itemId == item.ID)
                    {
                        isFindItem = true;

                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        paramDic.Add("Name", item.Name);
                        paramDic.Add("ID", item.ItemNo.ToString());
                        paramDic.Add("Type", item.ItemType.ToString());
                        paramDic.Add("Price", item.ItemPrice.ToString());
                        paramDic.Add("Quality", item.ItemQuality.ToString());

                        param["ItemInfo"] = paramDic;

                        return true;
                    }
                }
            }

            if (isGetServerBagEquipInfo)
            {
                foreach (EquipInfo equip in playerBagEquipInfoList)
                {
                    if (itemId == equip.ID)
                    {
                        isFindItem = true;

                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        paramDic.Add("Name", equip.Name);
                        paramDic.Add("ID", equip.EquipNo.ToString());
                        paramDic.Add("Type", "1");
                        paramDic.Add("Price", equip.EquipPrice.ToString());
                        paramDic.Add("Quality", equip.EquipQuality.ToString());

                        param["ItemInfo"] = paramDic;

                        return true;
                    }
                }
            }

            if (!isFindItem)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据equipId获取玩家装备选择信息Param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetPlayerEquipChooseInfoParam(long equipId, out ActionParam param)
        {
            param = new ActionParam();

            if (!isGetServerBagEquipInfo)
            {
                return false;
            }
            
            List<EquipInfo> equipInfoList = playerEquip.GetPlayerEquipInfo (1);

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();
            int location = 1;

            //当equipId为0的时候，返回全部可用装备,需要屏蔽掉与当前穿戴装备同类型的其他装备
            if (0 >= equipId)
            {
                foreach (EquipInfo equip in playerBagEquipInfoList)
                {
                    if (null != equipInfoList && 0 != equipInfoList.Count)
                    {
                        bool isSameEquipType = false;
                        foreach (EquipInfo currentEquip in equipInfoList)
                        {
                            if (equip.EquipNo == currentEquip.EquipNo)
                            {
                                isSameEquipType = true;
                                break;
                            }
                        }

                        if (isSameEquipType)
                        {
                            continue;
                        }
                    }

                    if (!equip.IsBelongequipPlan1)
                    {
                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        paramDic.Add("EquipID", equip.ID.ToString());
                        paramDic.Add("EquipNo", equip.EquipNo.ToString());
                        paramDic.Add("Quality", equip.EquipQuality.ToString());
                        paramDic.Add("Location", location.ToString());

                        paramList.Add(paramDic);

                        ++location;
                    }
                }

                param["ChooseEquipInfo"] = paramList;

                return true;
            }

            //当EquipId不为0的时候，返回全部可用装备，需要屏蔽掉除EquipId装备外的其他装备的同类型装备
            EquipInfo chooseEquip = null;
            foreach (EquipInfo equip in playerBagEquipInfoList)
            {
                if (equipId == equip.ID)
                {
                    chooseEquip = equip;
                    break;
                }
            }

            if (null == chooseEquip)
            {
                return false;
            }

            foreach (EquipInfo equip in playerBagEquipInfoList)
            {
                if (null != equipInfoList && 0 != equipInfoList.Count)
                {
                    bool isSameEquipType = false;
                    foreach (EquipInfo currentEquip in equipInfoList)
                    {
                        if (equip.EquipNo == currentEquip.EquipNo && currentEquip.EquipNo != chooseEquip.EquipNo)
                        {
                            isSameEquipType = true;
                            break;
                        }
                    }

                    if (isSameEquipType)
                    {
                        continue;
                    }
                }

                if (chooseEquip.ID != equip.ID && chooseEquip.EquipNo != equip.ID && !equip.IsBelongequipPlan1)
                {
                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("EquipID", equip.ID.ToString());
                    paramDic.Add("EquipNo", equip.EquipNo.ToString());
                    paramDic.Add("Quality", equip.EquipQuality.ToString());
                    paramDic.Add("Location", location.ToString ());

                    paramList.Add(paramDic);

                    ++location;
                }
            }

            param["ChooseEquipInfo"] = paramList;

            return true;
        }

        /// <summary>
        /// 获得物品类型1表示装备，0表示其他物品，-1表示没有该物品
        /// </summary>
        /// <returns></returns>
        public int GetItemType(long itemId)
        {
            int res = -1;

            foreach (ItemInfo item in playerBagItemInfoList)
            {
                if (itemId == item.ID)
                {
                    res = 0;
                    break;
                }
            }

            if (-1 != res)
            {
                return res;
            }

            foreach (EquipInfo equip in playerBagEquipInfoList)
            {
                if (itemId == equip.ID)
                {
                    res = 1;
                    break;
                }
            }

            return res;
        }
    }
}
