using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class MyPlayer : PlayerObj
    {
        protected bool mCanControl = true;
        public bool CanControl { get { return mCanControl; } }

        public int AttackRange
        {
            get {
                if (mAttr == null)
                    return 35;
                else
                    return mAttr.AttackRange + 12; 
            }
        }

        public override long CharID
        {
            get
            {
                if (null == MyInfo)
                    return 0;
                else
                    return MyInfo.CharID;
            }
        }

        public MyPlayerAttr MyAttr { get { return mAttr; } }
        protected MyPlayerAttr mAttr = null;

        public MyPlayerInfo MyInfo { get { return mInfo as MyPlayerInfo; } }

        public MyPlayerSkill m_objSkill = new MyPlayerSkill();

        protected Dictionary<long, NetDataPlayerBase> m_MyPlayerList = new Dictionary<long, NetDataPlayerBase>();
        protected Dictionary<long, NetDataPlayerBase> m_CreatePlayerList = new Dictionary<long, NetDataPlayerBase>();

        public MyPlayerMove MoveLogic =  new MyPlayerMove();
        //public MyPlayerSkill LearnSkillLogic = new MyPlayerSkill();//新增

        public MyPlayerSkillSingLogic MySingLogic { get { return mSingLogic as MyPlayerSkillSingLogic; } }
        public MyPlayerSkillEffectLogic MyEffectLogic { get { return mEffectLogic as MyPlayerSkillEffectLogic; } }

        public MyPlayer(int spriteid)
            :base(spriteid)
        {
            mSpriteType = (int)ESpriteType.Player;
            mHurtActCooldown = 2000;
            mEnable = true;
        }
        public override bool IsPlayer()
        {
            return true;
        }

        public override long GetSeqID()
        {
            return CharID;
        }

        public override void Clear()
        {
            base.Clear();

            mCanControl = true;
            mAttr = null;
            mUserId = 0;
            m_MyPlayerList.Clear();
            m_CreatePlayerList.Clear();
            mLoadFinish = false;
        }

        public override void SetInfo(object info)
        {
            base.SetInfo(info);
            Create(MyInfo.CharID, MyInfo.CharClass);
        }

        public void OnRcvAttr(MyPlayerAttr attr)
        {
            mAttr = attr;
            mSpeed = attr.MoveSpeed;
        }

        public void OnRcvStatus(MyPlayerStatus data)
        {

        }

        public override void OnInit()
        {
            mEffectLogic = new MyPlayerSkillEffectLogic();
            mSingLogic = new MyPlayerSkillSingLogic(MyEffectLogic);
        }

        public void OnLogout()
        {
            mUserId = 0;
            m_MyPlayerList.Clear();
            m_CreatePlayerList.Clear();
        }

        public void OnLogin(int userid, List<long> listCharID)
        {
            mUserId = userid;
            m_MyPlayerList.Clear();
            foreach(var id in listCharID)
            {
                m_MyPlayerList[id] = null;
            }
        }

        public void OnRcvCharList(List<NetDataPlayerBase> list)
        {
            foreach(var player in list)
            {
                m_MyPlayerList[player.CharID] = player;
            }
        }

        public void OnCreatePlayer(NetDataPlayerBase newplayer)
        {
            m_CreatePlayerList[newplayer.CharID] = newplayer;
        }

        public void OnDeletePlayer(long charid)
        {
            if (m_CreatePlayerList.ContainsKey(charid))
                m_CreatePlayerList.Remove(charid);
            if (m_MyPlayerList.ContainsKey(charid))
                m_MyPlayerList.Remove(charid);
        }

        public NetDataPlayerBase GetMyPlayerBase(long charid)
        {
            NetDataPlayerBase ret = null;
            m_MyPlayerList.TryGetValue(charid, out ret);
            return ret;
        }

        public NetDataPlayerBase OnCreatePlayerOver(long charid)
        {
            NetDataPlayerBase newplayer = null;
            if (m_CreatePlayerList.TryGetValue(charid, out newplayer))
            {
                m_MyPlayerList[charid] = newplayer;
                m_CreatePlayerList.Remove(charid);
            }
            return newplayer;
        }

        public bool IfRcvAllPlayerList()
        {
            foreach(var pair in m_MyPlayerList)
            {
                if (pair.Value == null)
                    return false;
            }

            return true;
        }

        public Dictionary<long, NetDataPlayerBase> getPlayerList()
        {
            return m_MyPlayerList;
        }

        public override void OnUpdate(long tickNow)
        {
            long tickPass = tickNow - mLastUpdateTime;
            if (tickPass < 30)
                return;
            mLastUpdateTime = tickNow;

            if (!mLoadFinish)
            {
                if (IfLoadFinish(tickNow) && mCanControl)
                    EnableControl(true);
            }

            UpdateShow(tickNow);
            UpdateMove(tickPass);
        }

        public void OnLeaveScene()
        {
            KillSprite();
            mEnable = false;
        }

        public override void AfterSpriteLoad()
        {
            ActionParam param = new ActionParam();
            param["ControlSpeed"] = (int)Speed;
            param["ControlAble"] = false;
            Local.Instance.CallUnityAction(UnityActionDefine.ControlPlayer, param);
        }

        public void EnableControl(bool enable)
        {
            ActionParam param = new ActionParam();
            param["ControlAble"] = enable;
            Local.Instance.CallUnityAction(UnityActionDefine.ControlPlayer, param);
        }

        public override void ToDead()
        {
            if (mDead)
                return;

            mDead = true;
            mCanControl = false;
            MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
            if (null != fight)
            {
                fight.CancelAutoFight();
            }
            EnableControl(false);
            ShowHud(false, 0);
            SetMoveDest((short)this.X, (short)this.Z);
            MoveSprite(300);
            PlayAct(EAnimType.death);

            ActionParam param = new ActionParam();
            param["LifeState"] = "ToDeath";
            Local.Instance.CallUnityAction(UnityActionDefine.ToDeath, param);//待定
        }

        public override void Reborn()
        {
            if (!mDead)
                return;
            mDead = false;
            mCanControl = true;
            MyPlayerFight fight = Local.Instance.GetModule("mpfight") as MyPlayerFight;
            if (null != fight)
            {
                fight.ResumeAutoFight();
            }
            EnableControl(true);
            PlayAct(EAnimType.idle);

            ActionParam param = new ActionParam();
            param["LifeState"] = "ReBorn";
            Local.Instance.CallUnityAction(UnityActionDefine.ToDeathOrReBorn, param);//待定
        }

        public override void UpdateSpriteInfo()
        {
            //if (!this.mShowHud)
            //    return;

            if (this.MyInfo == null || this.mAttr == null)
                return;

            ActionParam param = new ActionParam();
            param["SpriteID"] = this.SpriteID;
            param["Name"] = this.MyInfo.LastName;
            param["Level"] = this.MyInfo.Level;
            param["MaxHealth"] = this.mAttr.HpMax;
            param["CurrentHealth"] = this.mAttr.HpNow;
            Local.Instance.CallUnityAction(UnityActionDefine.SetSpriteInfo, param);
        }

        public override void ChangeHp(int newHp)
        {
            if (null == mAttr)
                return;

            if (mAttr.HpNow == newHp)
                return;

            mAttr.HpNow = newHp;
        }
		
		//2015-10-21 add by gaoyuhang
        /// <summary>
        /// 更新人物属性
        /// </summary>
        /// <returns></returns>
        public bool UpdatePlayerAttr (List<PlayerAttrSingle> playerAttrList)
        {
            foreach (PlayerAttrSingle attr in playerAttrList)
            {
                ChangePlayerAttr(attr.AttrType, attr.AttrValue);
            }

            ActionParam param = new ActionParam();
            param["Force"] = mAttr.Strength;
            param["Speed"] = mAttr.Dex;
            param["Wit"] = mAttr.Inte;
            param["ForceUp"] = mAttr.StrengthAdd;
            param["SpeedUp"] = mAttr.DexAdd;
            param["WitUp"] = mAttr.InteAdd;

            param["MaxHp"] = mAttr.HpMax;
            param["PhyATK"] = mAttr.PhyAttack;
            param["MagicATK"] = mAttr.MagAttack;
            param["PhysicProtect"] = mAttr.PhyDef;
            param["MagicProtect"] = mAttr.MagDef;
            param["PhyCrit"] = mAttr.HeavyAttack;
            param["CritHit"] = mAttr.HeavyAttackDamage;

            Local.Instance.CallUnityAction(UnityActionDefine.ShowRoleProperty, param);

            return true;
        }

        public void ChangePlayerAttr(short attrType, int AttrValue)
        {
            if (0 == attrType || 0 == AttrValue)
            {
                return;
            }

            switch (attrType)
            {
                //最大生命
                case 5:
                    {
                        mAttr.HpMax = AttrValue;
                        break;
                    }
                //生命回复
                case 6:
                    {
                        mAttr.HpRestore = AttrValue;
                        break;
                    }
                //最大能量
                case 8:
                    {
                        mAttr.MpMax = AttrValue;
                        break;
                    }
                //物理攻击
                case 9:
                    {
                        mAttr.PhyAttack = AttrValue;
                        break;
                    }
                //法术强度
                case 10:
                    {
                        mAttr.MagAttack = AttrValue;
                        break;
                    }
                //物理护甲
                case 11:
                    {
                        mAttr.PhyDef = AttrValue;
                        break;
                    }
                //法术抗性
                case 12:
                    {
                        mAttr.MagDef = AttrValue;
                        break;
                    }
                //暴击
                case 13:
                    {
                        mAttr.HeavyAttack = AttrValue;
                        break;
                    }
                //暴击伤害
                case 14:
                    {
                        mAttr.HeavyAttackDamage = AttrValue;
                        break;
                    }
                //闪避
                case 15:
                    {
                        mAttr.Dodge = AttrValue;
                        break;
                    }
                //护甲穿透
                case 16:
                    {
                        mAttr.AntiArmor = AttrValue;
                        break;
                    }
                //忽视抗性
                case 17:
                    {
                        mAttr.AntiResist = AttrValue;
                        break;
                    }
                //吸血
                case 18:
                    {
                        mAttr.AbsorbHp = AttrValue;
                        break;
                    }
                //力量
                case 19:
                    {
                        mAttr.Strength = (short)AttrValue;
                        break;
                    }
                //敏捷
                case 20:
                    {
                        mAttr.Dex = (short)AttrValue;
                        break;
                    }
                //智力
                case 21:
                    {
                        mAttr.Inte = (short)AttrValue;
                        break;
                    }
                //攻击距离
                case 22:
                    {
                        mAttr.AttackRange = (short)AttrValue;
                        break;
                    }
                //移动速度
                case 23:
                    {
                        mAttr.MoveSpeed = (short)AttrValue;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
