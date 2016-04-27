using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame.GameModule
{
    public class ParamMonsterAttack
    {
        public int monSeq;
        public long charid; 
        public short hurttype;
        public int playerHp;
    }

    public class MonsterAttack : GameModuleBase
    {
        public MonsterAttack()
        {

        }

        public void OnServerMonsterAttack(ParamMonsterAttack pp)
        {
            if (null == pp)
                return;

            GameScene myscene = Local.Instance.SceneMgr.GetMyScene();
            if (null == myscene)
                return;

            MonsterObj monster = myscene.monsterMng.GetMonster(pp.monSeq);
            if (null == monster)
                return;

            short tx=0, tz=0;

            SkillTarget target = new SkillTarget();
            HurtData hurtdata = new HurtData();
            hurtdata.effectCfg = null;
            hurtdata.actTick = DateTime.Now.Ticks / 10000 +(long)(200);
            hurtdata.srcID = (long)monster.Seq;
            hurtdata.srcIsPlayer = true;
            hurtdata.target = target;
            target.hurtType = (HurtType)pp.hurttype;
            target.targetID = pp.charid;
            target.targetIsPlayer = true;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (pp.charid == myplayer.CharID)
            {
                target.hurtValue = myplayer.MyAttr.HpNow - pp.playerHp;
                myscene.hurtManager.CreateHurtObj(hurtdata);

                tx = (short)myplayer.X;
                tz = (short)myplayer.Z;
                myplayer.ChangeHp(pp.playerHp);
            }
            else
            {
                PlayerObj player = myscene.playerMng.GetPlayer(pp.charid);
                if (null != player)
                {
                    target.hurtValue = player.Info.HpNow - pp.playerHp;
                    myscene.hurtManager.CreateHurtObj(hurtdata);

                    tx = (short)player.X;
                    tz = (short)player.Z;
                    player.ChangeHp(pp.playerHp);
                }
                else
                    return;
            }

            //monster.SetPos((short)pp.monsterX, monster.Y, (short)pp.monsterZ);
            monster.PlaySkill(0, tx, tz);
        }
    }
}
