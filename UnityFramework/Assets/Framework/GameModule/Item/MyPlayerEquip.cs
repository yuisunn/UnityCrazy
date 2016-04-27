using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class MyPlayerEquip
    {
        bool[] serverEquipInfoList = { false, false, false, false }; //是否已经获取了服务器的装备数据

        protected Dictionary<short, List<EquipInfo>> playerEquipDic = new Dictionary<short, List<EquipInfo>>(); //表示角色的装备信息

        public bool IsGetServerEquipInfo(short equipPlanNo)
        {
            if (equipPlanNo > serverEquipInfoList.Length || equipPlanNo < 1)
            {
                return false;
            }

            return serverEquipInfoList[equipPlanNo - 1];
        }

        public void Reset ()
        {
            for (int i = 0; i < serverEquipInfoList.Length; ++i)
            {
                serverEquipInfoList[i] = false;
            }

            playerEquipDic.Clear(); 
        }

        /// <summary>
        /// 填充玩家全部装备信息
        /// </summary>
        /// <param name="equipInfoList"></param>
        /// <param name="equipPlanNo"></param>
        public void OnRcvPlayerEquiInfo(List<EquipInfo> equipInfoList, short equipPlanNo)
        {
            if (null == equipInfoList)
            {
                return;
            }

            if (!serverEquipInfoList[equipPlanNo - 1])
            {
                serverEquipInfoList[equipPlanNo - 1] = true;
            }

            if (playerEquipDic.ContainsKey(equipPlanNo))
            {
                playerEquipDic[equipPlanNo] = equipInfoList;
            }
            else
            {
                playerEquipDic.Add(equipPlanNo, equipInfoList);
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            foreach (EquipInfo equip in equipInfoList)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                paramDic.Add("EquipID", equip.ID.ToString ());
                paramDic.Add("EquipNo", equip.EquipNo.ToString ());
                paramDic.Add("Quality", equip.EquipQuality.ToString());
                paramDic.Add("Location", equip.Plan1LocationNo.ToString ());
                paramDic.Add("IsEquip", "1");

                paramList.Add(paramDic);
            }

            //将装备信息返回给界面
            ActionParam param = new ActionParam();
            param["PlayerEquip"] = paramList;

            Local.Instance.CallUnityAction(UnityActionDefine.ShowPlayerEquip, param);
        }

        /// <summary>
        /// 接收玩家换装备后的信息
        /// </summary>
        /// <returns></returns>
        public bool OnRcvPlayerChangeEquipInfo(short equipPlanNo, List<EquipInfo> equipInfoList)
        {
            if (!serverEquipInfoList[equipPlanNo - 1])
            {
                return false;
            }

            List<EquipInfo> playerEquipInfoList = playerEquipDic[equipPlanNo];

            if (null == playerEquipInfoList)
            {
                return false;
            }

            List<Dictionary<string, string>> paramList = new List<Dictionary<string, string>>();

            bool isChangeEquip = false; //表示是否为替换装备，而不是穿上或者脱下装备

            if (2 == equipInfoList.Count)
            {
                isChangeEquip = true;
            }

            foreach (EquipInfo equip in equipInfoList)
            {
                short oldLocation = 0;

                switch (equipPlanNo)
                {
                    case 1:
                        {
                            if (equip.IsBelongequipPlan1)
                            {
                                playerEquipInfoList.Add(equip);
                            }
                            else
                            {
                                oldLocation = RemovePlayerEquipInfo(equip, playerEquipInfoList);
                                if (-1 == oldLocation)
                                {
                                    return false;
                                }
                            }

                            break;
                        }
                    case 2:
                        {
                            if (equip.IsBelongequipPlan2)
                            {
                                playerEquipInfoList.Add(equip);
                            }
                            else
                            {
                                oldLocation = RemovePlayerEquipInfo(equip, playerEquipInfoList);
                                if (-1 == oldLocation)
                                {
                                    return false;
                                }
                            }

                            break;
                        }
                    case 3:
                        {
                            if (equip.IsBelongequipPlan3)
                            {
                                playerEquipInfoList.Add(equip);
                            }
                            else
                            {
                                oldLocation = RemovePlayerEquipInfo(equip, playerEquipInfoList);
                                if (-1 == oldLocation)
                                {
                                    return false;
                                }
                            }

                            break;
                        }
                    case 4:
                        {
                            if (equip.IsBelongequipPlan4)
                            {
                                playerEquipInfoList.Add(equip);
                            }
                            else
                            {
                                oldLocation = RemovePlayerEquipInfo(equip, playerEquipInfoList);
                                if (-1 == oldLocation)
                                {
                                    return false;
                                }
                            }

                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }

                if (isChangeEquip)
                {
                    if (equip.IsBelongequipPlan1)
                    {
                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        paramDic.Add("EquipID", equip.ID.ToString());
                        paramDic.Add("EquipNo", equip.EquipNo.ToString());
                        paramDic.Add("Quality", equip.EquipQuality.ToString());
                        paramDic.Add("Location", equip.Plan1LocationNo.ToString());
                        paramDic.Add("IsEquip", "1");

                        paramList.Add(paramDic);
                    }
                }
                else
                {
                    Dictionary<string, string> paramDic = new Dictionary<string, string>();
                    paramDic.Add("EquipID", equip.ID.ToString());
                    paramDic.Add("EquipNo", equip.EquipNo.ToString());
                    paramDic.Add("Quality", equip.EquipQuality.ToString());

                    if (equip.IsBelongequipPlan1)
                    {
                        paramDic.Add("Location", equip.Plan1LocationNo.ToString());
                        paramDic.Add("IsEquip", "1");
                    }
                    else
                    {
                        paramDic.Add("Location", oldLocation.ToString ());
                        paramDic.Add("IsEquip", "0");
                    }

                    paramList.Add(paramDic);
                }

            }

            //生成ActionParam
            ActionParam param = new ActionParam();
            param["PlayerEquip"] = paramList;

            //将装备信息返回给界面
            Local.Instance.CallUnityAction(UnityActionDefine.ShowPlayerEquip, param);

            return true;
        }

        short RemovePlayerEquipInfo(EquipInfo equip, List<EquipInfo> playerEquipInfoList)
        {
            short oldLocation = 0;

            EquipInfo removeEquip = null;
            foreach (EquipInfo playerEquip in playerEquipInfoList)
            {
                if (equip.ID == playerEquip.ID)
                {
                    oldLocation = playerEquip.Plan1LocationNo;

                    removeEquip = playerEquip;
                    break;
                }
            }

            if (null == removeEquip)
            {
                return -1;
            }

            playerEquipInfoList.Remove(removeEquip);

            return oldLocation;
        }

        /// <summary>
        /// 得到玩家当前装备详细信息ActionParam
        /// </summary>
        /// <param name="itemId"></param>
        public bool GetPlayerItemDetailInfoParam(long itemId, out ActionParam param)
        {
            param = null;

            if (!serverEquipInfoList[0])
            {
                return false;
            }

            param = new ActionParam();

            List<EquipInfo> equipInfoList = playerEquipDic[1];

            bool isFindItem = false;

            foreach (EquipInfo equip in equipInfoList)
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

            if (!isFindItem)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取玩家当前装备信息
        /// </summary>
        /// <param name="planNo"></param>
        /// <returns></returns>
        public List<EquipInfo> GetPlayerEquipInfo (short planNo)
        {
            if (!IsGetServerEquipInfo(planNo))
            {
                return null;
            }

            return playerEquipDic[planNo];
        }
    }
}
