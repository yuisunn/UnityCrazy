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
    public class MonsterObj : LiveSprite
    {
        protected int mSeq;
        public int Seq { get { return mSeq; } }

        //protected int mID;
        //public int ID { get { return mID; } }
        public MonsterInfo Info { get { return mInfo as MonsterInfo; } }
        public MonsterObj(int spriteid)
            :base(spriteid)
        {
        }

        public void Create(int seq, int id)
        {
            mSpriteType = (int)ESpriteType.Monster;
            mSeq = seq;
            mResID = id;
        }

        public override void Clear()
        {
            base.Clear();
            mSeq = 0;
        }

        public void PlaySkill(int skillid, short targetX, short targetZ)
        {
            ReCalcDir((short)mX, (short)mZ, targetX, targetZ);
            //int rand = SPSGame.Tools.SPSRand.GetRandomNumber(0, 1);
            StopMove((EAnimType)(EAnimType.action11 + skillid));
        }

        public override void UpdateSpriteInfo()
        {
            //if (!mShowHud)
            //    return;
            if (this.Info == null)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["Name"] = this.Info.Name;
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
        }

        public override bool IsPlayer()
        {
            return false;
        }
        public override long GetSeqID()
        {
            return (long)mSeq;
        }
     }
}
