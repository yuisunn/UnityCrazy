using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class MyItemModule : GameModuleBase
    {
        MyPlayerEquip myPlayerEquip = new MyPlayerEquip(); //玩家装备信息
        MyItemBag myItemBag = new MyItemBag(); //玩家背包物品信息

        public override void OnInit()
        {
            //Local.Instance.SvrMgr.RegisterCmdRcver("game", SPSCmd.CMD_BUY_BUY, OnSC_BuyResult);

            //将玩家装备信息赋值给玩家背包物品信息
            myItemBag.playerEquip = myPlayerEquip;
        }


        public override void OnUpdate()
        {

        }

        /// <summary>
        /// 重置物品装备模块
        /// </summary>
        public void ResetItemModule ()
        {
            myPlayerEquip.Reset();
            myItemBag.Reset();
            myItemBag.Reset();
        }

        public void OnSC_BuyResult(ActionParam param)
        {
        }

        /// <summary>
        /// 获取全部玩家装备信息
        /// </summary>
        /// <param name="equipInfoList"></param>
        /// <param name="equipPlanNo"></param>
        public void OnRcvPlayerEquiInfo (List<EquipInfo> equipInfoList, short equipPlanNo)
        {
            myPlayerEquip.OnRcvPlayerEquiInfo(equipInfoList, equipPlanNo);
        }

        /// <summary>
        /// 获取全部玩家背包物品信息
        /// </summary>
        /// <param name="itemInfoList"></param>
        /// <param name="equipInfoList"></param>
        public void OnRcvPlayerBagItemInfo(List<ItemInfo> itemInfoList, List<EquipInfo> equipInfoList)
        {
            myItemBag.OnRcvPlayerBagItemInfo(itemInfoList, equipInfoList);
        }

        /// <summary>
        /// 当只收到玩家背包中装备信息时调用
        /// </summary>
        /// <param name="equipInfoList"></param>
        public void OnRcvPlayerOnlyBagEquipInfo(List<EquipInfo> equipInfoList)
        {
            myItemBag.OnRcvPlayerOnlyBagEquipInfo(equipInfoList);
        }

        /// <summary>
        /// 是否已经获取了服务器背包物品信息
        /// </summary>
        /// <returns></returns>
        public bool IsGetBagItemInfo ()
        {
            return myItemBag.IsGetServerBagItemInfo;
        }

        /// <summary>
        /// 是否已经获取了服务器背包装备信息
        /// </summary>
        /// <returns></returns>
        public bool IsGetBagEquipInfo ()
        {
            return myItemBag.IsGetServerBagEquipInfo;
        }

        /// <summary>
        /// 获取某一类别物品信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetBagTabItemInfo(short type, out ActionParam param)
        {
            return myItemBag.GetBagTabItemInfo(type, out param);
        }

        /// <summary>
        /// 是否已经获取了服务器玩家装备信息
        /// </summary>
        /// <param name="equipPlanNo"></param>
        /// <returns></returns>
        public bool IsGetEquipInfo (short equipPlanNo)
        {
            return myPlayerEquip.IsGetServerEquipInfo(equipPlanNo);
        }

        /// <summary>
        /// 增加玩家新物品信息
        /// </summary>
        public void AddPlayerItemInfo (List<ItemInfo> itemInfoList, out ActionParam param)
        {
            myItemBag.AddPlayerBagItemInfo(itemInfoList, out param);
        }

        /// <summary>
        /// 增加玩家新装备信息
        /// </summary>
        public void AddPlayerEquipInfo(List<EquipInfo> equipInfoList, out ActionParam param)
        {
            myItemBag.AddPlayerBagEquipInfo(equipInfoList, out param);
        }

        /// <summary>
        /// 减少玩家物品或装备信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemNum"></param>
        /// <param name="itemInfo"></param>
        public bool ReducePlayerItemInfo(long itemId, short itemNum, out ActionParam param)
        {
            return myItemBag.ReducePlayerBagItemInfo(itemId, itemNum, out param);
        }

        /// <summary>
        /// 获取玩家背包整理结果,并返回ActionParam
        /// </summary>
        public bool OnRcvPlayerItemResortInfo (List<ItemLocationChange> itemLocationChangeList, out ActionParam param)
        {
            return myItemBag.OnRcvPlayerItemResortInfo(itemLocationChangeList, out param);
        }

        /// <summary>
        /// 接收玩家换装备后的信息
        /// </summary>
        /// <returns></returns>
        public bool OnRcvPlayerChangeEquipInfo (short equipPlanNo, List<EquipInfo> equipInfoList)
        {
            if (!myItemBag.ChangeBagEquipInfo (equipInfoList))
            {
                return false;
            }

            return myPlayerEquip.OnRcvPlayerChangeEquipInfo(equipPlanNo, equipInfoList);
        }

        /// <summary>
        /// 获取玩家背包信息Param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetPlayerBagInfoParam (out ActionParam param)
        {
            return myItemBag.GetPlayerBagInfoParam(out param);
        }

        /// <summary>
        /// 根据equipId获取玩家装备选择信息Param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetPlayerEquipChooseInfoParam (long equipId, out ActionParam param)
        {
            return myItemBag.GetPlayerEquipChooseInfoParam(equipId, out param);
        }

        /// <summary>
        /// 将itemId的物品的信息转换为ActionParam
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool GetPlayerItemDetailInfoParam (long itemId, out ActionParam param)
        {
            if (!myItemBag.GetPlayerItemDetailInfoParam(itemId, out param))
            {
                return myPlayerEquip.GetPlayerItemDetailInfoParam(itemId, out param);
            }

            return true;
        }

        /// <summary>
        /// 更新背包分类物品信息
        /// </summary>
        /// <returns></returns>
        public bool AddBagTabItemInfo(List<ItemInfo> itemInfoList, out List<ActionParam> paramList)
        {
            return myItemBag.AddBagTabItemInfo(itemInfoList, out paramList);
        }

        /// <summary>
        /// 更新背包分类装备信息
        /// </summary>
        /// <returns></returns>
        public bool AddBagTabEquipInfo(List<EquipInfo> equipInfoList, out ActionParam param)
        {
            return myItemBag.AddBagTabEquipInfo(equipInfoList, out param);
        }

         /// <summary>
        /// 更新背包分类信息
        /// </summary>
        /// <returns></returns>
        public bool ReduceBagTabItemInfo (long itemId, short num, out ActionParam param)
        {
            return myItemBag.ReduceBagTabItemInfo(itemId, num, out param);
        }

        /// <summary>
        /// 获得物品类型1表示装备，0表示其他物品，-1表示没有该物品
        /// </summary>
        /// <returns></returns>
        public int GetItemType (long itemId)
        {
            return myItemBag.GetItemType(itemId);
        }
    }
}
