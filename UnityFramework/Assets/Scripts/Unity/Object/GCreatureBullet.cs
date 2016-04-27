using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class GCreatureBullet : GCreature
    {
        public GSprite sprite= null;
        public GSprite target = null;

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


            string resname = null;
            string sourcename = null;
            sourcename = sources[0];
            resname = sourcename + ".unity3d";

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

                uobj.gameObject.transform.parent = null;
                mPostion = hangon.transform.position;
                uobj.gameObject.transform.rotation = sprite.u3dObject.transform.rotation;
                mU3dObject = uobj.gameObject;
                uobj.Show();
            });
            
        }



        public override void RenderUpdate()
        {
            if (!isShow)
                return;

            if (target == null)
                Destroy();

            if (target.u3dObject == null)
                return;

            if (Vector3.Magnitude(position - target.hurtPosition) > moveSpeed * Time.deltaTime)
            {
                mPostion += (target.hurtPosition - mPostion).normalized * moveSpeed * Time.deltaTime;
                mDirection = (target.hurtPosition - mPostion).normalized;
            }
            else
            {
                Logicer.HitSprite(creatureID, target.spriteID);
                Destroy();
            }
        }

        public override void Destroy()
        {
            if (u3dObject != null)
            {
                CEffect eff = U3DMod.GetComponent<CEffect>(u3dObject);
                if(eff != null)
                    eff.Despawn();
                else
                    U3DMod.Destroy(u3dObject);
            }

            RenderManager.Instance.Remove(this);
        }
    }
}