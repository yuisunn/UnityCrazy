using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class MonsterManager
    {
        protected Dictionary<int, int> m_Sprite2Seq = new Dictionary<int, int>();
        protected Dictionary<int, MonsterObj> m_ObjDic = new Dictionary<int, MonsterObj>();

        public void OnUpdate(long tickNow)
        {
            List<int> listReq = null;
            foreach (var obj in m_ObjDic)
            {
                if (obj.Value.Enable)
                {
                    obj.Value.OnUpdate(tickNow);
                    if (obj.Value.ReqInfoIfNeed(tickNow))
                    {
                        if (null == listReq)
                            listReq = new List<int>();
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
                    gamesvr.ReadySend(CsShare.SPSCmd.CMD_CS_NearbyMonster, ActParam, null);
                }
            }
        }

        public void ClearAll()
        {
            ActionParam param = new ActionParam();
            param["SpriteID"] = -1;
            Local.Instance.CallUnityAction(UnityActionDefine.KillSprite, param);

            m_Sprite2Seq.Clear();
            foreach (var data in m_ObjDic)
            {
                MonsterFactory.Instance.Push(data.Value);
            }
            m_ObjDic.Clear();
        }

        public List<MonsterObj> GetDisableObj()
        {
            List<MonsterObj> list = new List<MonsterObj>();
            foreach(var pair in m_ObjDic)
            {
                if (!pair.Value.Enable)
                    list.Add(pair.Value);
            }
            return list;
        }

        public void EnableMonsters()
        {
            foreach (var pair in m_ObjDic)
            {
                MonsterObj obj = pair.Value;
                if (!obj.Enable)
                {
                    obj.SetEnable(true);
                }
            }
        }

        public int GetMonsterSeqID(int spriteid)
        {
            int seqid = 0;
            m_Sprite2Seq.TryGetValue(spriteid, out seqid);
            return seqid;
        }

        public MonsterObj GetMonster(int seqid)
        {
            MonsterObj monster = null;
            m_ObjDic.TryGetValue(seqid, out monster);
            return monster;
        }

        public MonsterObj Create(int seqid, int id)
        {
            MonsterObj newMonster = MonsterFactory.Instance.Pop();
            if (null == newMonster)
                return null;

            m_ObjDic[seqid] = newMonster;
            m_Sprite2Seq[newMonster.SpriteID] = seqid;
            newMonster.Create(seqid, id);
            return newMonster;
        }

        public void OnRcvNearbyID(GameScene scene, List<NearbyMonsterID> listData, bool enable)
        {
            foreach (var dd in listData)
            {
                MonsterObj monster = Create(dd.SeqID, dd.ID);
                if (null != monster)
                {
                    monster.SetSceneSeq(scene);
                    monster.SetPos(dd.X, dd.Y, dd.Z);
                    monster.SetSpeed(dd.Speed);
                    monster.SetMoveDest(dd.DestX, dd.DestZ);
                    monster.SetEnable(enable);
                    //if (dd.SeqID == 1)
                    //    SPSGame.Tools.DebugMod.Log(string.Format("NewMonster Moveto:{0}:{1}-->{2}:{3}", dd.X, dd.Z, dd.DestX, dd.DestZ));
                }
            }
        }
        
        public void RcvMonsterInfo (MonsterInfo info)
        {
            MonsterObj monster = GetMonster(info.Seq);
            if (null != monster)
                monster.SetInfo(info);
        }

        public void MonsterOut(int seqid)
        {
            MonsterObj monster = GetMonster(seqid);
            if (null != monster)
            {
                monster.KillSprite();
                m_Sprite2Seq.Remove(monster.SpriteID);
                m_ObjDic.Remove(seqid);
                MonsterFactory.Instance.Push(monster);
            }
        }

        public MonsterObj FindNearestMonster(float x, float y, float z, int maxDisQ)
        {
            MonsterObj nearest = null;
            int minDisq = 0;
            int disQ = 0;
            int diffX = 0;
            int diffZ = 0;
            foreach (var pair in m_ObjDic)
            {
                MonsterObj obj = pair.Value;
                if (!obj.Enable)
                    continue;
                if (!obj.Loaded)
                    continue;
                if (!obj.LastVisible)
                    continue;
                if ((short)obj.Y != (short)y)
                    continue;
                if (!obj.IsLive)
                    continue;

                diffX = (int)(obj.X - x);
                diffZ = (int)(obj.Z - z);
                disQ = diffX * diffX + diffZ * diffZ;
                if (disQ > maxDisQ)
                    continue;
                if (null == nearest || disQ < minDisq)
                {
                    minDisq = disQ;
                    nearest = obj;
                }
            }

            return nearest;
        }

        public MonsterObj FindDirMonster(short dirX, short dirZ, short x, short y, short z, int maxDisQ)
        {
            MonsterObj nearest = null;
            int minDisq = 0;
            int disQ = 0;
            int diffX = 0;
            int diffZ = 0;
            foreach (var pair in m_ObjDic)
            {
                MonsterObj obj = pair.Value;
                if (!obj.Enable)
                    continue;
                if (!obj.Loaded)
                    continue;
                if (!obj.LastVisible)
                    continue;
                if ((short)obj.Y != (short)y)
                    continue;
                if (!obj.IsLive)
                    continue;

                diffX = (int)(obj.X - x);
                diffZ = (int)(obj.Z - z);

                if (dirX * diffX < 0 || dirZ * diffZ < 0)
                    continue;       //朝向不对

                disQ = diffX * diffX + diffZ * diffZ;
                if (disQ > maxDisQ)
                    continue;       //超出范围

                if (null == nearest || disQ < minDisq)
                {
                    minDisq = disQ;
                    nearest = obj;
                }
            }

            return nearest;
        }
    }
}
