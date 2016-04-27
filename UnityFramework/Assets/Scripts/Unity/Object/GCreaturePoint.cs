using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class GCreaturePoint : GCreature
    {

        protected override void Load3DRes()
        {
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

                EffectManager.Instance.LoadEffect(resname, (uobj) =>
                {
                    mU3dObject = uobj.gameObject;
                    mU3dObject.transform.position = position;
                });
            }
        }

    }
}