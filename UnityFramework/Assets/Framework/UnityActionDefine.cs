using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{

    public enum UnityActionDefine
    {
        ChangeStage, //改变stage
        SendChar, //发送角色数据
        DeleteChar,//删除角色
        ActionFinish,//炒作结束
        ShowMsg,//显示消息
        LoadSprite,//加载精灵
        LoadScene,//加载场景
        MoveSprite,//移动精灵
        ActSprite,//精灵动作
        KillSprite,//删除精灵
        LocatSprite, //瞬间移动精灵
        ShowSprite, //显示或隐藏精灵
        HurtSprite,     //伤害飘字
        BackPackInfo,
        ButtonEvent,    //界面操作事件
        SetSpriteInfo,  //头顶信息
        RecvLearnSkill,
        RecvLevelUp,
        RecvStudySkill,
		ItemInfo, //显示物品详细信息
        ControlPlayer, //控制主角的移动速度和能否移动
        LoadCreature,   //创建技能特效
        KillCreature,   //删除技能特效
        ControlSpriteHud,   //控制血条是否显示
		ShowRoleProperty, //显示人物属性
        ShowPlayerEquip, //显示人物的当前装备
        ShowPlayerChooseEquip, //显示人物选择装备时的可选装备
        RecvChosenSkill,
        RecvChosenOneSkill,
        ToDeathOrReBorn, //通知角色死亡或复活
    }

    public enum MsgPos
    {
        Dialog,
        MainUI,
        ChatUI,
    }

    public enum ESpriteType
    {
        Unknown = -1,
        Player = 0,
        OtherPlayer = 1,
        Ghost = 2,
        NPC = 3,
        Monster,
    }
}

namespace SPSGame.Unity
{
    public enum ECreatureType
    {
        Unknown = 0,
        Bullet = 1,  //子弹类型
        Cross = 2,   //直线穿透类型
        Hang = 3,    //精灵悬挂类型
        Point = 4,   //定点施放类型
    }

    public enum ButtonEventType
    {
        LeaveBattle = 0,
    }

    public enum EHurtType
    {
        Normal = 0,
        Crit = 1,
        Miss = 2,
        Cure = 3
    }

    public enum ESceneType
    {
        UnKnown,
        MainCity = 1,
        Tower = 2,
        Boss = 3,
    }

    public enum EGameStage
    {
        Default = 0,
        StartUp,
        Update,
        Login,
        SelectRole,
        Gaming,
    }

    public enum EItemType
    {
        Default = 0,
        Equip = 1,
        Stone = 2,
        Stuff = 3,
        Consumer = 4,
    }

    public enum EItemQuality
    {
        Default = 0,
        White = 1,
        Green = 2,
        Blue = 3,
        Purple = 4,
        Orange = 5,
    }

    //public enum EAnimType
    //{
    //    idle = 0,
    //    walk = 1,
    //    run = 2,

    //    action1 = 11,
    //    action2 = 12,
    //    action3 = 13,
    //    action4 = 14,
    //    action5 = 15,

    //    state1 = 21,
    //    state2 = 22,
    //    state3 = 23,
    //    state4 = 24,
    //    state5 = 25,

    //    attack1 = 31,
    //    attack2 = 32,
    //    attack3 = 33,
    //    attack4 = 34,
    //    attack5 = 35,
    //    attack6 = 36,
    //    attack7 = 37,
    //    attack8 = 38,
    //    attack9 = 39,
    //    attack10 = 40,

    //    hurt = 50,
    //    death = 100,
    //}

    public enum EAnimType
    {
        idle = 1,
        walk = 2,
        run = 3,
        death = 4,
        hurt = 5,

        action11 = 11,
        action12 = 12,
        action13 = 13,
        action14 = 14,
        action15 = 15,
        action16 = 16,
        action17 = 17,
        action18 = 18,
        action19 = 19,
        action20 = 20,
        action21 = 21,
        action22 = 22,
        action23 = 23,
        action24 = 24,
        action25 = 25,
        action26 = 26,
        action27 = 27,
        action28 = 28,
        action29 = 29,
        action30 = 30,
        action31 = 31,
        action32 = 32,
        action33 = 33,
        action34 = 34,
        action35 = 35,
        action36 = 36,
        action37 = 37,
        action38 = 38,
        action39 = 39,
        action40 = 40,
    }

    public enum EUIWndType
    {
        Window = 0,
        Board = 100,
        Dialog = 200,
        MsgBox = 300,
        Loading = 1000,
    }

    public enum MsgStyle
    {
        Yes,
        YesAndNo,
        YesAndNoAndCancel
    }

    public enum MsgResult
    {
        Yes,
        No,
        Cancel,
    }
}
