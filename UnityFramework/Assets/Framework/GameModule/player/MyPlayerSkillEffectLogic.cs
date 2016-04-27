using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class MyPlayerSkillEffectLogic : PlayerSkillEffectLogic
    {
        public MyPlayerSkillEffectLogic()
            : base(Local.Instance.GetMyPlayer)
        {
        }

        public int ConvertToSkillID(int skillorder)
        {
            int temp = Local.Instance.GetMyPlayer.MyInfo.CharClass * 10000;
            int skillid = 0;
            switch (skillorder)
            {
                case -1:
                    skillid = Local.Instance.GetMyPlayer.MyInfo.CharClass;
                    break;
                default:
                    skillid = MyPlayerSkillDBManager.Instance.GetSkillIDByIconPos(skillorder);
                    break;
            }

            return skillid;
        }

        public override void BeginSkillEffect(ConfigSkillAll config, List<SkillTarget> targets, long tickNow)
        {
            //DebugMod.Log("MyPlayer BeginSkillEffect");
            base.BeginSkillEffect(config, targets, tickNow);
            Local.Instance.GetMyPlayer.EnableControl(false);      //禁止控制
            ReqCastSkill();
            MyPlayerSkillDBManager.Instance.OnRecordTime(config.SkillID, tickNow, config.SkillPublicCoolingTime );
            //DebugMod.Log("BeginSkillEffect");
        }
 
        protected void ReqCastSkill()
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

            //if (null != targetObj)
            //{
            //    actionParam["MonsterX"] = (short)targetObj.X;
            //    actionParam["MonsterZ"] = (short)targetObj.Z;
            //}
            //else
            //{
            //    actionParam["MonsterX"] = (short)(mOwner.X + mOwner.DirX);
            //    actionParam["MonsterZ"] = (short)(mOwner.Z + mOwner.DirZ);
            //}

            if (target != null)
                actionParam["isplayer"] = (short)(target.targetIsPlayer ? 1 : 0);
            else
                actionParam["isplayer"] = (short)0;

            actionParam["HurtValue"] = (int)((null != target) ? target.hurtValue : 0);
            actionParam["HurtType"] = (short)((null != target) ? target.hurtType : 0);

            GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
            if (null == gamesvr)
                return;
            gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_PlayerSingleAttack, actionParam, null);
        }

        protected override void EndYingzhi()
        {
            base.EndYingzhi();
            if (Local.Instance.GetMyPlayer.CanControl)
                Local.Instance.GetMyPlayer.EnableControl(true);       //恢复可控
        }
    }
}
