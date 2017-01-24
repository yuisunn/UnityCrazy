using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class NoticeConst
{
    /// <summary>
    /// Controller层消息通知
    /// </summary>
    public const string START_UP = "StartUp";                       //启动框架
    public const string DISPATCH_MESSAGE = "DispatchMessage";       //派发信息

    /// <summary>
    /// View层消息通知
    /// </summary>
    public const string UPDATE_MESSAGE = "UpdateMessage";           //更新消息
    public const string UPDATE_EXTRACT = "UpdateExtract";           //更新解包
    public const string UPDATE_DOWNLOAD = "UpdateDownload";         //更新下载
    public const string UPDATE_PROGRESS = "UpdateProgress";         //更新进度
}

class AppConst
    {
    public const bool DebugMode = true;                        //调试模式-用于内部测试

    /// <summary>
    /// 如果想删掉框架自带的例子，那这个例子模式必须要
    /// 关闭，否则会出现一些错误。
    /// </summary>
    public const bool ExampleMode = false;                       //例子模式 

    /// <summary>
    /// 如果开启更新模式，前提必须启动框架自带服务器端。
    /// 否则就需要自己将StreamingAssets里面的所有内容
    /// 复制到自己的Webserver上面，并修改下面的WebUrl。
    /// </summary>
    public const bool UpdateMode = false;                       //更新模式-默认关闭 
    public const bool AutoWrapMode = true;                      //自动添加Wrap模式

    public const bool UsePbc = true;                           //PBC
    public const bool UseLpeg = true;                          //lpeg
    public const bool UsePbLua = true;                         //Protobuff-lua-gen
    public const bool UseCJson = true;                         //CJson
    public const bool UseSproto = true;                        //Sproto
    public const bool LuaEncode = false;                        //使用LUA编码

    public const int TimerInterval = 1;
    public const int GameFrameRate = 30;                       //游戏帧频

    public const string AppName = "SimpleFramework";           //应用程序名称
    public const string AppPrefix = AppName + "_";             //应用程序前缀
    public const string WebUrl = "http://localhost:6688/";      //测试更新地址

    public static string UserId = string.Empty;                 //用户ID
    public static int SocketPort = 0;                           //Socket服务器端口
    public static string SocketAddress = string.Empty;          //Socket服务器地址

    public static string LuaFolder = "lua/";
    public static readonly string FilesName = "files.txt";

    public static string LuaBasePath
    {
        get { return Application.dataPath + "/uLua/Source/"; }
    }

    public static string LuaWrapPath
    {
        get { return LuaBasePath + "LuaWrap/"; }
    }
} 
