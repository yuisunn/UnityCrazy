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
    public class PlayerObj : LiveSprite
    {
        protected int mUserId;
        public int UserId { get { return mUserId; } }

        protected long mCharID;
        public virtual long CharID
        {
            get
            {
                return mCharID;
            }
        }

        public OtherPlayerInfo Info { get { return mInfo as OtherPlayerInfo; } }

        public PlayerObj(int spriteid)
            :base(spriteid)
        {
            mSpriteType = (int)ESpriteType.OtherPlayer;
            mHurtActCooldown = 1000;
        }

        public void Create(long charid, short charclass)
        {
            mCharID = charid;
            mResID = (int)charclass;
            OnInit();
        }


        public override void Clear()
        {
            base.Clear();
            mCharID = 0;
        }

        public override void UpdateSpriteInfo()
        {
            //if (!mShowHud)
            //    return;
            if (this.Info == null)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["Name"] = this.Info.LastName;
            param["Level"] = this.Info.Level;
            param["MaxHealth"] = this.Info.HpMax;
            param["CurrentHealth"] = this.Info.HpNow;
            Local.Instance.CallUnityAction(UnityActionDefine.SetSpriteInfo, param);
        }

        //public override void HurtHp(int hurt)
        //{
        //    if (null == Info)
        //        return;

        //    if (hurt == 0)
        //        return;

        //    Info.HpNow -= hurt;
        //    if (Info.HpNow < 0)
        //        Info.HpNow = 0;

        //    UpdateSpriteInfo();
        //}

        public override void ChangeHp(int newHp)
        {
            if (null == Info)
                return;

            if (Info.HpNow == newHp)
                return;

            Info.HpNow = newHp;
            //UpdateSpriteInfo();
        }

        public override bool IsPlayer()
        {
            return true;
        }
        public override long GetSeqID()
        {
            return mCharID;
        }

        public virtual void Reborn()
        {
            if (!mDead)
                return;
            mDead = false;
            PlayAct(EAnimType.idle);
        }

    }
}
