using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.CsShare.Data;
using SPSGame.GameModule;
using SPSGame.Unity;

namespace SPSGame
{


    public class MyPlayerFight : GameModuleBase
    {
        protected bool mEnableAutoFight;
        protected long mLastUpdate;
        protected int mLastAttackMonsterSeq;
        protected bool mAutoMoveState;
        protected long mLastNotifyTick;      //上次同步移动时间
        protected long mNextNormalAttackTick;     //下次普通攻击时间
        protected int mRandSeed = FightRandSeed.RandSeedHalf;


        public override void OnInit()
        {
            mLastUpdate = 0;
            mLastAttackMonsterSeq = 0;
            mAutoMoveState = false;
            mLastNotifyTick = 0;
            mNextNormalAttackTick = 0;
            mEnableAutoFight = true;
        }


        public override void OnUpdate()
        {
            long tickNow = DateTime.Now.Ticks / 10000;
            if (tickNow - mLastUpdate < 100)
                return;
            mLastUpdate = tickNow;

            if (!Local.Instance.GetMyPlayer.IfLoadFinish(tickNow))
                return;

            if (Local.Instance.GetMyPlayer.SingLogic != null)
            {
                Local.Instance.GetMyPlayer.SingLogic.Update(tickNow);
            }

            if (Local.Instance.GetMyPlayer.EffectLogic != null)
            {
                Local.Instance.GetMyPlayer.EffectLogic.Update(tickNow);
            }

            if (!Local.Instance.GetMyPlayer.SingLogic.IsSinging() && mEnableAutoFight)
            {
                mAutoMoveState = UpdateAutoFight(tickNow);
            }
        }

        public void CancelAutoFight()
        {
            mEnableAutoFight = false;
            mLastAttackMonsterSeq = 0;
            mAutoMoveState = false;
        }

        public void ResumeAutoFight()
        {
            mEnableAutoFight = true;
            mAutoMoveState = false;
        }

        protected bool UpdateAutoFight(long tickNow)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (!myplayer.LastVisible)
                return false;
            if (!myplayer.Loaded)
                return false;
            if (!myplayer.IsLive)
                return false;

            MonsterObj monster = null;
            monster = FindLastAttackMonster(myplayer.AttackRange);
            if (null == monster)
            {
                //如果上次攻击的怪物不在射程内，就重新寻找最近的怪物
                monster = FindNearestMonster(myplayer.X, myplayer.Y, myplayer.Z, myplayer.AttackRange * 10);
                if (null == monster)
                {
                    if (mAutoMoveState)
                    {
                        if (myplayer.MoveLogic != null)
                            myplayer.MoveLogic.OnLogicStopMove();
                    }
                    return false;
                }
            }

            float diffX = monster.X - myplayer.X;
            float diffZ = monster.Z - myplayer.Z;
            double disDiff = Math.Sqrt(diffX * diffX + diffZ * diffZ);
            double disNeedMove = disDiff - (0.8 * myplayer.AttackRange);
            if (disNeedMove <= 0)
            {
                if (tickNow >= mNextNormalAttackTick)         //攻击间隔
                {
                    AttackMonster(monster, -1, tickNow); //-1=普通攻击
                }
                return false;
            }

            bool needNotify = false;
            if (!mAutoMoveState)
            {
                needNotify = true;          //开始移动
                //DebugMod.Log("start move");
            }

            int olddirX = (int)(100 * myplayer.DirX) / 5 * 5;
            int olddirZ = (int)(100 * myplayer.DirZ) / 5 * 5;
            //short destX = (short)(myplayer.X + diffX / disDiff * myplayer.Speed * 5);
            //short destZ = (short)(myplayer.Z + diffZ / disDiff * myplayer.Speed * 5);
            myplayer.SetMoveDest((short)monster.X, (short)monster.Z);

            if (!needNotify)
            {
                int newdirX = (int)(100 * myplayer.DirX) / 5 * 5;
                int newdirZ = (int)(100 * myplayer.DirZ) / 5 * 5;
                if (olddirX != newdirX || olddirZ != newdirZ)
                {
                    needNotify = true;
                }
            }

            if (!needNotify)
            {
                if (tickNow - mLastNotifyTick > 300)       //同步
                {
                    needNotify = true;
                }
            }

            if (needNotify)
            {
                //发送新的目标点给服务器
                mLastNotifyTick = tickNow;
                if (myplayer.MoveLogic != null)
                    myplayer.MoveLogic.OnLogicMoveTo();
            }

            return true;
        }

        protected void AttackMonster(MonsterObj monster, int skillorder, long tickNow)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;

            int skillid = myplayer.MyEffectLogic.ConvertToSkillID(skillorder);
           
            ConfigSkillAll config = ConfigManager.Instance.GetConfigSkillAll(skillid);
            if (null == config)
                return;

            long delay = 0;
            if (skillorder < 0)
                delay = (int)(config.SkillPublicCoolingTime * 1000);
            else
                delay = (int)(config.SkillEndTime * 1000);

            mNextNormalAttackTick = tickNow + delay;
            mLastAttackMonsterSeq = null != monster ? monster.Seq : 0;
            mAutoMoveState = false;

            List<SkillTarget> targets = null;
            if (null != monster)
            {
                targets = new List<SkillTarget>();
                SkillTarget target = new SkillTarget()
                {
                    targetID = monster.Seq,
                    targetIsPlayer = false,
                };
                //计算伤害
                CalcAttackMonsterHurt(skillorder, monster, out target.hurtValue, out target.hurtType);
                targets.Add(target);
            }

            if (config.SkillSingTime > 0)
            {
                Local.Instance.GetMyPlayer.SingLogic.BeginSing(config, targets, tickNow);
            }
            else
            {
                myplayer.StopMove();
                Local.Instance.GetMyPlayer.EffectLogic.BeginSkillEffect(config, targets, tickNow);    //开始执行技能效果逻辑
            }
        }

        protected void CalcAttackMonsterHurt(int skillid, MonsterObj monster, out int hurtValue, out HurtType hurtType)
        {
            hurtValue = 0;
            hurtType = HurtType.Normal;

            ConfigMonster config = ConfigManager.Instance.GetConfigMonster(monster.ResID);
            if (null == config)
                return;
            ConfigMonsterStaticAttr configS = ConfigManager.Instance.GetConfigMonsterStaticAttr(config.StaticAttr);
            if (null == configS)
                return;
            ConfigMonsterDynAttr configD = ConfigManager.Instance.GetConfigMonsterDynAttr(config.DynAttr);
            if (null == configD)
                return;

            int defM = (int)(configS.PhyDef * configD.PhyDef);
            MyPlayerAttr attr = Local.Instance.GetMyPlayer.MyAttr;
            hurtValue = (int)((attr.PhyAttack - defM) * (1 + 0.12 * (FightRandSeed.RandSeedHalf - mRandSeed) / FightRandSeed.RandSeedTotal));
            if (hurtValue <= 0)
                hurtValue = 1;
            hurtType = HurtType.Normal;

            if (monster.Info != null)
            {
                if (hurtValue >= monster.Info.HpNow)
                    hurtType = HurtType.Dead;
            }
        }

        protected MonsterObj FindNearestMonster(float x, float y, float z, int range)
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;
            if (!scene.Enable)
                return null;
            if (!scene.Loaded)
                return null;
            //if (scene.SceneType != Unity.ESceneType.Tower)
            //    return null;



            MonsterObj ret =  scene.monsterMng.FindNearestMonster(x, y, z, range * range);
            if (null != ret)
            {
                //DebugMod.Log("monsterMng.FindNearestMonster");
            }
            return ret;
        }

        protected MonsterObj FindLastAttackMonster(int range)
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;
            if (!scene.Enable)
                return null;
            if (!scene.Loaded)
                return null;

            if (mLastAttackMonsterSeq == 0)
                return null;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            int maxDisQ = range * range;
            MonsterObj obj = scene.monsterMng.GetMonster(mLastAttackMonsterSeq);
            if (null == obj || !obj.Enable || !obj.IsLive || (short)obj.Y != (short)myplayer.Y || !obj.LastVisible)
            {
                mLastAttackMonsterSeq = 0;
                return null;
            }

            int diffX = (int)(obj.X - myplayer.X);
            int diffZ = (int)(obj.Z - myplayer.Z);
            int disQ = diffX * diffX + diffZ * diffZ;
            if (disQ > maxDisQ)
            {
                mLastAttackMonsterSeq = 0;
                return null;
            }

            return obj;
        }

        public void OnUseSkill(int skillorder)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null == myplayer)
                return;

            if (!myplayer.IsLive)
                return;

            //找目标
            MonsterObj monster = FindCurDirMonster(myplayer.AttackRange);
            AttackMonster(monster, skillorder, DateTime.Now.Ticks / 10000);
        }

        protected bool InMyDir(float x, float y, float z, int range)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if ((short)y != (short)myplayer.Y)
                return false;

            short diffX = (short)(x - myplayer.X);
            short diffZ = (short)(z - myplayer.Z);

            if (diffX * diffX + diffZ * diffZ > range * range)
                return false;    //超出范围

            return (myplayer.DirX * diffX >= 0 && myplayer.DirZ * diffZ >= 0);
        }

        //寻找前方射程内的怪物
        protected MonsterObj FindCurDirMonster(int range)
        {
            MyPlayer myplayer = Local.Instance.GetMyPlayer;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return null;
            if (!scene.Enable)
                return null;
            if (!scene.Loaded)
                return null;
            //if (scene.SceneType != Unity.ESceneType.Tower)
            //    return null;

            MonsterObj last = FindLastAttackMonster(range);
            if (null != last)
            {
                //优先攻击上次攻击的怪物
                if (InMyDir(last.X, last.Y, last.Z, range))
                    return last;
            }

            MonsterObj ret = scene.monsterMng.FindDirMonster(myplayer.DirX, myplayer.DirZ, (short)myplayer.X, (short)myplayer.Y, (short)myplayer.Z, range * range);
            return ret;
        }

        public void OnServerAttack(MySingleAttackParam param)
        {
            mRandSeed = param.newRandSeed;

            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;
            MyPlayer myplayer = Local.Instance.GetMyPlayer;

            if (param.hurttype == (short)HurtType.Dead)
            {
                HurtObj obj = scene.hurtManager.FindMyTarget(myplayer.CharID, param.target, param.isplayer > 0);
                if (null != obj)
                    obj.ChangeHurtType(param.hurttype);
            }

            if (param.isplayer == 0)
            {
                MonsterObj monsterTarget = scene.monsterMng.GetMonster((int)param.target);
                if (null != monsterTarget)
                {
                    monsterTarget.ChangeHp(param.targetHp);
                }
            }
            else
            {
                PlayerObj playerTarget = scene.playerMng.GetPlayer(param.target);
                if (null != playerTarget)
                {
                    playerTarget.ChangeHp(param.targetHp);
                }
            }
        }

    }
}
