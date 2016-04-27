using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class SkillTarget
    {
        public long targetID;
        public bool targetIsPlayer;
        public int hurtValue;
        public HurtType hurtType;
    }

    public class PlayerSkillEffectData
    {
        public long waitTime;
        //public long lifeTime;
        //public long startPlayTick;
        //public double startOdds;
        public ConfigSkillEffect effectCfg;
        public bool played;
        public int SfxSpriteID = 0;
    }

    public class PlayerSkillEffectLogic
    {
        protected long mCurrSkillYingzhiTick;           //本次攻击硬直结束时间
        protected long mCurrSkillEndTick;              //本次攻击结束时间
        protected long mCurrSkillStartTick;            //本次攻击开始时间
        protected ConfigSkillAll mCurrSkillCfg;        //本次攻击技能id
        protected List<PlayerSkillEffectData> mCurrSkillEffects = new List<PlayerSkillEffectData>();

        protected List<SkillTarget> mTargets = null;
        protected LiveSprite m_Owner = null;
        public virtual bool IsOwnerLive()
        {
            if (m_Owner == null)
                return false;
            else
                return m_Owner.IsLive;
        }

        public PlayerSkillEffectLogic(LiveSprite obj)
        {
            m_Owner = obj;
        }

        public void Update(long tickNow)
        {
            if (mCurrSkillCfg != null)
            {
                if (!IsOwnerLive())
                {
                    EndCurrSkill(true);
                }
                else if (mCurrSkillYingzhiTick > 0 && tickNow > mCurrSkillYingzhiTick)
                {
                    EndYingzhi();
                }
                else if (tickNow > mCurrSkillEndTick)   //上次技能是否结束
                {
                    EndCurrSkill(false);
                }
                else
                {
                    UpdateEffects(tickNow);
                }
            }
        }

        protected virtual void EndYingzhi()
        {
            mCurrSkillYingzhiTick = 0;
        }

        protected virtual void EndCurrSkill(bool isbreak = false)
        {
            mCurrSkillCfg = null;
            //if (isbreak)
            //{
            //    //如果是被打断的，需要清除技能特效
            //    foreach(var eff in mCurrSkillEffects)
            //    {
            //        KillSfx(eff);
            //    }
            //}
            mCurrSkillEffects.Clear();
            mCurrSkillEndTick = 0;
            mCurrSkillStartTick = 0;

            if (m_Owner != null)
                m_Owner.PlayAct(Unity.EAnimType.idle);

            EndYingzhi();
        }

        protected void UpdateEffects(long tickNow)
        {
            if (mCurrSkillEffects.Count == 0)
                return;

            foreach(var effect in mCurrSkillEffects)
            {
                if (effect.played)
                    continue;
                if (tickNow > mCurrSkillStartTick + effect.waitTime)
                {
                    if (!PlayEffect(effect, tickNow))
                    {
                        EndCurrSkill(true);
                        break;
                    }
                }
            }
        }

        protected bool PlayEffect(PlayerSkillEffectData effect, long tickNow)
        {
            effect.played = true;
            //effect.startPlayTick = tickNow;

            if (effect.effectCfg.MoveEffectType > 0)
            {
                if (!PlayFastMove(effect))
                    return false;
            }
            else if (effect.effectCfg.CastActionID > 0)
            {
                if (!CastSkill(effect))
                    return false;
            }

            BuildHurtData(effect, tickNow);
            return true;
        }

        protected bool CastSkill(PlayerSkillEffectData effect)
        {
            if (null == m_Owner)
                return false;
            if (null == mCurrSkillCfg)
                return false;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return false;

            bool needChangeDir = false;
            if (mTargets != null && mTargets.Count > 0)
            {
                LiveSprite targetObj = scene.GetLiveSprite(mTargets[0].targetID, mTargets[0].targetIsPlayer);
                if (null != targetObj)
                {
                    short oldDirx = m_Owner.DirX;
                    short oldDirz = m_Owner.DirZ;
                    m_Owner.ReCalcDir((short)m_Owner.X, (short)m_Owner.Z, (short)targetObj.X, (short)targetObj.Z);   //改变朝向
                    if (m_Owner.DirX != oldDirx || m_Owner.DirZ != oldDirz)
                    {
                        needChangeDir = true;
                    }
                }
            }

            //创建动作特效
            PlaySkillSfx(effect);

            if (needChangeDir)
                m_Owner.StopMove((Unity.EAnimType)effect.effectCfg.CastActionID);
            else
                m_Owner.PlayAct((Unity.EAnimType)effect.effectCfg.CastActionID); //放技能动作

            return true;
        }

        protected void PlayFly(PlayerSkillEffectData effect)
        {

        }

        protected bool PlayFastMove(PlayerSkillEffectData effect)
        {
            if (null == m_Owner)
                return false;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return false;


            if ((MoveEffectType)effect.effectCfg.MoveEffectType == MoveEffectType.Target_Front)
            {
                LiveSprite targetObj = null;
                //瞬移到目标身前
                if (mTargets != null && mTargets.Count > 0)
                {
                    int randtarget = SPSGame.Tools.SPSRand.GetRandomNumber(0, mTargets.Count);
                    SkillTarget target = mTargets[randtarget];
                    targetObj = scene.GetLiveSprite(target.targetID, target.targetIsPlayer);
                    if (null == targetObj)
                    {
                        //DebugMod.LogWarning("targetObj miss");
                        return false;
                    }
                }
                else
                    return false;

                //DebugMod.LogWarning(string.Format("ReCalcDir:{0}_{1}/{2}_{3}", (short)m_Owner.X, (short)m_Owner.Z, (short)targetObj.X, (short)targetObj.Z));
                m_Owner.ReCalcDir((short)m_Owner.X, (short)m_Owner.Z, (short)targetObj.X, (short)targetObj.Z);

                MovePoint destPoint = m_Owner.CalcPointByDir((MoveEffectType)effect.effectCfg.MoveEffectType, effect.effectCfg.MoveEffectDistance, targetObj);
                if (null != destPoint)
                {
                    m_Owner.FastMoveTo(destPoint.X, destPoint.Z, m_Owner.DirX, m_Owner.DirZ, effect.effectCfg.MoveResourceID, effect.effectCfg.MoveEffectSpeed);
                    PlaySkillSfx(effect);
                }
                else
                    return false;
            }
            else if ((MoveEffectType)effect.effectCfg.MoveEffectType == MoveEffectType.Target_Back)
            {
                LiveSprite targetObj = null;
                //瞬移到目标身后
                if (mTargets != null && mTargets.Count > 0)
                {
                    int randtarget = SPSGame.Tools.SPSRand.GetRandomNumber(0, mTargets.Count);
                    SkillTarget target = mTargets[randtarget];
                    targetObj = scene.GetLiveSprite(target.targetID, target.targetIsPlayer);
                    if (null == targetObj)
                    {
                        //DebugMod.LogWarning("targetObj miss");
                        return false;
                    }
                }
                else
                    return false;

                //DebugMod.LogWarning(string.Format("ReCalcDir:{0}_{1}/{2}_{3}", (short)m_Owner.X, (short)m_Owner.Z, (short)targetObj.X, (short)targetObj.Z));
                m_Owner.ReCalcDir((short)m_Owner.X, (short)m_Owner.Z, (short)targetObj.X, (short)targetObj.Z);

                MovePoint destPoint = m_Owner.CalcPointByDir((MoveEffectType)effect.effectCfg.MoveEffectType, effect.effectCfg.MoveEffectDistance, targetObj);
                if (null != destPoint)
                {
                    m_Owner.ReCalcDir(destPoint.X, destPoint.Z, (short)targetObj.X, (short)targetObj.Z);
                    m_Owner.FastMoveTo(destPoint.X, destPoint.Z, m_Owner.DirX, m_Owner.DirZ, effect.effectCfg.MoveResourceID, effect.effectCfg.MoveEffectSpeed);
                    PlaySkillSfx(effect);
                }
                else
                    return false;
            }
            else 
            {
                //自身向前方或后方
                MovePoint destPoint = m_Owner.CalcPointByDir((MoveEffectType)effect.effectCfg.MoveEffectType, effect.effectCfg.MoveEffectDistance);
                if (null != destPoint)
                {
                    m_Owner.BeatToPos(destPoint.X, destPoint.Z, (short)(effect.effectCfg.MoveEffectSpeed));

                    if (effect.effectCfg.MoveResourceID > 0)
                        m_Owner.PlayAct((Unity.EAnimType)effect.effectCfg.MoveResourceID);
                    PlaySkillSfx(effect);
                }
            }

            return true;
        }

        protected virtual void BuildHurtData(PlayerSkillEffectData effect, long tickNow)
        {
            if (null == m_Owner)
                return;

            if (mTargets == null || mTargets.Count == 0)
                return;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;

            foreach (var target in mTargets)
            {
                HurtData data = new HurtData();
                data.effectCfg = effect.effectCfg;
                data.actTick = tickNow + (long)(effect.effectCfg.EffectDelayTime * 1000);
                data.srcID = m_Owner.GetSeqID();
                data.srcIsPlayer = m_Owner.IsPlayer();
                data.target = target;
                scene.hurtManager.CreateHurtObj(data);
            }
        }

        public bool IsPlaySkill()
        {
            return mCurrSkillCfg != null;
        }

        public virtual void BeginSkillEffect(ConfigSkillAll config, List<SkillTarget> targets, long tickNow)
        {
            if (m_Owner == null)
                return;
            if (null == m_Owner.SceneObj)
                return;
            if (m_Owner.SingLogic != null)
            {
                if (m_Owner.SingLogic.IsSinging())
                    m_Owner.SingLogic.CancelSing();
            }

            if (null != mCurrSkillCfg)
            {
                EndCurrSkill(true);
            }

            mTargets = targets;
            mCurrSkillCfg = config;
            mCurrSkillEndTick = tickNow + (long)(config.SkillEndTime * 1000);
            mCurrSkillYingzhiTick = tickNow + (long)(config.SkillYingzhiTime * 1000);
            mCurrSkillStartTick = tickNow;
            mCurrSkillEffects.Clear();

            foreach (var col in config.EffectCols)
            {
                ConfigSkillEffect effectCfg = ConfigManager.Instance.GetConfigSkillEffect(col.EffectID);
                if (null != effectCfg)
                {
                    PlayerSkillEffectData effect = new PlayerSkillEffectData();
                    effect.waitTime = (long)(col.EffectTime * 1000);
                    //effect.startOdds = col.EffectOdds;
                    effect.effectCfg = effectCfg;
                    effect.played = false;
                    effect.SfxSpriteID = 0;
                    mCurrSkillEffects.Add(effect);
                }
            }
        }

        protected void PlaySkillSfx(PlayerSkillEffectData effect)
        {
            if (null == mCurrSkillCfg || null == effect || null == m_Owner)
                return;
            if (effect.effectCfg.CastActEffID <= 0)
                return;

            int lifetime = (int)(mCurrSkillCfg.SkillEndTime * 1000 - effect.waitTime);
            if (lifetime > 0)
            {
                SfxObj sfxobj = m_Owner.PlaySfx(effect.effectCfg.CastActEffID, lifetime);
                if (null != sfxobj)
                {
                    effect.SfxSpriteID = sfxobj.SpriteID;
                }
            }
        }

        private void KillSfx(PlayerSkillEffectData effect)
        {
            if (null == effect)
                return;

            if (0 == effect.SfxSpriteID)
                return;

            if (null == m_Owner.SceneObj)
                return;
            SfxObj obj = m_Owner.SceneObj.sfxManager.GetObj(effect.SfxSpriteID);
            if (null != obj)
                obj.KillSfx();
        }

        public void CheckActionBreaking(Unity.EAnimType newAct)
        {
            if (null == mCurrSkillCfg)
                return;
            if (newAct == Unity.EAnimType.death)
            {
                EndCurrSkill(true);
            }
        }
    }
}
