using UnityEngine;
using System.Collections.Generic;

using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class GCreature : GObject
    {
        public int creatureID;

        
        public float maxlife = -1;
        protected float mLife = -1;
        

        public string[] GetSourceNames()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["ResID"] = resID.ToString();
            List<Dictionary<string, string>> datas = DataManager.Instance.GetEffectRes(dic);
            string source = "";
            if (datas.Count > 0 && datas[0].ContainsKey("特效名称"))
            {
                source = datas[0]["特效名称"];
            }

            if (string.IsNullOrEmpty(source))
            {
                DebugMod.LogError("Get no effect res in id:" + resID);
                return null;
            }

            string[] sources = source.Split(';');

            if (sources.Length == 0)
                DebugMod.LogError("Get no effect res in id:" + resID);

            return sources;
        }

        public override void RenderUpdate()
        {
            if (!isShow || maxlife == -1)
                return;

            mLife += Time.deltaTime;
            if (mLife >= maxlife)
                Destroy();
        }


    }
}