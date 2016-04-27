using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class MonsterMove : GameModuleBase
    {
        public MonsterMove()
        {

        }

        public void OnServerStopMove(int seq, ParamStopMove dd)
        {
            if (null == dd)
                return;

            GameScene myscene = Local.Instance.SceneMgr.GetMyScene();
            if (null == myscene)
                return;

            MonsterObj monster = myscene.monsterMng.GetMonster(seq);
            if (null == monster)
                return;

            //if (seq == 1)
             //   DebugMod.Log(string.Format("Monster Stop:{0}:{1}/{2}:{3}", dd.CurrX, dd.CurrZ, (short)monster.X, (short)monster.Z));

            monster.SetPos(dd.CurrX, monster.Y, dd.CurrZ);
            monster.StopMove();
        }

        public void OnLogicMoveTo(int seq, ParamMoveTo dd, short speed)
        {
            if (null == dd)
                return;

            GameScene myscene = Local.Instance.SceneMgr.GetMyScene();
            if (null == myscene)
                return;

            MonsterObj monster = myscene.monsterMng.GetMonster(seq);
            if (null == monster)
                return;

            //if (seq == 1)
            //    DebugMod.Log(string.Format("Monster MoveTo:{0}:{1}-->{2}:{3}/{4}:{5}/{6}", 
            //        dd.CurrX, dd.CurrZ, dd.DestX, dd.DestZ, (short)monster.X, (short)monster.Z, speed));

            monster.SetPos(dd.CurrX, monster.Y, dd.CurrZ);
            monster.SetSpeed(speed);
            monster.SetMoveDest(dd.DestX, dd.DestZ, true);
        }
    }
}
