using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SPSGame.Tools;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class Item
    {
        public string sourcename = string.Empty;
        public UnityAction<Object> cb = null;
        public MonsterLoader loader = null;
        public Item(string name, UnityAction<Object> _cb, MonsterLoader _loader)
        {
            sourcename = name;
            cb = _cb;
            loader = _loader;
        }
    }

    public class CasheMe
    {
        public List<Item> items = new List<Item>();

        public bool Load(AssetBundle bundle)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                Object go = bundle.LoadAsset(items[i].sourcename);
                try
                {                   
                    items[i].cb(go);                   
                }
                catch
                {
                    DebugMod.LogError("Error occored in MonsterLoader callback delegate");
                }
                items[i].loader.Destroy();
            }
            items.Clear();
            return true;
        }
    }

    public class MonsterLoader : U3DObject
    {

        static Dictionary<string, CasheMe> mDic = new Dictionary<string, CasheMe>();

        protected override void Start()
        {
            base.Start();
        }

        public void LoadMonster(string filename, string sourcename, UnityAction<Object> cb, bool forceloadAsyn = false)
        {
            string path = PathMod.AssetPath(string.Format("Monster/{0}", filename));

            if (!mDic.ContainsKey(path))
            {
                CasheMe ca = new CasheMe();
                ca.items.Add(new Item(sourcename, cb,this));
                mDic[path] = ca;
                LoadAsset(path);
            }
            else
            {
                mDic[path].items.Add(new Item(sourcename, cb,this));
            }
        }


        void LoadAsset(string path)
        {
            if (AssetBundleManager.Instance.mAssetBundleDic.ContainsKey(path))
            {
                AssetBundle ab = AssetBundleManager.Instance.GetAssetBundle(path);

                if (mDic[path].Load(ab))
                {
                    mDic.Remove(path);
                }
            }
            else
            {
                UnityMain.StartCoroutine(LoadBundleEnumerator(path));
            }
        }

        IEnumerator LoadBundleEnumerator(string path)
        {
            using (WWW www = new WWW(path))
            {
                yield return www;
                if (www.isDone)
                {
                    if ( AssetBundleManager.Instance.mAssetBundleDic.ContainsKey(path))
                    {
                        LoadAsset(path);
                        yield break;
                    }

                    AssetBundle ab = www.assetBundle;
                    if (null == ab)
                    {
                        DebugMod.LogError("www.assetBundle is null when load: " + path);
                        yield break;
                    }

                    AssetBundleManager.Instance.AddAssetBundle(path, ab);
                    LoadAsset(path);
                }
                else
                {
                    DebugMod.LogError(www.error);
                }
            }
        }


    }
}