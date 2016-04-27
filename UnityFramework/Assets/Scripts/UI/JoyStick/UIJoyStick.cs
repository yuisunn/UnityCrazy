using UnityEngine;
using System.Collections;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class UIJoyStick : UIWndBase
    {

        #region Delegate
        public delegate void JoystickMoveStartHandler(MovingStick move);
        public delegate void JoystickMoveHandler(MovingStick move);
        public delegate void JoystickMoveEndHandler(MovingStick move);
        public delegate void JoystickTouchStartHandler(MovingStick move);
        public delegate void JoystickTapHandler(MovingStick move);
        public delegate void JoystickDoubleTapHandler(MovingStick move);
        public delegate void JoystickTouchUpHandler(MovingStick move);
        #endregion


        #region Event
        /// <summary>
        /// Occurs the joystick starts move.
        /// </summary>
        public event JoystickMoveStartHandler On_JoystickMoveStart;
        /// <summary>
        /// Occurs when the joystick move.
        /// </summary>
        public event JoystickMoveHandler On_JoystickMove;
        /// <summary>
        /// Occurs when the joystick stops move
        /// </summary>
        public event JoystickMoveEndHandler On_JoystickMoveEnd;
        /// <summary>
        /// Occurs when a touch start hover the joystick
        /// </summary>
        public event JoystickTouchStartHandler On_JoystickTouchStart;
        /// <summary>
        /// Occurs when a tap happen's on the joystick
        /// </summary>
        public event JoystickTapHandler On_JoystickTap;
        /// <summary>
        /// Occurs when a double tap happen's on the joystick
        /// </summary>
        public event JoystickDoubleTapHandler On_JoystickDoubleTap;
        /// <summary>
        /// Occurs when touch up happen's on the joystick
        /// </summary>
        public event JoystickTouchUpHandler On_JoystickTouchUp;
        #endregion


        private enum MessageName { On_JoystickMoveStart, On_JoystickTouchStart, On_JoystickTouchUp, On_JoystickMove, On_JoystickMoveEnd, On_JoystickTap, On_JoystickDoubleTap };

        //[SerializeField]
        private float zoneRadius = .1f;
        public float ZoneRadius
        {
            get
            {
                return this.zoneRadius;
            }
            set
            {
                zoneRadius = value;
            }
        }

        private Vector2 mStickAxis;
        public Vector2 joyStickAxis
        {
            get
            {
                return mStickAxis;
            }
        }

        private float mStickValue = 0f;


        private Vector2 mStickPosition;
        public Vector2 stickPosition
        {
            get
            {
                return mStickPosition;
            }
            set
            {
                Vector2 dir = value - mLocaltion;

                mStickAxis = dir.normalized;

                Vector2 offect = Vector2.ClampMagnitude(dir, zoneRadius);
                mStickValue = Vector2.Distance(offect,Vector2.zero);

                if (mType == EStickType.Corss)
                    offect.y = 0;
                mStickPosition = offect + mLocaltion;
            }
        }

        bool mEnable = true;
        public bool enable
        {
            get
            {
                return mEnable;
            }
            set
            {
                mEnable = value;
                U3DMod.SetActive(joyStickRoot, mEnable&&mType == EStickType.Stick);
                U3DMod.SetActive(corssRoot, mEnable && mType == EStickType.Corss); 
            }
        }

        bool selected = false;

        
        public bool isActivated = true;

        public GameObject joyStickRoot = null;
        public GameObject joyStick = null;

        public GameObject corssRoot = null;
        public GameObject corss = null;

        private Vector2 mLocaltion;

        public enum EStickType { Stick, Corss };
        EStickType mType = EStickType.Stick;

        protected override void Awake()
        {
            base.Awake();
            EasyTouch.On_TouchStart += OnTouchStart;
            EasyTouch.On_TouchUp += OnTouchUp;
            EasyTouch.On_TouchDown += OnTouchDown;
            EasyTouch.On_SimpleTap += OnSimpleTap;
        }

        protected override void Start()
        {
            base.Start();
            enable = false;
        }

        public override void Show( bool isshow )
        {

        }

        public void SetType( EStickType type )
        {
            mType = type;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EasyTouch.On_TouchStart -= OnTouchStart;
            EasyTouch.On_TouchUp -= OnTouchUp;
            EasyTouch.On_TouchDown -= OnTouchDown;
            EasyTouch.On_SimpleTap -= OnSimpleTap;
        }

        protected override void Update()
        {
            base.Update();

            transform.position = mLocaltion;
            joyStick.transform.position = stickPosition;
            corss.transform.position = stickPosition;
        }


        void OnTouchStart(Gesture gesture)
        {
//             if (gesture.pickObject == null || gesture.pickObject.layer != LayerMask.NameToLayer("Sprite"))
//                 enable = true;

            mLocaltion = UICamera.currentCamera.ScreenToWorldPoint(gesture.position);
            selected = false;
        }

        void OnTouchUp(Gesture gesture)
        {
            CreateEvent(MessageName.On_JoystickMoveEnd);
            enable = false;
        }

        void OnTouchDown(Gesture gesture)
        {

            stickPosition = UICamera.currentCamera.ScreenToWorldPoint(gesture.position);

            if( mType == EStickType.Stick )
            {
                if (mStickValue > 0.01f && !enable)
                    enable = true;
            }
            else
            {
                if (mStickValue >= .04f)
                    selected = true;

                if ( !selected && Mathf.Abs(mStickAxis.y) < Mathf.Abs(mStickAxis.x) && mStickValue > 0.01f && !enable)
                    enable = true;
            }

            CreateEvent(MessageName.On_JoystickMove);
        }

        void OnSimpleTap(Gesture gesture)
        {

        }

        void CreateEvent(MessageName message)
        {
            if (!enable)
                return;

            MovingStick move = new MovingStick();
            move.stickAxis = joyStickAxis;
            move.stickValue = mStickValue;

            switch (message)
            {
                case MessageName.On_JoystickMoveStart:
                    if (On_JoystickMoveStart != null)
                    {
                        On_JoystickMoveStart(move);
                    }
                    break;
                case MessageName.On_JoystickMove:
                    if (On_JoystickMove != null)
                    {
                        On_JoystickMove(move);
                    }
                    break;
                case MessageName.On_JoystickMoveEnd:
                    if (On_JoystickMoveEnd != null)
                    {
                        On_JoystickMoveEnd(move);
                    }
                    break;
                case MessageName.On_JoystickTouchStart:
                    if (On_JoystickTouchStart != null)
                    {
                        On_JoystickTouchStart(move);
                    }
                    break;
                case MessageName.On_JoystickTap:
                    if (On_JoystickTap != null)
                    {
                        On_JoystickTap(move);
                    }
                    break;

                case MessageName.On_JoystickDoubleTap:
                    if (On_JoystickDoubleTap != null)
                    {
                        On_JoystickDoubleTap(move);
                    }
                    break;
                case MessageName.On_JoystickTouchUp:
                    if (On_JoystickTouchUp != null)
                    {
                        On_JoystickTouchUp(move);
                    }
                    break;
            }
           
        }
	
    }
}