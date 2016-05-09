using UnityEngine;
using System.Collections;


namespace SLCGame.Unity
{
    public enum GUILAYER
    {
        GUI_BACKGROUND = 0, //背景层
        GUI_MENU,           //菜单层0
        GUI_MENU1,           //菜单层1
        GUI_PANEL,          //面板层
        GUI_PANEL1,         //面板1层
        GUI_PANEL2,         //面板2层
        GUI_PANEL3,         //面板3层
        GUI_FULL,           //满屏层
        GUI_MESSAGE,        //消息层
        GUI_MESSAGE1,        //消息层
        GUI_GUIDE,           //引导层
        GUI_LOADING,        //加载层
    }
    public enum MsgResult
    {
        Yes = 0,
        No = 1,
        Cancel = 2
    }
    public enum MsgStyle
    {
        Yes = 0,
        YesAndNo = 1,
        YesAndNoAndCancel = 2
    }

}