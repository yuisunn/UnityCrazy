using UnityEngine;
using System.Collections;
using System.IO;

namespace SPSGame.Unity
{

    public class PathMod
    {

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