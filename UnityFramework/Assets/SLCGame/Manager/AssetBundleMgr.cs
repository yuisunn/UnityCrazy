using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SLCGame.Tools;
using System.IO;

namespace SLCGame.Unity
{

    public class BundleCache
    {
        AssetBundle m_AssetBundle;
        public float m_fTimer = 0;
        public float m_fWaitTime = 5;
        public bool m_isRelease;//是否执行释放
        public bool m_isAssetRelease;//是否销毁asset load的对象 
        public bool m_isHaveMirror;
        public AssetBundle assetBundle
        {
            get
            {
                m_fTimer = 0;
                return m_AssetBundle;
            }
        }

        public Object LoadAsset(string name)
        {
            return m_AssetBundle.LoadAsset(name);
        }

        public BundleCache(AssetBundle ab)
        {
            m_isRelease = true;
            m_isAssetRelease = false;
            m_AssetBundle = ab;
            m_isHaveMirror = true;
        }

        public void Release()
        {
            if (m_isRelease && m_AssetBundle != null)
            {
                m_AssetBundle.Unload(m_isAssetRelease);
            }
        } 

        public bool Tick()
        {
            m_fTimer += Time.deltaTime;
            return m_fTimer < m_fWaitTime;
        }

        public void Unload(bool unloadallobjects)
        {
            assetBundle.Unload(unloadallobjects);
        }
    }

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



        /// <summary>
        /// 通过 assetbundle 读取单个gameobject
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sourcename"></param>
        /// <param name="cb"></param>
        /// <param name="forceloadAsyn"></param> 


        //public void LoadAudio(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        //{
        //    string path = PathMod.AssetPath(string.Format("Audio/{0}", filename));
        //    LoadAsset(path, sourcename, cb, forceloadAsyn);
        //}
        //public void LoadConfig(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        //{
        //    string path = PathMod.AssetPath(string.Format("Config/{0}", filename));
        //    LoadAsset(path, sourcename, cb, forceloadAsyn);
        //}
        //public void LoadConfig(string filename,UnityAction<AssetBundle> cb)
        //{
        //    string path = PathMod.AssetPath(string.Format("Config/{0}", filename));
        //    LoadAsset(path,cb, false);
        //}

        public void LoadEffect(string filename, string sourcename)
        {
            string path = PathMod.AssetPath(string.Format("Effect/{0}", filename));
            LoadAsset(path, sourcename);
        }
         
        public BundleCache LoadUIAssetBundle(string name)
        {
            LoadAsset(name);
            return mAssetBundleDic[name];
        }


        public GameObject LoadUIPerfab(string sourcename) 
        {
            string path = PathMod.DataPath + string.Format("{0}", sourcename.ToLower());
            return LoadAsset(path, sourcename) as GameObject;
        }

        public AssetBundle LoadAssetMemory(string name)
        {
            byte[] stream = null;
            AssetBundle bundle = null;
            string uri = name.ToLower();
            stream = File.ReadAllBytes(uri); 
            bundle = AssetBundle.LoadFromFile(name);
            AddAssetBundle(name, bundle);
            new BundleCache(bundle);
            return bundle; 
        }


        /// <summary>
        /// 加载Asset资源
        /// </summary>
        /// <param name="path">StreamingAssets下相对路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调函数</param>
        /// <returns></returns>
        public Object LoadAsset(string path, string sourcename)
        {
            Object obj = null;
            AssetBundle ab = GetAssetBundle(path);
            if (ab != null)
            {
                obj = ab.LoadAsset(sourcename); 
            }
            else
            {
                ab = LoadAssetMemory(path);
                AddAssetBundle(path, ab);
                obj = ab.LoadAsset(sourcename);

                if (obj == null)
                {
                    DebugMod.LogError("can't get assetresource object null from " + path);
                } 
            }
            return obj;
        }

        public AssetBundle LoadAsset(string path)
        {
            AssetBundle ab = null;
            if (mAssetBundleDic.ContainsKey(path))
            {
                ab = GetAssetBundle(path); 
            }
            else
            {
                ab = LoadAssetMemory(path);
                AddAssetBundle(path, ab);
                if(ab == null)
                    DebugMod.LogError("can't get AssetBundle null from " + path);
                //GameMain.Instance.StartCoroutine(LoadBundleEnumerator(path, cb, managercharge));
            }
            return ab;
        }


        /// <summary>
        /// 加载AssetBundle中的资源
        /// </summary>
        /// <param name="ab">AssetBundle对象</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        //IEnumerator LoadSourceEnumerator(AssetBundle ab, string sourcename, UnityAction<UnityEngine.Object> cb)
        //{
        //    AssetBundleRequest abr = ab.LoadAssetAsync(sourcename);
        //    Debug.Log("Source AssetBundleRequest");
        //    yield return abr;
        //    if (abr.isDone)
        //    {
        //        if (abr.asset != null)
        //        {
        //            Debug.Log("Source cb");
        //            cb(abr.asset);
        //        }
        //        else
        //        {
        //            DebugMod.LogError("Can't find res " + sourcename + "in assetbundle");
        //        }
        //    }
        //}


        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        //IEnumerator LoadBundleEnumerator(string path, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        //{
        //    using (WWW www = new WWW(path))
        //    {
        //        yield return www;
        //        if (www.isDone)
        //        {
        //            if (mAssetBundleDic.ContainsKey(path))
        //            {
        //                LoadAsset(path, sourcename, cb, forceloadAsyn);
        //                yield break;
        //            }

        //            AssetBundle ab = www.assetBundle;
        //            if (null == ab)
        //            {
        //                DebugMod.LogError("www.assetBundle is null when load:" + path);
        //                yield break;
        //            }

        //            AddAssetBundle(path, ab);
        //            LoadAsset(path, sourcename, cb, forceloadAsyn);
        //        }
        //        else
        //        {
        //            DebugMod.LogError(www.error);
        //        }
        //    }
        //}

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="sourcename">资源名</param>
        /// <param name="cb">回调</param>
        /// <returns></returns>
        //IEnumerator LoadBundleEnumerator(string path, UnityAction<AssetBundle> cb, bool managercharge = true)
        //{
        //    using (WWW www = new WWW(path))
        //    {
        //        yield return www;
        //        if (www.isDone)
        //        {
        //            AssetBundle ab = www.assetBundle;
        //            if (null == ab)
        //            {
        //                DebugMod.LogError("www.assetBundle is null when load:" + path);
        //                yield break;
        //            }
        //            if (managercharge)
        //            {
        //                AddAssetBundle(path, ab);
        //                LoadAsset(path, cb);
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    cb(ab);
        //                }
        //                catch
        //                {
        //                    DebugMod.LogError("Error occored in LoadBundleEnumerator callback delegate");
        //                }
        //            }
                        
        //        }
        //        else
        //        {
        //            DebugMod.LogError(www.error);
        //        }
        //    }
        //}


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
        /// 卸载全部资源 asset也一起
        /// </summary>
        public void ClearAllAssetBundles()
        {
            string[] keys = mAssetBundleDic.Keys.ToArray();
            for (int i = 0; i < mAssetBundleDic.Count; ++i)
            {
                mAssetBundleDic[keys[i]].assetBundle.Unload(true);
            }
            mAssetBundleDic.Clear();
        }

        /// <summary>
        /// 根据情况有的标记为不卸载的可以不卸载
        /// </summary>
        /// <returns></returns>
        public void ClearAssetBundles()
        {
            string[] keys = mAssetBundleDic.Keys.ToArray();
            for (int i = 0; i < mAssetBundleDic.Count; ++i)
            { 
                mAssetBundleDic[keys[i]].Release();
                if(mAssetBundleDic[keys[i]].m_isRelease)
                    mAssetBundleDic.Remove(keys[i]);
            }    
        }
        /// <summary>
        /// 释放全部镜像 
        /// </summary>
        public void ClearAssetBundlesMirror()
        {
            string[] keys = mAssetBundleDic.Keys.ToArray();
            for (int i = 0; i < mAssetBundleDic.Count; ++i)
            {
                mAssetBundleDic[keys[i]].assetBundle.Unload(false); 
            }
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