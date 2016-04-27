using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.Unity;
using SPSGame.CsShare.Data;

//场景内设施
namespace SPSGame
{
    public class HurtObj : SceneObject
    {
        protected HurtData m_data = null;
        protected bool m_IsOver = false;
        protected long m_BulletCreateTick = 0;
        public HurtObj(int spriteid)
            :base(spriteid)
        {

        }

        public bool IsOver()
        {
            return m_IsOver;
        }

        public bool IsMyTarget(long myID, long target, bool targetIsPlayer)
        {

            if (m_data.srcID != myID)
                return false;
            if (!m_data.srcIsPlayer)
                return false;
            if (m_data.target.targetID != target)
                return false;
            return (m_data.target.targetIsPlayer == targetIsPlayer);
        }

        public void ChangeHurtType(short hurttype)
        {
            if (m_data == null)
                return ;
            if (m_data.target == null)
                return ;
            m_data.target.hurtType = (HurtType)hurttype;
        }

        public void Create(HurtData data)
        {
            m_data = data;
            m_IsOver = false;
            mEnable = true;
            m_BulletCreateTick = 0;
            //DebugMod.Log("create HurtObj:" + this.SpriteID);
        }

        private int GetTargetSpriteID()
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
            {
                return 0;
            }

            LiveSprite targetObj = scene.GetLiveSprite(m_data.target.targetID, m_data.target.targetIsPlayer);
            if (null != targetObj)
                return targetObj.SpriteID;
            else
                return 0;
        }

        private int GetSrcSpriteID()
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
            {
                return 0;
            }

            LiveSprite srcObj = scene.GetLiveSprite(m_data.srcID, m_data.srcIsPlayer);
            if (null != srcObj)
                return srcObj.SpriteID;
            else
                return 0;
        }

        protected void CreateBullet(long tickNow)
        {
            if (null == m_data.effectCfg)
                return;
            int targetSpriteID = GetTargetSpriteID();
            int srcSpriteID = GetSrcSpriteID();
            if (0 == srcSpriteID || targetSpriteID == 0)
            {
                m_IsOver = true;
                return;
            }

            ActionParam param = new ActionParam();
            param["CreatureID"] = this.SpriteID;
            param["CreatureType"] = (int)ECreatureType.Bullet;
            param["ResID"] = m_data.effectCfg.FlyResourceID;
            param["SpriteID"] = srcSpriteID;
            param["TargetSpriteID"] = targetSpriteID;
            param["Speed"] = m_data.effectCfg.EffectBulletSpead;
            param["Life"] = 10.0f;

            Local.Instance.CallUnityAction(UnityActionDefine.LoadCreature, param);

            m_BulletCreateTick = tickNow;
        }

        public override void OnUpdate(long tickNow)
        {
            if (m_IsOver)
                return;

            if (null == m_data)
            {
                m_IsOver = true;
                return;
            }

            if (tickNow < m_data.actTick)
                return;

            if (m_BulletCreateTick > 0)
            {
                //子弹发射后，最多等待10秒钟的命中事件如果等不到就会把本对象结束
                if (tickNow > m_BulletCreateTick + 10000)
                {
                    m_IsOver = true;
                    return;
                }
                else
                    return;
            }

            //DebugMod.Log("HurtObj is over:" + this.SpriteID);

            if (null != m_data.effectCfg && m_data.effectCfg.EffectBulletSpead > 0)
            {
                //创建子弹
                CreateBullet(tickNow);
            }
            else
            {
                //播放受伤害，并结束本对象
                m_IsOver = true;
                PlayHitTarget();
            }
        }

        protected void PlayHitTarget()
        {
            if (null == m_data)
                return;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
            {
                return;
            }

            LiveSprite targetObj = scene.GetLiveSprite(m_data.target.targetID, m_data.target.targetIsPlayer);
            if (null == targetObj)
                return;

            if (null != m_data.effectCfg)
            {
                MovePoint point = targetObj.CalcPointByDir((MoveEffectType)m_data.effectCfg.TargetMoveEffectType, m_data.effectCfg.TargetMoveEffectDistance);
                if (null != point)
                {
                    targetObj.BeatToPos(point.X, point.Z, (short)m_data.effectCfg.TargetMoveEffectSpeed);
                }

                if (m_data.effectCfg.HitActID > 0)
                    targetObj.PlayHurt(m_data.target.hurtValue, m_data.target.hurtType, (EAnimType)m_data.effectCfg.HitActID);
                else
                    targetObj.PlayHurt(m_data.target.hurtValue, m_data.target.hurtType);

                if (m_data.effectCfg.HitActEffID > 0)
                    targetObj.PlaySfx(m_data.effectCfg.HitActEffID, 3000);
            }
            else
                targetObj.PlayHurt(m_data.target.hurtValue, m_data.target.hurtType);
        }

        public void OnBulletHit(int hitSpriteID)
        {
            PlayHitTarget();
        }
    }
}
