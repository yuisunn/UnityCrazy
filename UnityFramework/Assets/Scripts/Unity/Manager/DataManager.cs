using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class DataManager : Singleton<DataManager>
    {
        AssetBundle mConfigBundle = null;
        DataUnit mModelData = null;
        DataUnit mSceneData = null;
        CSVMod mEffectResData = null;
        public CSVMod mIconData = null;
        public void Init()
        {
//             AssetBundleManager.Instance.LoadConfig("serverconfig.unity3d",(o) =>
//             {
//                 mConfigBundle = o;
// 
//                 LogicMain.Instance.OnDataReady();
//             });

            TextAsset roles = ResourceManager.Load<TextAsset>("Config/Models_Client");
            mModelData = new DataUnit(roles.text);

            TextAsset scene = ResourceManager.Load<TextAsset>("Config/Scene");
            mSceneData = new DataUnit(scene.text);

            TextAsset effect = ResourceManager.Load<TextAsset>("Config/EffectRes_Client");
            mEffectResData = new CSVMod(effect.text);
            if (!mEffectResData.LoadCsvStr())
                DebugMod.Log("Load effectdata error!");

            TextAsset icons = ResourceManager.Load<TextAsset>("Config/Icons");
            mIconData = new CSVMod(icons.text);
            if (!mIconData.LoadCsvStr())
                DebugMod.Log("Load icons error!");

            LogicMain.Instance.OnDataReady();
             
        }

        public string GetModelData( int modelid,string key )
        {
            string res = mModelData.GetDataById(modelid, key);
            if (string.IsNullOrEmpty(res))
            {
                DebugMod.LogError( string.Format( "can't find Model data {0} from id {1}",key,modelid));
            }
            return res;
        }

        public string GetModelName(int id)
        {
           string res = mModelData.GetDataById(id, "Name");
           if (string.IsNullOrEmpty(res))
           {
               DebugMod.LogError("can't find ModelName from id " + id);
           }
           return res;
        }

        public string GetModelRes( int id )
        {
            string res = mModelData.GetDataById(id, "ResName");
            if (string.IsNullOrEmpty(res))
            {
                DebugMod.LogError("can't find ModelRes from id " + id);
            }
            return res;
        }


        public string GetSceneRes(int id)
        {
            return mSceneData.GetDataById(id, "资源名");
        }

        public ESceneType GetSceneType(int id)
        {
            string typestr = mSceneData.GetDataById(id, "类型");
            int typeint = -1;
            if(int.TryParse(typestr,out typeint))
            {
                return (ESceneType)typeint;
            }
            return ESceneType.UnKnown;
        }


        public List<Dictionary<string, string>> GetEffectRes(Dictionary<string, string> keys)
        {
            return mEffectResData.GetRowDataByMultiColValue(keys);
        }


        public byte[] GetConfigBytesData( string sourcename )
        {
            
            TextAsset conf =  Resources.Load<TextAsset>(string.Format("Config/{0}",sourcename));
            if (conf != null)
                return conf.bytes;

//             if (mConfigBundle != null)
//             {
//                 Object obj = mConfigBundle.LoadAsset(sourcename);
//                 if (obj != null)
//                 {
//                     TextAsset ta = obj as TextAsset;
//                     if (ta != null)
//                         return ta.bytes;
//                 }
//                 
//             }
            return null;
        }

        public string GetConfigStringData(string sourcename)
        {
            TextAsset conf = Resources.Load<TextAsset>(string.Format("Config/{0}", sourcename));
            if (conf != null)
                return conf.text;

//             if (mConfigBundle != null)
//             {
//                 Object obj = mConfigBundle.LoadAsset(sourcename);
//                 if (obj != null)
//                 {
//                     TextAsset ta = obj as TextAsset;
//                     if( ta != null )
//                         return ta.text;
//                 }
// 
//             }
            return null;
        }

        public TextAsset GetConfigTextData(string sourcename)
        {
            if (mConfigBundle != null)
            {
                Object obj = mConfigBundle.LoadAsset(sourcename);
                if (obj != null)
                {
                    TextAsset ta = obj as TextAsset;
                    if (ta != null)
                        return ta;
                }

            }
            return null;
        }


    }
}