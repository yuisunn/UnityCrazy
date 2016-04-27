using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class CAnimator : U3DComponent
    {
        Animator mAnimator;
        AnimatorStateInfo mCurrentStateInfo;

        private UnityAction OnAttackPointHandle = null;

        public void OnAttackPoint(int param)
        {
            if (OnAttackPointHandle != null)
            {
                OnAttackPointHandle.ProcessAction();
                OnAttackPointHandle = null;
            }
        }


        /// <summary>
        /// 改变动画
        /// </summary>
        /// <param name="num">动画片段类型整型值</param>
        /// <param name="animspeed">播放动画速度</param>
        /// <param name="attackhandle">攻击点回调函数</param>
        /// <returns></returns>
        public void ChangeAnimation(int num, float animspeed = 1, UnityAction attackhandle = null)
        {
            EAnimType type = (EAnimType)num;
            ChangeAnimation(type, animspeed, attackhandle);
        }


        /// <summary>
        /// 改变动画
        /// </summary>
        /// <param name="type">动画片段类型</param>
        /// <param name="animspeed">播放动画速度</param>
        /// <param name="attackhandle">攻击点回调函数</param>
        /// <returns></returns>
        public void ChangeAnimation(EAnimType type, float animspeed = 1, UnityAction attackhandle = null)
        {
            if (mAnimator == null)
            {
                mAnimator = GetComponent<Animator>();
                if (mAnimator == null)
                {
                    Debug.LogError("No Animator attatched");
                    return;
                }
            }

            switch (type)
            {
                case EAnimType.idle:
                case EAnimType.walk:
                case EAnimType.run:
                case EAnimType.death:
                    mAnimator.SetInteger("state", (int)type);
                    break;

                case EAnimType.hurt:
                case EAnimType.action11:
                case EAnimType.action12:
                case EAnimType.action13:
                case EAnimType.action14:
                case EAnimType.action15:
                case EAnimType.action16:
                case EAnimType.action17:
                case EAnimType.action18:
                case EAnimType.action19:
                case EAnimType.action20:
                case EAnimType.action21:
                case EAnimType.action22:
                case EAnimType.action23:
                case EAnimType.action24:
                case EAnimType.action25:
                case EAnimType.action26:
                case EAnimType.action27:
                case EAnimType.action28:
                case EAnimType.action29:
                case EAnimType.action30:
                case EAnimType.action31:
                case EAnimType.action32:
                case EAnimType.action33:
                case EAnimType.action34:
                case EAnimType.action35:
                case EAnimType.action36:
                case EAnimType.action37:
                case EAnimType.action38:
                case EAnimType.action39:
                case EAnimType.action40:
                    mAnimator.SetInteger("state", 0);
                    mAnimator.SetTrigger(type.ToString());
                    break;

                default:
                    Debug.Log("can't find aniamtion type:" + type.ToString());
                    return;
            }
            mAnimator.speed = animspeed;
            OnAttackPointHandle = attackhandle;
        }

        // Update is called once per frame

    }
}
