using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace AssetBundles
{
    class AssetRefCache
    {
        public long m_Guid;
        int m_RefCount;
        public string m_ABNames;
        public AssetRefCache()
        {
            m_RefCount = 0;
        }

        public void AddRef()
        {
            m_RefCount++;
        }
    }

    class PackCache
    {
        public string m_Name;
        public List<string> m_RefPacks;
        public PackCache(string name)
        {
            m_Name = name;
            m_RefPacks = new List<string>();
        }
    }


    public class CheckReference
    {

        static Dictionary<string, AssetRefCache> m_CacheDics;
        static Dictionary<string, AssetRefCache> m_SplitAssetDics;
        static Dictionary<string, PackCache> m_CachePackDics;


        static private List<string> GetFolderNames(string path)
        {
            List<string> dirs = new List<string>();
            foreach (string strs in Directory.GetDirectories(path))
            {
                dirs.Add(strs);
            }
            return dirs;
        }

        static List<string> GetFileNames(string path)
        {
            List<string> dirs = new List<string>();
            foreach (string strs in Directory.GetFiles(path))
            {
                dirs.Add(strs);
            }
            return dirs;
        }

        [MenuItem("AssetBundle/AssetBundleUnpack", false, 10)]
        static private void AssetBundleUnpack()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Equals(""))
                return;
            m_SplitAssetDics = new Dictionary<string, AssetRefCache>();
            m_CacheDics = new Dictionary<string, AssetRefCache>();
            m_CachePackDics = new Dictionary<string, PackCache>();

            SplitPack(GetFolderNames(path).ToArray());
            string outputPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);
        }
        /// <summary>
        /// 分割沉余资源
        /// </summary>
        static private void SplitPack(string[] paths)
        {

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i].Replace("\\", "/");
                m_CachePackDics.Add(paths[i], new PackCache(paths[i]));
                CacheFolder(paths[i], paths[i]);
                CacheFile(paths[i], paths[i]);

            }

            foreach (KeyValuePair<string, AssetRefCache> cache in m_SplitAssetDics)
            {
                string guid = AssetDatabase.AssetPathToGUID(cache.Key);
                SetAssetBundleName(guid, cache.Key);
                m_CachePackDics[cache.Value.m_ABNames].m_RefPacks.Add(guid);
            }
        }

        static private void CacheFolder(string path, string abName)
        {
            path.Replace("\\", "/");
            foreach (string str in Directory.GetDirectories(path))
            {
                CacheFile(str, abName);
            }
        }
        static private void CacheFile(string path, string abName)
        {
            path.Replace("\\", "/");
            foreach (string str in Directory.GetFiles(path))
            {
                CacheDependencies(str, abName);
            }

        }

        static private void CacheDependencies(string path, string abName)
        {
            path.Replace("\\", "/");
            string[] strs = AssetDatabase.GetDependencies(path);
            foreach (string str in strs)
            {
                AssetRefCache cache;
                if (m_CacheDics.TryGetValue(str, out cache))
                {
                    if (cache.m_ABNames.Equals(abName))
                    {
                        cache.AddRef();
                        if (!m_SplitAssetDics.ContainsKey(str))
                            m_SplitAssetDics.Add(str, cache);
                    }
                }
                else
                {
                    cache = new AssetRefCache();
                    cache.m_ABNames = abName;
                    m_CacheDics.Add(str, cache);
                }
            }
            SetAssetBundleName(abName, path);
        }


        /// <summary>
        /// 修改ab name
        /// </summary>
        /// <param name="path"></param>
        static private void SetAssetBundleName(string name, string path)
        {
            path = path.Replace("\\", "/");
            path = path.Replace(".meta", "");
            var importer = AssetImporter.GetAtPath(path);
            if (importer && importer.assetBundleName != name)
            {
                name.Replace("\\", "/");
                importer.assetBundleName = name;
            }
        }

    }
}