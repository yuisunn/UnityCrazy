using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class CPreviewAnimator : U3DComponent
    {
        Animator mAnimator;

        protected override void Start()
        {
            base.Start();
            mAnimator = GetComponent<Animator>();
        }

        public void Stand(bool isstand)
        {
            mAnimator.SetBool("stand", isstand);
        }

        public void RandomAct()
        {
            mAnimator.SetBool("action" + Random.Range(1, 6), true);
        }
    }
}