using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using SPSGame.CsShare.Data;
using SPSGame.Tools;
using SPSGame.Unity;

namespace SPSGame.GameModule
{
    public class PlayerSkillSingLogic
    {
        protected long mCurrSingEndTick;              //本次吟唱结束时间
        protected ConfigSkillAll mCurrSkillCfg;         //本次吟唱技能id
        protected List<SkillTarget> mTargets;
        protected LiveSprite m_Owner = null;
        protected int SfxSpriteID = 0;
        protected EAnimType m_CurAnimType = EAnimType.idle;

        public PlayerSkillSingLogic(LiveSprite obj)
        {
            m_Owner = obj;
            SfxSpriteID = 0;
        }

        public bool IsSinging()
        {
            return mCurrSkillCfg != null;
        }

        public virtual void Update(long tickNow)
        {
        }

        public virtual void EndSing(long tickNow)
        {
            //施放技能
            mCurrSkillCfg = null;
            mCurrSingEndTick = 0;
            mTargets = null;
            m_CurAnimType = EAnimType.idle;
            m_Owner.PlayAct(EAnimType.idle);
            KillSfx();
            SfxSpriteID = 0;
        }

        private void KillSfx()
        {
            if (0 == SfxSpriteID)
                return;
            if (null == m_Owner.SceneObj)
                return;
            SfxObj obj = m_Owner.SceneObj.sfxManager.GetObj(SfxSpriteID);
            if (null != obj)
                obj.KillSfx();
        }

        //吟唱
        public virtual void BeginSing(ConfigSkillAll config, List<SkillTarget> targets, long tickNow)
        {
            if (IsSinging())
            {
                CancelSing();
            }
            mTargets = targets;
            mCurrSkillCfg = config;
            mCurrSingEndTick = tickNow + (long)(config.SkillSingTime * 1000);
            SfxSpriteID = 0;
            PlaySingAct();
        }

        protected void PlaySingAct()
        {
            DebugMod.Log("OtherPlayer PlaySingAct");
            if (null == m_Owner || m_Owner.SceneObj == null)
                return;
            if (null == mCurrSkillCfg)
                return;

            m_CurAnimType = (Unity.EAnimType)mCurrSkillCfg.SkillSingActionID;
            m_Owner.StopMove(m_CurAnimType);

            //创建动作特效
            if (mCurrSkillCfg.SkillSingActionEffectID > 0)
            {
                SfxObjData sfxdata = new SfxObjData();
                sfxdata.lifetime = -1;
                sfxdata.ResID = mCurrSkillCfg.SkillSingActionEffectID;
                sfxdata.ShowTick = DateTime.Now.Ticks / 10000;
                sfxdata.srcSpriteID = m_Owner.SpriteID;
                sfxdata.creatureType = (int)Unity.ECreatureType.Hang;
                SfxObj sfxobj = m_Owner.SceneObj.sfxManager.CreateSfxObj(sfxdata);
                if (null != sfxobj)
                {
                    SfxSpriteID = sfxobj.SpriteID;
                }
            }
        }

        public virtual void CancelSing()
        {
            mCurrSkillCfg = null;
            mCurrSingEndTick = 0;
            mTargets = null;
            m_CurAnimType = EAnimType.idle;
            KillSfx();
            SfxSpriteID = 0;
        }

        public void CheckActionBreaking(EAnimType newAct)
        {
            if (mCurrSkillCfg == null)
                return;
            if (m_CurAnimType == newAct)
                return;
            if (newAct == EAnimType.hurt)
                return;
            if (newAct == EAnimType.idle)
                return;
            CancelSing();
        }
    }
}
