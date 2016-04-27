using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class PlayerAttack : GameModuleBase
    {
        public PlayerAttack()
        {

        }

        public void OnServerSingSkill(long charid, SingSkillParam param)
        {

        }

        public void OnServerAttack(long charid, SingleAttackParam param)
        {
            GameScene scene = Local.Instance.SceneMgr.GetMyScene();
            if (null == scene)
                return;
            if (!scene.Loaded || !scene.Enable)
                return;

            //攻击者
            if (param.charid != Local.Instance.GetMyPlayer.CharID)
            {
                PlayerObj player = scene.playerMng.GetPlayer(charid);
                if (null != player)
                {
                    ConfigSkillAll config = ConfigManager.Instance.GetConfigSkillAll(param.skill);
                    if (null == config)
                        return;

                    List<SkillTarget> targets = null;
                    short tx = 0, tz = 0;
                    if (param.target > 0)
                    {
                        targets = new List<SkillTarget>();
                        SkillTarget target = new SkillTarget()
                        {
                            targetID = param.target,
                            targetIsPlayer = param.isplayer > 0,
                            hurtValue = 0,
                            hurtType = (HurtType)param.hurttype,
                        };

                        if (param.isplayer == 0)
                        {
                            MonsterObj monsterTarget = scene.monsterMng.GetMonster((int)param.target);
                            if (null != monsterTarget)
                            {
                                if (null != monsterTarget.Info)
                                    target.hurtValue = monsterTarget.Info.HpNow - param.targetHp;
                                tx = (short)monsterTarget.X;
                                tz = (short)monsterTarget.Z;
                                monsterTarget.ChangeHp(param.targetHp);
                            }
                        }
                        else
                        {
                            PlayerObj playerTarget = scene.playerMng.GetPlayer(param.target);
                            if (null != playerTarget)
                            {
                                if(null != player.Info)
                                    target.hurtValue = player.Info.HpNow - param.targetHp;
                                tx = (short)playerTarget.X;
                                tz = (short)playerTarget.Z;
                                playerTarget.ChangeHp(param.targetHp);
                            }
                        }

                        targets.Add(target);

                        player.SetMoveDest(param.sx, param.sz);
                        player.ReCalcDir(param.sx, param.sz, tx, tz);
                        player.MoveSprite(300);
                    }

                    long tickNow = DateTime.Now.Ticks / 10000;
                    player.StopMove();
                    player.EffectLogic.BeginSkillEffect(config, targets, tickNow);
                }
            }
        }
        
    }
}
