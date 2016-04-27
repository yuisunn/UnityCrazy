using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class CModel : U3DComponent
    {

        //模型节点
        protected Transform mHeadTrs;
        protected Transform mBackTrs;
        protected Transform mRootTrs;

        protected Transform mLHandTrs;
        protected Transform mRHandTrs;

        protected Transform mLShoulderTrs;
        protected Transform mRShoulderTrs;

        protected Transform mLFootTrs;
        protected Transform mRFootTrs;

        protected Transform mWeaponTrs;

        protected SkinnedMeshRenderer mMeshRender;


        List<CEffect> mEffect = new List<CEffect>();

        Dictionary<string, Transform> mBoneDic = new Dictionary<string, Transform>();

        public override void Init()
        {
            base.Init();

            mRootTrs = transform;
            mLHandTrs = U3DMod.FindChild(transform, "Bip001 L Hand").transform;
            mRHandTrs = U3DMod.FindChild(transform, "Bip001 R Hand").transform;
            mWeaponTrs = U3DMod.FindChild(transform, "Bip001 Prop2").transform;
        }

        protected override void OnInited()
        {
            base.OnInited();
        }

        public void SetEquipment(int modelid, int type, bool pair = false)
        {

        }

        public void ShowWeapon(int pos, bool isVisible)
        {

        }

        public void SetVisible(bool isvisible)
        {

        }

        public Transform GetBone( string bonename )
        {
            if( mBoneDic.ContainsKey(bonename) )
            {
                return mBoneDic[bonename];
            }
            else
            {
                GameObject bone = U3DMod.FindChild(transform, bonename);
                if( bone != null )
                {
                    mBoneDic[bonename] = bone.transform;
                    return GetBone(bonename);
                }
            }
            return null;
        }

        public void PlayEffect( int resid,int actid )
        {

//             if( datas.Count != 0 )
//             {
//                 string effectname = "";
//                 bool isloop = false;
//                 if (datas[0].ContainsKey("EffectName"))
//                     effectname = datas[0]["EffectName"];
//                 else
//                     DebugMod.LogError(string.Format("can't find effect for Model{0} on action {1}", resid, actid));
//                 if (datas[0].ContainsKey("Loop"))
//                 {
//                     string str= datas[0]["Loop"];
//                     isloop = str =="1"?true:false;
//                 }
// 
//                 if( !string.IsNullOrEmpty(effectname) )
//                 {
//                     LoadEffect(effectname, 2);
//                 }
//             }
        }

        void LoadEffect(string sourceName, float showtime = 0)
        {
            string resname = sourceName + ".unity3d";

            EffectManager.Instance.LoadEffect(resname, (uobj) => 
            {
                int index = sourceName.LastIndexOf("_");
                string bone = sourceName.Substring(index + 1);
                if (!string.IsNullOrEmpty(bone))
                {
                    GameObject hang = U3DMod.FindChild(transform, bone);
                    if (hang != null)
                    {
                        U3DMod.AddChild(hang, uobj.gameObject, false);
                    }
                }
                else
                {
                    U3DMod.AddChild(gameObject, uobj.gameObject, false);
                }

                if (showtime != 0f)
                {
                    UnityMain.Instance.Timers.AddTimer(uobj.Despawn, showtime);
                }                   
                else
                {
                    mEffect.Add((CEffect)uobj);
                }   
            
            });
            
        }


        #region Debug
        Vector3 movetopos = Vector3.zero;
        public void DrawMoveTo(Vector3 pos)
        {
            movetopos = pos;
        }


        void OnDrawGizmos()
        {
            if(movetopos != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(movetopos, Vector3.one);
            }
                
        }
        #endregion Debug


    }
}