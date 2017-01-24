using UnityEngine;
using System.Collections;
using System.IO;

namespace SLCGame.Unity
{

    public class PathMod
    {
        public static readonly string LuaUI = "View/";

        /// <summary>
        /// 获取文件无后缀名称
        /// </summary>
        /// <param name="xmlName"></param>
        /// <returns></returns>
        public static string GetPureName(string fullnameOrPath)
        {
            if (string.IsNullOrEmpty(fullnameOrPath))
                return null;
            int start = fullnameOrPath.LastIndexOf("/");
            start += 1;
            int end = fullnameOrPath.LastIndexOf(".");
            if (end < 0)
            {
                end = fullnameOrPath.Length;
            }

            return fullnameOrPath.Substring(start, end - start);
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = Application.dataPath + "/StreamingAssets/";
                    break;
            }
            return path;
        }

        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                string game = AppConst.AppName.ToLower();
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/" + game + "/";
                }
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    return Application.streamingAssetsPath + "/";
                }
                if (Application.isEditor)
                {
                    return Application.dataPath + "/StreamingAssets/";
                }
                return "c:/" + game + "/";
            }
        }



        /// <summary>
        /// 获取Asset路径
        /// </summary>
        /// <param name="relativepath">相对路径（带后缀）</param>
        /// <returns></returns>
        public static string AssetPath(string relativepath)
        {
            string path = GetPersistentPath(relativepath);
            if (File.Exists(path))
            {
                return GetURL(path);
            }
            else
            {
                return GetSteamingAssetsPath(relativepath);
            }
        }

        public static string GetURL(string path)
        {
            if (path.StartsWith("http://") || path.StartsWith("ftp://") || path.StartsWith("https://") || path.StartsWith("file://") || path.StartsWith("jar:file://"))
            {
                return path;
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                return path.Insert(0, "file://");
            }
            else if (Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer)
            {
                return path.Insert(0, "file:///");
            }

            return path;
        }

        /// <summary>
        /// 获取SteamingAsset路径
        /// </summary>
        /// <param name="relativepath"></param>
        /// <returns></returns>
        private static string GetSteamingAssetsPath(string relativepath = "")
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return "file:///" + Application.dataPath + "/StreamingAssets/" + relativepath;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/" + relativepath;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Application.dataPath + "/Raw/" + relativepath;
            }
            else
                return "file:///" + Application.streamingAssetsPath + "/" + relativepath;
        }

        /// <summary>
        /// 获取资源更新路径
        /// </summary>
        /// <param name="relativepath"></param>
        /// <returns></returns>
        public static string GetPersistentPath(string relativepath)
        {
            return Application.persistentDataPath + "/" + relativepath;
        }

    }
}