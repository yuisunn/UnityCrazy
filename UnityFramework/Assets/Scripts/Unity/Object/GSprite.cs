using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class GSprite : GObject
    {
        #region 二级属性
        public int spriteID = -1;
        public ESpriteType spriteType;
        public EAnimType action = EAnimType.idle;
        public float actionSpeed = 1;
        public SpriteInfo spriteInfo = null;
        public float hudHeight = 0;
        #endregion 二级属性

        #region 三级属性
        protected CModel mModel = null;
        protected CAnimator mAnim = null;
        protected UIHudHarm mHarmUI = null;
        #endregion 三级属性


        public override bool isShow{
            get{
                return mIsShow;
            }
            set{
                mIsShow = value;
                if (u3dObject != null)
                    U3DMod.SetActive(u3dObject, mIsShow);

                if (mIsShow && mAnim != null)
                    mAnim.ChangeAnimation(action, actionSpeed);
            }
        }

        public CalculatePositionDelegate CalculatePositionHandler = null;
        public virtual Vector3 viewPosition{
            get{
                if (CalculatePositionHandler == null)
                    return mPostion;
                else
                    return CalculatePositionHandler(mPostion);
            }
        }

        public virtual Vector3 hurtPosition{
            get{
                if (mModel == null)
                    return Vector3.zero;

                Transform hurtbone = mModel.GetBone("Bip001 Spine1");
                if (hurtbone == null)
                {
                    DebugMod.LogError("can't find bone");
                    return Vector3.zero;
                }                
                return hurtbone.transform.position;
            }
        }

        public virtual Vector3 firePosition
        {
            get
            {
                Transform firebone = mModel.GetBone("Bip001 Spine2");
                if (firebone == null)
                {
                    DebugMod.LogError("can't find bone");
                    return Vector3.zero;
                }
                return firebone.transform.position;
            }
        }

        public Transform GetBoneTransform( string bonename )
        {
            return mModel.GetBone(bonename);
        }

        protected override void OnLoadResComplete()
        {
            mU3dObject.transform.position = viewPosition;
            mU3dObject.transform.forward = direction;

//            mRigid = U3DMod.GetComponent<NavMeshAgent>(mU3dObject);
//             mRigid.useGravity = false;
//             mRigid.freezeRotation = true;

            mU3dObject.layer = LayerMask.NameToLayer("Sprite");

            if (spriteType == ESpriteType.Player)
            {
                U3DMod.GetComponent<NotDestroy>(mU3dObject);
            }

            mAnim = U3DMod.GetComponent<CAnimator>(u3dObject);
            mModel = U3DMod.GetComponent<CModel>(u3dObject);
            mAnim.ChangeAnimation(action, actionSpeed);

            U3DMod.SetActive(mU3dObject, isShow);
        }

        public override void SetUp3DRes()
        {
            if (u3dObject != null)
            {
                if (u3dObject.transform.position != viewPosition)
                    u3dObject.transform.position = viewPosition;

                u3dObject.transform.forward = direction;
            }
        }

        public override void RenderUpdate()
        {
            if (!isShow)
                return;

            if (Vector3.Magnitude(mPostion - mDestPostion) > moveSpeed * Time.deltaTime)
            {
                mPostion += (mDestPostion - mPostion).normalized * moveSpeed * Time.deltaTime;
            }
        }

        public virtual void SetSpriteInfo( SpriteInfo info )
        {
            spriteInfo = info;
        }

        public virtual void ShowInfoUI( bool isshow )
        {

        }

        public virtual void ChangeAction( EAnimType _action,float _speed = 1 )
        {
            action = _action;
            actionSpeed = _speed;

            if ( isShow && mAnim != null)
                mAnim.ChangeAnimation(action, actionSpeed);

            if (((int)_action >= 31 && (int)_action <= 40) || ((int)_action >= 21 && (int)_action <= 25))
            {
                if( mModel != null )
                    mModel.PlayEffect(resID, (int)_action);
                else
                    DebugMod.LogWarning("Model component is null");
            }
        }

        public virtual void OnHurt( EHurtType type,int number )
        {
            if( mHarmUI == null && u3dObject != null )
            {
                mHarmUI = UIManager.Instance.CreateHud<UIHudHarm>();
                mHarmUI.Init(u3dObject, hudHeight);

                UIFollowTarget flw = mHarmUI.gameObject.AddComponent<UIFollowTarget>();
                flw.target = u3dObject.transform;
            }
                
            if( mHarmUI != null)
                mHarmUI.HurtNumber(type, number);
        }
    }
}