//using UnityEngine;
//using System.Collections;

//namespace SLCGame.Unity
//{
//    public class TweenFlipCARDS : MonoBehaviour
//    {
//        public GameObject fanhui;
//        public GameObject positive;
//        public GameObject reverse;
//        private TweenRotation mPositiveTween;
//        private TweenRotation mReverseTween;
//        /// <summary> 半圈时间 </summary>
//        public float duration = 1;

//        void Start()
//        { 
//            mPositiveTween = positive.GetComponent<TweenRotation>();
//            if (mPositiveTween == null) mPositiveTween = positive.AddComponent<TweenRotation>();

//            mReverseTween = reverse.GetComponent<TweenRotation>();
//            if (mReverseTween == null) mReverseTween = reverse.AddComponent<TweenRotation>();
//            mPositiveTween.enabled = false;
//            mReverseTween.enabled = false;
//            reverse.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);

//            mPositiveTween.from = Vector3.zero;
//            mPositiveTween.to = new Vector3(0, 90, 0);
//            mPositiveTween.duration = duration;

//            mReverseTween.from = new Vector3(0, 90, 0);
//            mReverseTween.to = Vector3.zero;
//            mReverseTween.duration = duration;

//            UIEventListener listener = UIEventListener.Get(positive.gameObject);
//            listener.onClick = ClickUIButton;
//            listener = UIEventListener.Get(reverse.gameObject);
//            listener.onClick = ClickUIButton;

//            //listener = UIEventListener.Get(fanhui.gameObject);
//            //listener.onClick = ClickUIButton;

//            mPositiveTween.onFinished.Add(new EventDelegate(PositiveEventDelegate));
//            mReverseTween.onFinished.Add(new EventDelegate(ReverseEventDelegate));

//        }

//        GameObject mNowDown;
//        void ClickUIButton(GameObject click)
//        {
//            mNowDown = click;
//            if (click == positive)
//            {
//                PlayPositive();
//            }
//            else if (click == reverse)
//            {
//                PlayReverse();
//            }
//        }

//        public void ClickReverse()
//        {
//            mNowDown = reverse;
//            PlayReverse();
//        }


//        /// <summary>
//        /// 翻牌
//        /// </summary>
//        public void PlayPositive()
//        {
//            mPositiveTween.Play(true);
//        }
//        /// <summary>
//        /// 复位
//        /// </summary>
//        public void PlayReverse()
//        {
//            mReverseTween.Play(false);
//        }


//        /// <summary>
//        /// 翻牌回调
//        /// </summary>
//        void PositiveEventDelegate()
//        {
//            if (mNowDown == mPositiveTween.gameObject) mReverseTween.Play(true);
//        }
//        /// <summary>
//        /// 复位回调
//        /// </summary>
//        void ReverseEventDelegate()
//        {
//            if (mNowDown == mReverseTween.gameObject) mPositiveTween.Play(false);
//        }
//    }
//}