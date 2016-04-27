using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public enum LogicActionDefine
    {
        LoginServer,    //登录
        CreateChar,     //创建角色
        DeleteChar,     //删除角色
        EnterGame,      //进入游戏
        ChangeStageResult, //改变状态阶段结果
        ChangeMapResult, //改变地图结果
        ReturnToLogin,  //返回登录
        UseSkill,       //使用技能
        MoveTo,         //角色移动
        StopMove,       //角色停止移动
        GMCommand,      //GM命令
        ChangeFloor,    //换楼层
        ChangeScene,    //换场景
		GetEquipInfoList, //获取角色装备信息
        GetItemInfoList,  //获取角色物品信息
        ButtonEvent,      //按钮点击事件
		UseItem,  //使用物品
        GetItemDetailInfo, //获取物品或装备的详细信息
        ItemResort,  //背包排序
        LearnSkill,
		ChangeEquip, //换装备，可能是穿装备或者脱装备
        GetBagEquip, //获取背包装备信息
        HitSprite,  //命中
		GetBagTabItemInfo, //获取背包分类物品信息
		ShowPlayerAttr, //显示人物属性
    }

    public class ClientDefine
    {
        public static int MinOtherPlayerSpriteID = 1000;
        public static int MinMonsterSpriteID = 1000000;
        public static int MinHurtSpriteID = 2000000;
        public static int MinSfxSpriteID = 3000000;
    }

    public class ParamMoveTo
    {
        public short CurrX;
        public short CurrZ;
        public short DestX;
        public short DestZ;
    }

    public class ParamStopMove
    {
        public short CurrX;
        public short CurrZ;
    }
}
