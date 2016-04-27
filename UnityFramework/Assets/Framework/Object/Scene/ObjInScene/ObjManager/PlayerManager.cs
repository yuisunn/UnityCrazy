using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class PlayerManager
    {
        protected Dictionary<int, long> m_Sprite2CharID = new Dictionary<int, long>();
        protected Dictionary<long, PlayerObj> m_ObjDic = new Dictionary<long, PlayerObj>();

        public void OnUpdate(long tickNow)
        {
            List<long> listReq = null;
            foreach (var obj in m_ObjDic)
            {
                if (obj.Value.Enable)
                {
                    obj.Value.OnUpdate(tickNow);
                    if (obj.Value.ReqInfoIfNeed(tickNow))
                    {
                        if (null == listReq)
                            listReq = new List<long>();
                        listReq.Add(obj.Key);
                        if (listReq.Count >= 32)
                            break;
                    }
                }
            }

            if (null != listReq)
            {
                GameServer gamesvr = (GameServer)Local.Instance.SvrMgr.GetServer("game");
                if (null != gamesvr)
                {
                    ActionParam ActParam = new ActionParam();
                    ActParam["listReq"] = listReq;
                    gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_NearbyPlayer, ActParam, null);
                }
            }
        }

        public void ClearAll()
        {
            ActionParam param = new ActionParam();
            param["SpriteID"] = -1;
            Local.Instance.CallUnityAction(UnityActionDefine.KillSprite, param);

            m_Sprite2CharID.Clear();
            foreach (var data in m_ObjDic)
            {
                PlayerFactory.Instance.Push(data.Value);
            }
            m_ObjDic.Clear();
        }

        public PlayerObj GetPlayer(long charid)
        {
            PlayerObj player = null;
            m_ObjDic.TryGetValue(charid, out player);
            return player;
        }

        public long GetCharID(int spriteid)
        {
            long charid = 0;
            m_Sprite2CharID.TryGetValue(spriteid, out charid);
            return charid;
        }

        protected List<PlayerObj> GetDisableObj()
        {
            List<PlayerObj> list = new List<PlayerObj>();
            foreach (var pair in m_ObjDic)
            {
                if (!pair.Value.Enable)
                    list.Add(pair.Value);
            }
            return list;
        }

        public void EnablePlayers()
        {
            foreach (var pair in m_ObjDic)
            {
                PlayerObj player = pair.Value;
                if (!player.Enable)
                {
                    player.SetEnable(true);
                }
            }
        }

        public void OnRcvNearbyID(GameScene scene, List<NearbyPlayerID> listData, bool enable)
        {
            foreach (var dd in listData)
            {
                PlayerObj player = Create(dd.CharID, dd.CharClass);
                if (null != player)
                {
                    player.SetSceneSeq(scene);
                    player.SetPos(dd.X, dd.Y, dd.Z);
                    player.SetSpeed(dd.Speed);
                    player.SetMoveDest(dd.DestX, dd.DestZ);
                    player.SetEnable(enable);
                }
            }
        }

        public void RcvPlayerInfo(OtherPlayerInfo info)
        {
            PlayerObj player = GetPlayer(info.CharID);
            if (null == player)
                return;

            player.SetInfo(info);
        }

        protected PlayerObj Create(long charid, short charclass)
        {
            PlayerObj newPlayer = PlayerFactory.Instance.Pop();
            if (null == newPlayer)
                return null;

            m_ObjDic[charid] = newPlayer;
            m_Sprite2CharID[newPlayer.SpriteID] = charid;

            newPlayer.Create(charid, charclass);
            return newPlayer;
        }

        public void PlayerOut(long charid)
        {
            PlayerObj player = GetPlayer(charid);
            if (null != player)
            {
                player.KillSprite();
                m_Sprite2CharID.Remove(player.SpriteID);
                m_ObjDic.Remove(charid);
                PlayerFactory.Instance.Push(player);
            }
        }
    }
}
