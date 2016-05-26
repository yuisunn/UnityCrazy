using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SLCGame.Tools;

namespace SLCGame.Unity
{
    /// <summary>
    /// asset 的加载 
    /// 功能没有抽象解耦出来
    /// </summary>
    public class AssetBundleMgr : Singleton<AssetBundleMgr>
    {
        public void LoadCachedGameObject(string path)
        {
#if UNITY_EDITOR
// var Obj = Resources.LoadAssetAtPath<T>(path); 
#else
#endif
        }

        public Dictionary<string, BundleCache> mAssetBundleDic = new Dictionary<string, BundleCache>();

        public class BundleCache
        {
            AssetBundle mAssetBundle;
            public float timer = 0;
            public float waitTime = 5;

            public AssetBundle assetBundle
            {
                get
                {
                    timer = 0;
                    return mAssetBundle;
                }
            }

            public BundleCache(AssetBundle ab)
            {
                mAssetBundle = ab;
            }

            public bool Tick()
            {
                timer += Time.deltaTime;
                return timer < waitTime;
            }

            public void Unload(bool unloadallobjects)
            {
                assetBundle.Unload(unloadallobjects);
            }
        }

        /// <summary>
        /// 通过 assetbundle 读取单个gameobject
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sourcename"></param>
        /// <param name="cb"></param>
        /// <param name="forceloadAsyn"></param>
 

        public void LoadAudio(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath(string.Format("Audio/{0}", filename));
            LoadAsset(path, sourcename, cb, forceloadAsyn);
        }
        public void LoadConfig(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath(string.Format("Config/{0}", filename));
            LoadAsset(path, sourcename, cb, forceloadAsyn);
        }
        public void LoadConfig(string filename,UnityAction<AssetBundle> cb)
        {
            string path = PathMod.AssetPath(string.Format("Config/{0}", filename));
            LoadAsset(path,cb, false);
        }

        public void LoadEffect(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath(string.Format("Effect/{0}", filename));
            LoadAsset(path, sourcename, cb, forceloadAsyn);
        }
        public void LoadEquip(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath(string.Format("Equip/{0}", filename));
            LoadAsset(path, sourcename, cb, forceloadAsyn);
        }
        public void LoadMonster(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath( string.Format("Monster/{0}",filename));
            LoadAsset(path, sourcename, cb, forceloadAsyn);
        }

 

        /// <summary>
        /// 加载Asset资源
        /// </summary>
        /// <param name="path">StreamingAssets下相对路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调函数</param>
        /// <returns></returns>
        void LoadAsset(string path, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = true)
        {
            if (mAssetBundleDic.ContainsKey(path))
            {
                AssetBundle ab = GetAssetBundle(path);
                if (forceloadAsyn)
                {
                    GameMain.Instance.StartCoroutine(LoadSourceEnumerator(ab, sourcename, cb));
                }
                else
                {
                    Object go = ab.LoadAsset(sourcename);
                    if (go != null)
                    {
                        cb(go);
                    }
                    else
                    {
                        DebugMod.LogError("can't get assetresource from " + path);
                    }
                }

            }
            else
            {
                GameMain.Instance.StartCoroutine(LoadBundleEnumerator(path, sourcename, cb, forceloadAsyn));
            }
        }

        void LoadAsset(string path,  UnityAction<AssetBundle> cb, bool managercharge = true)
        {
            if (mAssetBundleDic.ContainsKey(path))
            {
                AssetBundle ab = GetAssetBundle(path); ;
                cb(ab);
            }
            else
            {
                GameMain.Instance.StartCoroutine(LoadBundleEnumerator(path, cb, managercharge));
            }
        }


        /// <summary>
        /// 加载AssetBundle中的资源
        /// </summary>
        /// <param name="ab">AssetBundle对象</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        IEnumerator LoadSourceEnumerator(AssetBundle ab, string sourcename, UnityAction<Object> cb)
        {
            AssetBundleRequest abr = ab.LoadAssetAsync(sourcename);
            Debug.Log("Source AssetBundleRequest");
            yield return abr;
            if (abr.isDone)
            {
                if (abr.asset != null)
                {
                    Debug.Log("Source cb");
                    cb(abr.asset);
                }
                else
                {
                    DebugMod.LogError("Can't find res " + sourcename + "in assetbundle");
                }
            }
        }


        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        IEnumerator LoadBundleEnumerator(string path, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            using (WWW www = new WWW(path))
            {
                yield return www;
                if (www.isDone)
                {
                    if (mAssetBundleDic.ContainsKey(path))
                    {
                        LoadAsset(path, sourcename, cb, forceloadAsyn);
                        yield break;
                    }

                    AssetBundle ab = www.assetBundle;
                    if (null == ab)
                    {
                        DebugMod.LogError("www.assetBundle is null when load:" + path);
                        yield break;
                    }

                    AddAssetBundle(path, ab);
                    LoadAsset(path, sourcename, cb, forceloadAsyn);
                }
                else
                {
                    DebugMod.LogError(www.error);
                }
            }
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        IEnumerator LoadBundleEnumerator(string path, UnityAction<AssetBundle> cb, bool managercharge = true)
        {
            using (WWW www = new WWW(path))
            {
                yield return www;
                if (www.isDone)
                {
                    AssetBundle ab = www.assetBundle;
                    if (null == ab)
                    {
                        DebugMod.LogError("www.assetBundle is null when load:" + path);
                        yield break;
                    }
                    if (managercharge)
                    {
                        AddAssetBundle(path, ab);
                        LoadAsset(path, cb);
                    }
                    else
                    {
                        try
                        {
                            cb(ab);
                        }
                        catch
                        {
                            DebugMod.LogError("Error occored in LoadBundleEnumerator callback delegate");
                        }
                    }
                        
                }
                else
                {
                    DebugMod.LogError(www.error);
                }
            }
        }


        /// <summary>
        /// 添加AssetBundle资源到字典
        /// </summary>
        /// <param name="key">路径key</param>
        /// <param name="assetBundle">bundle资源</param>
        /// <returns></returns>
        public void AddAssetBundle(string pathkey, AssetBundle assetBundle)
        {
            mAssetBundleDic[pathkey] = new BundleCache(assetBundle);
        }


        /// 卸载AssetBundle缓存
        /// </summary>
        /// <param name="pathkey">路径key</param>
        /// <param name="unloadallobjects">是否卸载所加载的对象</param>
        /// <returns></returns>
        public void UnloadAssetBundle(string pathkey, bool unloadallobjects)
        {
            if (mAssetBundleDic.ContainsKey(pathkey))
            {
                mAssetBundleDic[pathkey].Unload(unloadallobjects);
                mAssetBundleDic.Remove(pathkey);
            }
        }


        /// <summary>
        /// 卸载AssetBundle缓存
        /// </summary>
        /// <param name="ab">路径key</param>
        /// <param name="unloadallobjects">是否卸载所加载的对象</param>
        /// <returns></returns>
        public void UnloadAssetBundle(AssetBundle ab, bool unloadallobjects)
        {
            foreach (string key in mAssetBundleDic.Keys.ToArray())
            {
                if (mAssetBundleDic[key].assetBundle == ab)
                {
                    mAssetBundleDic[key].Unload(unloadallobjects);
                }
                mAssetBundleDic.Remove(key);
            }

        }

        /// <summary>
        /// 获取AssetBundle资源
        /// </summary>
        /// <param name="pathkey">路径key</param>
        /// <returns></returns>
        public AssetBundle GetAssetBundle(string pathkey)
        {
            BundleCache bc = null;
            if (!mAssetBundleDic.TryGetValue(pathkey, out bc))
            {
                return null;
            }
            return bc.assetBundle;
        }


        /// <summary>
        /// 清楚所有AssetBundle资源
        /// </summary>
        /// <returns></returns>
        public void ClearAssetBundles()
        {
            foreach (string key in mAssetBundleDic.Keys.ToArray())
            {
                mAssetBundleDic[key].Unload(false);
            }
            mAssetBundleDic.Clear();

        }

        public void Update()
        {    
            string[] keys = mAssetBundleDic.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i) 
            {
                if (!mAssetBundleDic[keys[i]].Tick())
                {
                    mAssetBundleDic[keys[i]].Unload(false);
                    mAssetBundleDic.Remove(keys[i]);
                }
            }
        }
    }
}