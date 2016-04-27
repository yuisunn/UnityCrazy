using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class GMCommand
    {
        public static void Do(string cmd)
        {
            try
            {
                string[] fields = cmd.Split(';');
                string type = fields[0];
                if (type.ToLower() == "map")
                {
                    ActionParam param = new ActionParam();
                    param["SceneID"] = int.Parse(fields[1]);
                    MyPlayerMove move = Local.Instance.GetMyPlayer.MoveLogic;
                    if (null != move)
                        move.OnEngineChangeMap(param);
                }
                else if(type.ToLower () == "additem")
                {
                    //增加物品信息

                    ActionParam param = new ActionParam();
                    param["ItemNo"] = int.Parse(fields[1]);
                    param["Num"] = short.Parse(fields[2]);

                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_SC_GetNewItem, param, null);

                    //增加装备信息

                    //ActionParam param = new ActionParam();
                    //param["EquipNo"] = int.Parse(fields[1]);
                    //param["StorageLocation"] = short.Parse(fields[3]);

                    //GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    //if (null == gameServer)
                    //{
                    //    return;
                    //}

                    //gameServer.ReadySend(CsShare.SPSCmd.CMD_SC_GetNewEquip, param, null);
                }
                else if (type.ToLower () == "addequip")
                {
                    //增加装备信息

                    ActionParam param = new ActionParam();
                    param["EquipNo"] = int.Parse(fields[1]);

                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_SC_GetNewEquip, param, null);
                }
                else if (type.ToLower () == "useitem")
                {
                    //使用物品信息

                    ActionParam param = new ActionParam();
                    param["Num"] = short.Parse (fields[1]);
                    param["ItemID"] = long.Parse(fields[2]);
                    param["UseType"] = short.Parse(fields[3]);

                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_UseItem, param, null);
                }
                else if (type.ToLower() == "itemresort")
                {
                    //背包排序

                    ActionParam param = new ActionParam();

                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_ItemResort, param, null);
                }
				else if (type.ToLower () == "equipinfo")
                {
                    ActionParam param = new ActionParam();
                    param["EquipPlanNo"] = short.Parse(fields[1]);

                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_EquipInfo_List, param, null);
                }
                else if (type.ToLower () == "changeequip")
                {
                    //更换装备

                    ActionParam param = new ActionParam();
                    param["BeforeEquipId"] = long.Parse (fields[1]);
                    param["NewEquipId"] = long.Parse(fields[2]);
                    param["EquipPlanNo"] = short.Parse(fields[3]);
                    param["EquipLocationNo"] = short.Parse(fields[4]);


                    GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                    if (null == gameServer)
                    {
                        return;
                    }

                    gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_ChangeEquip, param, null);

                }
                else if (type.ToLower () == "bagequip")
                {
                    MyItemModule myItemModule = Local.Instance.GetModule("item") as MyItemModule;

                    if (null == myItemModule)
                    {
                        return;
                    }

                    long equipId = long.Parse (fields[1]);

                    if (myItemModule.IsGetBagEquipInfo())
                    {
                        ActionParam param = null;

                        if (!myItemModule.GetPlayerEquipChooseInfoParam(equipId, out param))
                        {
                            return;
                        }

                        //调用UnityAction发送ActionParam

                        return;

                    }
                    else
                    {
                        GameServer gameServer = (GameServer)Local.Instance.SvrMgr.GetServer("game");

                        if (null == gameServer)
                        {
                            Local.Instance.CallActionFinish(LogicActionDefine.GetBagEquip, 0);
                            return;
                        }

                        ActionParam param = new ActionParam();
                        param["EquipId"] = equipId;
                        gameServer.ReadySend(CsShare.SPSCmd.CMD_CS_GetBagEquipInfo, param, null);

                        return;
                    }
                }
            }
            catch(Exception)
            {
                DebugMod.LogError("GMCommand error!!!");
            }
        }
    }
}
