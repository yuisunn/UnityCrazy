using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class MyItemBank
    {
        bool isGetServerBankItemInfo = false; //是否已经获取了服务器的仓库物品信息

        protected List<EquipInfo> playerBankEquipInfoList = null; //表示角色的仓库装备信息
        protected List<ItemInfo> playerBankItemInfoList = null; //表示角色的仓库物品信息

        public bool IsGetServerBankItemInfo
        {
            get
            {
                return isGetServerBankItemInfo;
            }
        }

        public void Reset ()
        {
            isGetServerBankItemInfo = false;

            playerBankEquipInfoList = null;
            playerBankItemInfoList = null; 
        }

        /// <summary>
        /// 填充玩家全部仓库物品信息
        /// </summary>
        /// <param name="itemInfoList"></param>
        /// <param name="equipInfoList"></param>
        public void OnRcvPlayerBankItemInfo(List<ItemInfo> itemInfoList, List<EquipInfo> equipInfoList)
        {
            if (null == itemInfoList || null == equipInfoList)
            {
                return;
            }

            if (!isGetServerBankItemInfo)
            {
                isGetServerBankItemInfo = true;
            }

            playerBankEquipInfoList = equipInfoList;
            playerBankItemInfoList = itemInfoList;
        }

        /// <summary>
        /// 增加玩家仓库物品信息
        /// </summary>
        /// <param name="itemInfoList"></param>
        public void AddPlayerBankItemInfo(List<ItemInfo> itemInfoList)
        {
            foreach (ItemInfo item in itemInfoList)
            {
                bool isFindItem = false;

                foreach (ItemInfo playerItem in playerBankItemInfoList)
                {
                    if (item.ID == playerItem.ID)
                    {
                        playerItem.HeapNum = item.HeapNum;
                        isFindItem = true;

                        break;
                    }
                }

                if (!isFindItem)
                {
                    playerBankItemInfoList.Add(item);
                }
            }
        }

        /// <summary>
        /// 增加玩家仓库装备信息
        /// </summary>
        /// <param name="equipInfoList"></param>
        public void AddPlayerBankEquipInfo(List<EquipInfo> equipInfoList)
        {
            foreach (EquipInfo equip in equipInfoList)
            {
                playerBankEquipInfoList.Add(equip);
            }
        }

        /// <summary>
        /// 减少玩家仓库物品信息
        /// </summary>
        public void ReducePlayerBankItemInfo (long itemId, short itemNum, out ItemInfo itemInfo)
        {
            itemInfo = null;
            bool isZero = false;

            foreach (ItemInfo item in playerBankItemInfoList)
            {
                if (itemId == item.ID)
                {
                    if (itemNum <= 0)
                    {
                        isZero = true;
                    }
                    else
                    {
                        item.HeapNum = itemNum;
                    }

                    itemInfo = item;

                    break;
                }
            }

            if (isZero && null != itemInfo)
            {
                playerBankItemInfoList.Remove(itemInfo);
                itemInfo.HeapNum = 0;
            }
        }

        public bool GetPlayerBankInfoParam(out ActionParam param)
        {
            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (ItemInfo item in playerBankItemInfoList)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", item.ID.ToString ());
                paramDic.Add("ItemNo", item.ItemNo.ToString());
                paramDic.Add("Num", item.HeapNum.ToString());
                paramDic.Add("Type", item.ItemType.ToString());
                paramDic.Add("Location", item.StorageLocationNo.ToString ());

                paramList.Add(paramDic);
            }

            foreach (EquipInfo equip in playerBankEquipInfoList)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("ItemID", equip.ID.ToString ());
                paramDic.Add("ItemNo", equip.EquipNo.ToString());
                paramDic.Add("Num", "1");
                paramDic.Add("Type", "1");
                paramDic.Add("Location", equip.StorageLocationNo.ToString());

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
    }
}
