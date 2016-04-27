using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class GPlayer:GMonster
    {
        protected NavMeshAgent mRigid = null;

        float mTimer = 0f;

        Vector3 mLastPos = Vector3.zero;
        Vector3 mDestPos = Vector3.zero;

        float controlSpeed = 3.5f;
        bool controlAble = true;

        public void SetControlAble( bool iscontrol )
        {
            controlAble = iscontrol;
        }

        public void SetControlSpeed( float speed )
        {
            controlSpeed = speed;
        }

        protected override void OnLoadResComplete()
        {
            base.OnLoadResComplete();
            GameObject shadow = ResourceManager.New("Prefab/halogreen");
            U3DMod.AddChild(u3dObject, shadow);
            mRigid = U3DMod.GetComponent<NavMeshAgent>(u3dObject);

            u3dObject.transform.GetChild(0).gameObject.AddComponent<CModelRender>();
        }


        public override void RenderFixedUpdate()
        {
            //if( !mUnderControl )
                base.RenderFixedUpdate();
//             mTimer += Time.fixedDeltaTime;
//             if (mTimer >= .2f)
//             {
//                 Vector3 velocity = (position - mLatsPos) / mTimer;
//                 mDestPos = position + 5 * direction * velocity.magnitude;
//                 if (mModel != null) mModel.DrawMoveTo(mDestPos);
// 
//                 Logicer.MovePlayerTo(position.x, position.z, mDestPos.x, mDestPos.z);
// 
//                 mLatsPos = position;
//                 mTimer = 0f;
//             }
        }

        public override void RenderUpdate()
        {
            base.RenderUpdate();
        }


        public virtual void ControlMove(Vector2 direct,bool ontower,bool stop = false)
        {
            if (!isShow || mRigid == null || !controlAble )
                return;

            mRigid.velocity = new Vector3(direct.x, 0, direct.y) * controlSpeed;

            if (direct != Vector2.zero)
            {  
                 //mRigid.MoveRotation(Quaternion.Euler(0, MathMod.VectorToAngle(direct), 0));
                if (ontower)
                    mPostion.x = mRigid.transform.position.x;
                else
                    mPostion = mU3dObject.transform.position;

                mDirection = new Vector3(direct.x, 0, direct.y);
                mDestPostion = mPostion;
                mDestDirection = destDirection;

                mTimer += Time.fixedDeltaTime;
                if (mTimer >= .2f)
                {
//                    Vector3 velocity = (mDestPos - mLatsPos) / mTimer;
                    mDestPos = mPostion + 5 * direction.normalized * controlSpeed;
                    if (mModel != null) mModel.DrawMoveTo(mDestPos);

                    Logicer.MovePlayerTo(mPostion.x, mPostion.z, mDestPos.x, mDestPos.z);

                    mLastPos = mPostion;
                    mTimer = 0f;
                }

                ChangeAction(EAnimType.run);
            }
            else
            {
                if(stop)
                {
                    Logicer.StopPlayer(position.x, position.z);
                }                  

                ChangeAction(EAnimType.idle);
            }
        }

    }
}