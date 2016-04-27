using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class GCreatureHang : GCreature
    {
        protected List<GameObject> u3dObjectList = new List<GameObject>();
        public GSprite sprite = null;

        protected override void Load3DRes()
        {
            if (sprite.u3dObject == null)
                return;

            if (resID == -1)
            {
                DebugMod.LogError("GObject has no res id");
                return;
            }

            string[] sources = GetSourceNames();

            

            for (int i = 0; i < sources.Length; ++i)
            {
                string resname = null;
                string sourcename = null;

                sourcename = sources[i];
                resname = sourcename + ".unity3d";

                //DebugMod.LogWarning("Play Effect Hang id:" + resID+ ",name:"+ sourcename);

                EffectManager.Instance.LoadEffect(resname, (uobj) =>
                {
                    int index = sourcename.LastIndexOf("_");
                    string bone = sourcename.Substring(index + 1);
                    Transform hangon = null;
                    if (!string.IsNullOrEmpty(bone))
                    {
                        hangon = sprite.GetBoneTransform(bone);
                    }

                    if (hangon == null)
                        hangon = sprite.u3dObject.transform;

                    U3DMod.AddChild(hangon, uobj.gameObject);
                    //uobj.gameObject.transform.parent = null;
                    u3dObjectList.Add(uobj.gameObject);
                    mPostion = hangon.transform.position;
                    uobj.Show();
                });
            }
        }

        public override void Destroy()
        {
            if (u3dObjectList != null)
            {
                while (u3dObjectList.Count > 0)
                {
                    if (u3dObjectList[0] != null)
                    {
                        CEffect eff = U3DMod.GetComponent<CEffect>(u3dObjectList[0]);
                        eff.Despawn();
                    }
                    u3dObjectList.RemoveAt(0);
                }
            }

            RenderManager.Instance.Remove(this);
        }
    }
}