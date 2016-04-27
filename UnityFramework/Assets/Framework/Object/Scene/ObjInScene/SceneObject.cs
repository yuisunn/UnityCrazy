using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.CsShare;
using SPSGame.GameModule;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class SceneObject
    {
        protected int mResID;
        public int ResID
        {
            get { return mResID; }
        }

        protected int mSpriteType = -1;
        public int SpriteType
        {
            get { return mSpriteType; }
        }

        protected object mInfo = null;

        private int mSpriteID;
        public int SpriteID
        {
            get { return mSpriteID; }
        }

        protected float mX;
        protected float mY;
        protected float mZ;
        public float X { get { return mX; } }
        public float Y { get { return mY; } }
        public float Z { get { return mZ; } }

        protected short mDirX;
        protected short mDirY;
        protected short mDirZ;
        public short DirX { get { return mDirX; } }
        public short DirY { get { return mDirY; } }
        public short DirZ { get { return mDirZ; } }

        protected short mDestX;
        protected short mDestY;
        protected short mDestZ;
        public short DestX { get { return mDestX; } }
        public short DestY { get { return mDestY; } }
        public short DestZ { get { return mDestZ; } }

        protected GameScene mSceneObj = null;
        public GameScene SceneObj { get { return mSceneObj; } }

        protected long mLastUpdateTime = 0;

        protected short mSpeed = 35;
        public virtual short Speed
        {
            get { return mSpeed; }
        }

        protected bool mInSight = false;
        protected long mTickSaw = 0;

        protected bool mLoaded = false;     //是否已经加载资源（但不一定显示）
        protected bool mVisible = true;     //是否允许显示（不允许的情况：1，隐身；2，还没从服务器收到必要的信息)
        protected bool mLastVisible = false;    //上一次Update时的显示状态
        protected bool mEnable = false;

        public bool Enable
        {
            get
            {
                return mEnable;
            }
        }

        public bool Visible
        {
            get
            {
                return mVisible;
            }
        }

        public void SetVisible(bool value)
        {
            mVisible = value;
        }

        public void SetEnable(bool value)
        {
            mEnable = value;
        }

        public bool LastVisible
        {
            get
            {
                return mLastVisible;
            }
        }

        public bool Loaded
        {
            get
            {
                return mLoaded;
            }
        }

        public SceneObject(int spriteid)
        {
            mSpriteID = spriteid;
            Clear();
        }

        public void SetPos(short x, short z)
        {
            mX = x;
            mZ = z;
        }

        public void SetPos(float x, float y, float z)
        {
            mX = x;
            mY = y;
            mZ = z;
        }

        public void SetSpeed(short speed)
        {
            mSpeed = speed;
        }

        public void SetSceneSeq(GameScene scene)
        {
            mSceneObj = scene;
            mEnable = true;
        }

        public virtual void SetInfo(object info)
        {
            mInfo = info;
        }

        public virtual void Clear()
        {
            mTickSaw = 0;
            mInSight = false;
            mVisible = true;
            mLastVisible = false;
            mLoaded = false;
            mEnable = false;
            mSceneObj = null;
            mX = 0.0f;
            mY = 0.0f;
            mZ = 0.0f;
            mDirX = 10;
            mDirY = 0;
            mDirZ = 0;
            mDestX = 0;
            mDestY = 0;
            mDestZ = 0;
            mInfo = null;
        }

        public virtual void OnInit()
        {

        }

        public bool ReCalcDir(short srcX, short srcZ, short destX, short destZ)
        {
            int diffX = destX - srcX;
            int diffZ = destZ - srcZ;
            if (diffX == 0 && diffZ == 0)
            {
                return false;
            }

            double dist = Math.Sqrt(diffX * diffX + diffZ * diffZ);
            mDirX = (short)(100 * diffX / dist);
            mDirZ = (short)(100 * diffZ / dist);
            return true;
        }

        public virtual void OnUpdate(long tickNow)
        {
        }

        protected bool CheckInSight(long tickNow)
        {
            if (null == mSceneObj)
                return false;
            if (Local.Instance.SceneMgr.MySceneSeq != mSceneObj.SeqId)
                return false;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            MyPlayerInfo myinfo = myplayer.MyInfo;

            int difY = (short)myplayer.Y - (short)mY;
            if (difY >= 5)
                return false;
            int difX = (short)myplayer.X - (short)mX;
            int difZ = (short)myplayer.Z - (short)mZ;
            return (difX * difX + difZ * difZ < 1000000);
        }

        public virtual bool IsPlayer() { return false; }
        public virtual long GetSeqID() { return 0;}

     }
}
