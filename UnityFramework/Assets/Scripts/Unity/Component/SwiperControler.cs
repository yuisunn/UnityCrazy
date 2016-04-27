using UnityEngine;
using System.Collections;
using System;

namespace SPSGame.Unity
{

    public class SwiperControler:U3DComponent
    {

        public delegate Vector3 GetPosition();
        public delegate void SetPosition( Vector3 position );

        public enum ESwiperType
        {
            Default,
            Horizontal,
            Vertical,
            HorizontalAndVertical,
            AllDirection
        }

        public Action OnSwipTop;
        public Action OnSwipBottom;
        public Action OnSwipLeft;
        public Action OnSwipRight;

        //被移动对象
        //public GameObject mover;

        //滑动方向类型
        public ESwiperType swiperType = ESwiperType.Default;

        //贴住水平线
        public bool stickHorizontal = false;
        //贴住竖直线
        public bool stickVertical = false;

        private bool mSwiping = false;

        private float mViewDeepth = 5;

        //Z向景深
        public float viewDeepth
        {
            set
            {
                mViewDeepth = value;
            }
        }

        private ESwiperType mTempSwiperType = ESwiperType.Default;

        //3D主摄像机
        Camera m3DCamera;
        //滑动参照计算摄像机
        Camera mReferCamera;

        //原始位置
        Vector3 mOriginalPos;
        //移动者起始位置
        Vector3 mMoverStartPos;
        //触摸起始位置
        Vector3 mTouchStartPos;

        //移动者移动目标位置
        Vector3 mMoveToPos;
        //移动者当前位置
        Vector3 mCurrentPos;

        private float mTopY = 1000;
        private float mBottomY = -1000;
        private float mLeftX = -1000;
        private float mRightX = 1000;

        public SetPosition SetPositionHandler = null;
        public GetPosition GetPositionHandler = null;


        // Use this for initialization
        protected override void Start()
        {

            //mover = gameObject;

            if (EasyTouch.instance == null)
            {
                Debug.LogError("SwiperControler out of work,because EasyTouch not Create!");
                return;
            }

            EasyTouch.On_TouchStart += OnSwiperStart;
            EasyTouch.On_TouchDown += OnSwiperStay;
            EasyTouch.On_TouchUp += OnSwiperEnd;

            m3DCamera = EasyTouch.GetCamera();

            if (m3DCamera != null)
            {
                GameObject cmrobj = new GameObject("_Swipe RefCamera");
                Camera cmr = cmrobj.AddComponent<Camera>();
                cmr.clearFlags = CameraClearFlags.Nothing;
                cmr.cullingMask = 0;
                cmr.fieldOfView = m3DCamera.fieldOfView;
                cmrobj.transform.position = m3DCamera.transform.position;

                mReferCamera = cmr;
                DontDestroyOnLoad(mReferCamera);
            }
            else
            {
                Debug.LogError("Can't find 3d camera");
            }

            mOriginalPos = GetMoverPosition();

        }


        public void Init(Vector3 camerapos, float top, float bottom, float left, float right)
        {
            SetMoverPosition(camerapos);
            mOriginalPos = GetMoverPosition();

            mTopY = top;
            mBottomY = bottom;
            mLeftX = left;
            mRightX = right;

            OnSwipTop = () => { print("get top!"); };
        }


        /// <summary>
        /// 返回原始位置
        /// </summary>
        /// <returns></returns>
        public void ResumePosition()
        {
            SetMoverPosition(mOriginalPos);
        }

        protected override void OnDestroy()
        {
            EasyTouch.On_TouchStart -= OnSwiperStart;
            EasyTouch.On_TouchDown -= OnSwiperStay;
            EasyTouch.On_TouchUp -= OnSwiperEnd;

            if (mReferCamera != null)
                GameObject.Destroy(mReferCamera.gameObject);
        }


        void OnSwiperStart(Gesture ges)
        {
            mMoverStartPos = mCurrentPos;
            mTouchStartPos = GetTouchPos(mViewDeepth, ges.position, true);

            switch (swiperType)
            {
                case ESwiperType.Default:
                    break;
                case ESwiperType.Horizontal:
                    break;
                case ESwiperType.HorizontalAndVertical:
                    mTempSwiperType = ESwiperType.Default;
                    break;
                case ESwiperType.AllDirection:
                    break;
            }
        }


        void OnSwiperStay(Gesture ges)
        {
            UIJoyStick stick = UIManager.Instance.GetWindow<UIJoyStick>();
            if (stick!= null && stick.enable)
                return;

            Vector3 touchpos = GetTouchPos(mViewDeepth, ges.position, true);
            Vector3 dir = mTouchStartPos - touchpos;
            //Vector3 dir = mover == gameObject ? (mTouchStartPos - touchpos) : (touchpos - mTouchStartPos);
            dir.z = 0;

            switch (swiperType)
            {
                case ESwiperType.Horizontal:
                    dir.y = 0;
                    break;
                case ESwiperType.Vertical:
                    dir.x = 0;
                    break;
                case ESwiperType.HorizontalAndVertical:
                    if (mTempSwiperType == ESwiperType.Default)
                    {
                        if (Vector3.SqrMagnitude(dir) < mViewDeepth * 0.1f)
                        {
                            return;
                        }
                        else
                        {
                            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
                            {
                                mTempSwiperType = ESwiperType.Vertical;
                            }
                            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x))
                            {
                                mTempSwiperType = ESwiperType.Horizontal;
                            }
                            dir = Vector3.zero;
                        }
                    }

                    if (mTempSwiperType == ESwiperType.Horizontal)
                        dir.y = 0;
                    else if (mTempSwiperType == ESwiperType.Vertical)
                        dir.x = 0;
                    else
                        dir.y = dir.x = 0;

                    break;
                case ESwiperType.AllDirection:
                    break;
                case ESwiperType.Default:
                    return;
            }

            mMoveToPos = mMoverStartPos + dir;
            mSwiping = true;

            mMoverStartPos = mMoveToPos;
            mTouchStartPos = touchpos;
        }


        void OnSwiperEnd(Gesture ges)
        {
            mSwiping = false;
        }

        Vector3 GetTouchPos(float z, Vector2 position, bool worldZ = false)
        {
            if (!worldZ)
            {
                return mReferCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, z));
            }
            else
            {
                return mReferCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, z - m3DCamera.transform.position.z));
            }
        }

        Vector3 GetMoverPosition()
        {
            if (GetPositionHandler != null)
                return GetPositionHandler();

            return Vector3.zero;
            //return mover.transform.position;
        }

        void SetMoverPosition( Vector3 position )
        {
            if (SetPositionHandler != null)
                SetPositionHandler(position);
        }

        void CalculatePosition(Vector3 pos)
        {
            if (pos.y > mTopY)
            {
                if (!isGetTop() && OnSwipTop != null)
                    OnSwipTop();
                pos.y = mTopY;
            }

            if (pos.y < mBottomY)
            {
                if (!isGetBottom() && OnSwipBottom != null)
                    OnSwipBottom();
                pos.y = mBottomY;
            }

            if (pos.x < mLeftX)
            {
                if (!isGetLeft() && OnSwipLeft != null)
                    OnSwipLeft();
                pos.x = mLeftX;
            }

            if (pos.x > mRightX)
            {
                if (!isGetRight() && OnSwipRight != null)
                    OnSwipRight();
                pos.x = mRightX;
            }

            SetMoverPosition(pos);
//             if (mover.transform.position != pos)
//                 mover.transform.position = pos;
        }

        bool Near(float param1, float param2)
        {
            return Mathf.Abs(param1 - param2) < 0.01f;
        }

        bool isGetTop()
        {
            return GetMoverPosition().y >= mTopY;
        }

        bool isGetBottom()
        {
            return GetMoverPosition().y <= mBottomY;
        }

        bool isGetLeft()
        {
            return GetMoverPosition().x <= mLeftX;
        }

        bool isGetRight()
        {
            return GetMoverPosition().x >= mRightX;
        }


        public override void DestroyThisComponent()
        {
            U3DMod.Destroy(mReferCamera);
            base.DestroyThisComponent();
        }

        void LateUpdate()
        {
            mCurrentPos = GetMoverPosition();

            if (!mSwiping)
            {
                if (stickHorizontal && !Near(mCurrentPos.y, mOriginalPos.y))
                {
                    mCurrentPos = Vector3.Lerp(mCurrentPos, new Vector3(mCurrentPos.x, mOriginalPos.y, mCurrentPos.z), 10 * Time.deltaTime);
                }

                if (stickVertical && !Near(mCurrentPos.x, mOriginalPos.x))
                {
                    mCurrentPos = Vector3.Lerp(mCurrentPos, new Vector3(mOriginalPos.x, mCurrentPos.y, mCurrentPos.z), 10 * Time.deltaTime);
                }
            }
            else
            {
                mCurrentPos = Vector3.Lerp(mCurrentPos, mMoveToPos, 15 * Time.deltaTime);
            }

            CalculatePosition(mCurrentPos);
        }


    }
}