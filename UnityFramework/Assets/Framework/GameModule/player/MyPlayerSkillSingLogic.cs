using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class MyPlayerSkillSingLogic : PlayerSkillSingLogic
    {
        protected MyPlayerSkillEffectLogic mEffectLogic = null;

        public MyPlayerSkillSingLogic(MyPlayerSkillEffectLogic EffectLogic)
            :base(Local.Instance.GetMyPlayer)
        {
            mEffectLogic = EffectLogic;
        }

        //吟唱
        public override void BeginSing(ConfigSkillAll config, List<SkillTarget> targets, long tickNow)
        {
            DebugMod.Log("开始吟唱");
            base.BeginSing(config, targets, tickNow);
            //通知服务器
            ReqSingSkill();
        }

        public override void Update(long tickNow)
        {
            if (IsSinging() && tickNow > mCurrSingEndTick)
                EndSing(tickNow);
        }

        public override void EndSing(long tickNow)
        {
            //DebugMod.Log("myplayer end sing");

            ConfigSkillAll lastSkillCfg = mCurrSkillCfg;         //本次吟唱技能id
            List<SkillTarget> lastTargets = mTargets;

            base.EndSing(tickNow);

            //施放技能
            if (null != mEffectLogic)
            {
                mEffectLogic.BeginSkillEffect(lastSkillCfg, lastTargets, tickNow);
            }
        }

        protected void ReqSingSkill()
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;

            if (mCurrSkillCfg == null)
                return;

            LiveSprite targetObj = null;
            SkillTarget target = null;
            if (mTargets != null && mTargets.Count > 0)
            {
                target = mTargets[0];
                targetObj = scene.GetLiveSprite(target.targetID, target.targetIsPlayer);
            }

            //通知服务器
            ActionParam actionParam = new ActionParam();

            //int hurt = 100;
            actionParam["SkillID"] = mCurrSkillCfg.SkillID;

            if (null != target)
                actionParam["target"] = target.targetID;
            else
                actionParam["target"] = (long)0;

            actionParam["PlayerX"] = (short)m_Owner.X;
            actionParam["PlayerZ"] = (short)m_Owner.Z;

            if (null != targetObj)
            {
                actionParam["MonsterX"] = (short)targetObj.X;
                actionParam["MonsterZ"] = (short)targetObj.Z;
            }
            else
            {
                actionParam["MonsterX"] = (short)(m_Owner.X + m_Owner.DirX);
                actionParam["MonsterZ"] = (short)(m_Owner.Z + m_Owner.DirZ);
            }

            if (target != null)
                actionParam["isplayer"] = (short)(target.targetIsPlayer ? 1 : 0);
            else
                actionParam["isplayer"] = (short)0;

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_BeginSingSkill, actionParam, null);
        }
    }
}
