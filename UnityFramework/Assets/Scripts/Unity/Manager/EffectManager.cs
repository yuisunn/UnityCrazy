using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using UnityEngine.Events;

namespace SPSGame.Unity
{
    public class EffectManager : Singleton<EffectManager>
    {

        Dictionary<string, U3dCache> mEffectDic = new Dictionary<string, U3dCache>();

        public void LoadEffect(string resfilename, UnityAction<U3DObject> cb)
        {
            if (mEffectDic.ContainsKey(resfilename))
            {
                U3DObject uob = mEffectDic[resfilename].SpawnOne();
                if( uob != null )
                {
                    cb(uob);
                }
                else
                {
                    mEffectDic.Remove(resfilename);
                    LoadEffect(resfilename, cb);
                }
            }
            else
            {
                string sourceName = PathMod.GetPureName(resfilename);
                AssetBundleManager.Instance.LoadEffect(resfilename, sourceName, (go) =>
                {
                    GameObject obj = U3DMod.Clone(go as GameObject);
                    CEffect eff = U3DMod.GetComponent<CEffect>(obj);
                    eff.Hide();
                    mEffectDic[resfilename] = new U3dCache(eff);
                    cb(mEffectDic[resfilename].SpawnOne());
                });
            }
        }
    }
}