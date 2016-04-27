using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame.GameModule
{
    public class OtherPlayerMove : GameModuleBase
    {
        public OtherPlayerMove()
        {

        }

        public void OnServerStopMove(long charid, ParamStopMove dd)
        {
            if (null == dd)
                return;

            GameScene myscene = Local.Instance.SceneMgr.GetMyScene();
            if (null == myscene)
                return;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null == myplayer)
                return;

            if (charid == myplayer.CharID)
                return;

            PlayerObj player = myscene.playerMng.GetPlayer(charid);
            if (null == player)
                return;

            player.SetPos(dd.CurrX, player.Y, dd.CurrZ);
            player.StopMove();
        }

        public void OnLogicMoveTo(long charid, ParamMoveTo dd)
        {
            if (null == dd)
                return;

            GameScene myscene = Local.Instance.SceneMgr.GetMyScene();
            if (null == myscene)
                return;

            MyPlayer myplayer = Local.Instance.GetMyPlayer;
            if (null == myplayer)
                return;

            if (charid == myplayer.CharID)
                return;

            PlayerObj player = myscene.playerMng.GetPlayer(charid);
            if (null == player)
                return;

            player.SetPos(dd.CurrX, player.Y, dd.CurrZ);
            player.SetMoveDest(dd.DestX, dd.DestZ, true);
        }
    }
}
