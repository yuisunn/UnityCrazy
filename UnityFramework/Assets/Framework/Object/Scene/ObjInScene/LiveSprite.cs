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
    public class LiveSprite : SceneObject
    {
        protected long mLastPlayHurtTime = 0;
        protected bool mDead;
        public bool IsLive { get { return !mDead; } }

        protected bool mReqInfo = false;

        protected bool mShowHud = false;
        protected long mShowHudTime = 0;

        protected long mLoadSpriteTime = 0;

        protected int mHurtActCooldown = 500;

        protected bool mLoadFinish = false;

        protected PlayerSkillSingLogic mSingLogic = null;
        protected PlayerSkillEffectLogic mEffectLogic = null;

        public virtual PlayerSkillSingLogic SingLogic { get { return mSingLogic; } }
        public virtual PlayerSkillEffectLogic EffectLogic { get { return mEffectLogic; } }

        public LiveSprite(int spriteid)
            :base(spriteid)
        {
        }

        public override void OnInit()
        {
            mSingLogic = new PlayerSkillSingLogic(this);
            mEffectLogic = new PlayerSkillEffectLogic(this);
        }

        public override void Clear()
        {
            base.Clear();

            mDead = false;
            mReqInfo = false;
            mShowHud = false;
            mShowHudTime = 0;
            mLastPlayHurtTime = 0;
            mLoadFinish = false;
            mLoadSpriteTime = 0;
        }


        public void KillSprite()
        {
            if (0 == this.SpriteID)
                return;

            if (!mLoaded)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            Local.Instance.CallUnityAction(UnityActionDefine.KillSprite, param);

            mLoaded = false;
            mLastVisible = false;
            mLoadSpriteTime = 0;
            mLoadFinish = false;
            mShowHud = false;
            mShowHudTime = 0;
        }

        public bool IfLoadFinish(long tickNow)
        {
            if (mLoadFinish)
                return true;
            if (0 == mLoadSpriteTime)
                return false;
            else if (tickNow > mLoadSpriteTime + 500)
            {
                mLoadFinish = true;
                return true;
            }
            else
                return false;
        }

        public void OnDisapear()
        {
            SetVisible(false);
            ShowSprite(false);
            ShowHud(false, 0);
        }

        public void ShowSprite(bool show)
        {
            if (mLastVisible == show)
                return;
            if (!mLoaded)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["Show"] = show ? 1 : 0;
            Local.Instance.CallUnityAction(UnityActionDefine.ShowSprite, param);
            this.mLastVisible = show;
        }

        public void UpdateMove(long tickPass)
        {
            if (mDestX == (short)mX && mDestZ == (short)mZ)
                return;

            double speed = Speed * 0.001f;
            double disPass = speed * tickPass;
            int diffX = mDestX - (short)mX;
            int diffZ = mDestZ - (short)mZ;
            double disDest = Math.Sqrt(diffX * diffX + diffZ * diffZ);

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();

            float newX, newZ;

            if (disPass >= disDest)
            {
                //移动到终点
                newX = mDestX;
                newZ = mDestZ;
            }
            else
            {
                newX = (float)(mX + disPass * diffX / disDest);
                newZ = (float)(mZ + disPass * diffZ / disDest);
            }

            if (newX > scene.Width)
            {
                newX = mDestX = scene.Width;
                mDestZ = (short)newZ;
            }
            else if (newX < -scene.Width)
            {
                newX = mDestX = (short)(-scene.Width);
                mDestZ = (short)newZ;
            }

            if (newZ > scene.Height)
            {
                newZ = mDestZ = scene.Height;
                mDestX = (short)newX;
            }
            else if (newZ < -scene.Height)
            {
                newZ = mDestZ = (short)(-scene.Height);
                mDestX = (short)newX;
            }

            SetPos(newX, mY, newZ);
        }

        public void StopMove(EAnimType actid = EAnimType.idle)
        {
            mDestX = (short)mX;
            mDestY = (short)mY;
            mDestZ = (short)mZ;

            if (!mLoaded)
                return;

            PlayAct(actid);

            MoveSprite((short)(Speed * 3));
        }

        public void PlayAct(EAnimType actid)
        {
            if (!mLoaded)
                return;

            if (mDead && actid != EAnimType.death)
            {
                DebugMod.Log("死亡时不应播放其它动作");
                return;
            }

            if (null != SingLogic)
            {
                if (SingLogic.IsSinging() && actid == EAnimType.idle)
                    return;
                if (SingLogic.IsSinging() && actid == EAnimType.hurt)
                    return;
            }

            if (null != EffectLogic)
            {
                if (EffectLogic.IsPlaySkill() && actid == EAnimType.idle)
                    return;
                if (EffectLogic.IsPlaySkill() && actid == EAnimType.hurt)
                    return;
            }

            CheckActionBreaking(actid);

            ActionParam param = new ActionParam();

            param["SpriteID"] = this.SpriteID;
            param["Speed"] = 100;
            param["ActID"] = (int)actid;
            Local.Instance.CallUnityAction(UnityActionDefine.ActSprite, param);
        }

        public void BeatToPos(short x, short z, short speed)
        {
            mDestX = x;
            mDestZ = z;
            MoveSprite(speed);
        }

        public void LocatSprite(short x, short y, short z)
        {
            SetPos(x, y, z);
            mDestX = x;
            mDestY = y;
            mDestZ = z;

            if (!mLoaded)
                return;
            if (null == mSceneObj)
                return;

            ActionParam param = new ActionParam();

            param["SpriteID"] = this.SpriteID;
            param["PosX"] = mDestX;
            param["PosY"] = mDestY;
            param["PosZ"] = mSceneObj.IsTower ? (mDestZ + 1) * 10 : mDestZ;
            param["DirX"] = (mSceneObj.IsTower && 0 == mDirX) ? 10 : mDirX;
            param["DirY"] = mDirY;
            param["DirZ"] = mDirZ;
            Local.Instance.CallUnityAction(UnityActionDefine.LocatSprite, param);
        }

        public void FastMoveTo(short destX, short destZ, short dirX, short dirZ, int actid, int speed=400)
        {
            //DebugMod.LogWarning(string.Format("fast move to {0}:{1}:{2}", destX, destZ, actid));
            mDestX = destX;
            mDestZ = destZ;
            mX = destX;
            mZ = destZ;
            mDirX = dirX;
            mDirZ = dirZ;
            //LocatSprite(mDestX, mDestY, mDestZ);
            MoveSprite((short)speed);
            if (actid > 0)
                PlayAct((EAnimType)actid);
        }

        public void ForceSetPos(short x, short y, short z, bool visible)
        {
            SetVisible(visible);
            LocatSprite(x, y, z);
        }

        public void SetMoveDest(short x, short z, bool MoveRightNow = false)
        {
            mDestX = x;
            mDestZ = z;

            ReCalcDir((short)mX, (short)mZ, mDestX, mDestZ);

            if (MoveRightNow)
            {
                MoveToDest();
            }
        }

        public void MoveSprite(short speed = -1)
        {
            if (!mLoaded)
                return;

            if (speed < 0)
                speed = Speed;

            if (!mDead && speed == Speed)
            {
                if (speed >= 20)
                    PlayAct(EAnimType.run);
                else
                    PlayAct(EAnimType.walk);
            }

            ActionParam param = new ActionParam();

            param["SpriteID"] = this.SpriteID;
            param["Speed"] = speed;
            param["PosX"] = mDestX;
            param["PosY"] = (short)mY;
            param["PosZ"] = mSceneObj.IsTower ? (mDestZ + 1) * 10 : mDestZ;
            param["DirX"] = (mSceneObj.IsTower && 0 == mDirX) ? 10 : mDirX;
            param["DirY"] = mDirY;
            param["DirZ"] = mDirZ;
            Local.Instance.CallUnityAction(UnityActionDefine.MoveSprite, param);

        }

        protected void MoveToDest()
        {
            if (mDead)
            {
                DebugMod.Log("dead MoveToDest");
                return;
            }

            int diffX = mDestX - (short)mX;
            int diffZ = mDestZ - (short)mZ;
            if (diffX == 0 && diffZ == 0)
            {
                return;
            }

            if (Speed >= 20)
                PlayAct(EAnimType.run);
            else
                PlayAct(EAnimType.walk);

            MoveSprite(Speed);
        }

        protected void InSight(long tickNow)
        {
            if (mTickSaw == 0)
                mTickSaw = tickNow;
            LoadSprite();
        }

        protected void OutSight(long tickNow)
        {
            mTickSaw = 0;
            KillSprite();
        }

        public bool ReqInfoIfNeed(long tickNow)
        {
            if (mReqInfo)
                return false;
            //insight 超过xx毫秒
            if (mInfo != null)
                return false;
            if (mTickSaw == 0)
                return false;

            //200ms
            if (tickNow - mTickSaw < 200)
                return false;

            mReqInfo = true;
            return true;
        }

        public override void OnUpdate(long tickNow)
        {
            long tickPass = tickNow - mLastUpdateTime;
            if (tickPass < 30)
                return;
            mLastUpdateTime = tickNow;

            if (!mLoadFinish)
            {
                if (IfLoadFinish(tickNow))
                {

                }
            }

            if (CheckInSight(tickNow))
            {
                InSight(tickNow);
            }
            else
            {
                OutSight(tickNow);
            }

            UpdateShow(tickNow);
            UpdateMove(tickPass);

            if (IfLoadFinish(tickNow))
            {
                if (this.SingLogic != null)
                {
                    this.SingLogic.Update(tickNow);
                }

                if (this.EffectLogic != null)
                {
                    this.EffectLogic.Update(tickNow);
                }
            }
        }

        protected virtual void UpdateShow(long tickNow)
        {
            if (!mLoaded)
                return;

            if (mLastVisible != mVisible)
            {
                ShowSprite(mVisible);
            }

            if (mVisible)
            {
                if (tickNow > mShowHudTime + 5000)
                    ShowHud(false, tickNow);
            }
        }

        public virtual void AfterSpriteLoad()
        {
            MoveToDest();
        }

        public virtual void LoadSprite()
        {
            if (mLoaded)
                return;
            if (mInfo == null)      //收到info后才加载资源
                return;
            if (!mEnable)
                return;
            if (0 == this.SpriteID)
                return;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;

            if (mY > scene.Layer || mY < -scene.Layer)
                return;
            if (mX > scene.Width || mX < -scene.Width)
                return;
            if (mZ > scene.Height || mZ < -scene.Height)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["ResID"] = mResID;
            param["Show"] = mVisible ? 1 : 0;
            param["SpriteTypeID"] = mSpriteType;
            param["PosX"] = (short)mX;
            param["PosY"] = (short)mY;
            param["PosZ"] = (short)(mSceneObj.IsTower ? (mZ + 1) * 10 : mZ);
            param["DirX"] = (mSceneObj.IsTower && 0 == mDirX) ? 10 : mDirX;
            param["DirY"] = mDirY;
            param["DirZ"] = mDirZ;
            Local.Instance.CallUnityAction(UnityActionDefine.LoadSprite, param);

            mLoadSpriteTime = DateTime.Now.Ticks / 10000;
            mLoadFinish = false;
            this.mLoaded = true;
            this.mLastVisible = mVisible;

            if (mDead)
            {
                PlayAct(EAnimType.death);
            }

            UpdateSpriteInfo();
            ShowHud(false, mLoadSpriteTime);
            AfterSpriteLoad();
        }

        public virtual void ChangeHp(int newHp)
        {

        }

        public void PlayHurt(int hurt, HurtType hurttype, EAnimType actid = EAnimType.hurt)
        {
            if (hurt == 0 && (hurttype == HurtType.Normal || hurttype == HurtType.Crit))
                return;

            EHurtType etype = EHurtType.Normal;
            if (hurttype == HurtType.Miss)
                etype = EHurtType.Miss;
            else if (hurttype == HurtType.Crit)
                etype = EHurtType.Crit;

            long tickNow = DateTime.Now.Ticks / 10000;

            PlayHurtSprite(hurt, (int)etype);
            ShowHud(true, tickNow);

            if (hurttype == HurtType.Dead)
            {
                UpdateSpriteInfo();
                ToDead();
            }
            else if (hurttype == HurtType.Miss)
            {

            }
            else
            {
                UpdateSpriteInfo();
                if (tickNow - mLastPlayHurtTime > mHurtActCooldown)
                {
                    PlayAct(actid);
                    mLastPlayHurtTime = tickNow;
                }
            }
        }

        public virtual void ToDead()
        {
            if (!mDead)
            {
                mDead = true;
                StopMove(EAnimType.death);
                ShowHud(false, 0);
            }
        }

        private void PlayHurtSprite(int hurt, int hurttype)
        {
            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["HurtType"] = hurttype;
            param["HurtNumber"] = hurt;
            Local.Instance.CallUnityAction(UnityActionDefine.HurtSprite, param);
        }

        public virtual void UpdateSpriteInfo()
        {
        }

        public void ShowHud(bool show, long tickNow)
        {
            mShowHudTime = tickNow;
            if (show == mShowHud)
                return;
            mShowHud = show;
            ActionParam param = new ActionParam();
            param["ShowHud"] = show;
            param["SpriteID"] = this.SpriteID;
            Local.Instance.CallUnityAction(UnityActionDefine.ControlSpriteHud, param);
        }

        public MovePoint    CalcPointByDir(MoveEffectType movetype, int distance, SceneObject target=null)
        {
            if (this.SceneObj == null)
                return null;

            int dirX = this.DirX;
            int dirZ = this.DirZ;
            int moveDis = distance;

            double disDiff = 0.0;
            if (null != target)
            {
                float diffX = target.X - this.X;
                float diffZ = target.Z - this.Z;
                disDiff = Math.Sqrt(diffX * diffX + diffZ * diffZ);
                if (disDiff >= 1)
                {
                    dirX = (int)(diffX / disDiff * 100);
                    dirZ = (int)(diffZ / disDiff * 100);
                }
                else
                {
                    dirX = -dirX;
                    dirZ = -dirZ;
                }
            }

            if (movetype == MoveEffectType.Target_Front)
            {
                moveDis = (int)(disDiff - distance);
                if (moveDis < 0)
                    moveDis = 0;
            }
            else if (movetype == MoveEffectType.Target_Back)
            {
                moveDis = (int)(disDiff + distance);
            }
            else if (movetype == MoveEffectType.Foward)
            {

            }
            else if (movetype == MoveEffectType.Backward)
            {
                dirX = -dirX;
                dirZ = -dirZ;
            }
            else
            {
                return null;
            }

            MovePoint point = new MovePoint();
            point.X = (short)(this.X + dirX * moveDis / 100);
            point.Z = (this.SceneObj.SceneType != Unity.ESceneType.Tower) ?
                (short)(this.Z + dirZ * moveDis / 100) : (short)(this.Z);
            return point;
        }

        public SfxObj PlaySfx(int resId, int lifetime)
        {
            if (null == this.SceneObj)
                return null;

            SfxObjData sfxdata = new SfxObjData();
            sfxdata.lifetime = lifetime;
            sfxdata.ResID = resId;
            sfxdata.ShowTick = DateTime.Now.Ticks / 10000;
            sfxdata.srcSpriteID = this.SpriteID;
            sfxdata.creatureType = (int)Unity.ECreatureType.Hang;
            return this.SceneObj.sfxManager.CreateSfxObj(sfxdata);
        }

        protected virtual void CheckActionBreaking(EAnimType newAct)
        {
            if (SingLogic != null && SingLogic.IsSinging())
            {
                SingLogic.CheckActionBreaking(newAct);
            }

            if (EffectLogic != null && EffectLogic.IsPlaySkill())
            {
                EffectLogic.CheckActionBreaking(newAct);
            }
        }
    }
}
