using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame
{
    public class MonsterFactory : Singleton<MonsterFactory>
    {
        protected int m_InitNum;
        protected int m_MaxSeq;

        protected Queue<MonsterObj> m_MonsterPool = new Queue<MonsterObj>();
        public void Init(int num)
        {
            m_InitNum = num;
            for (int i = ClientDefine.MinMonsterSpriteID; i <= ClientDefine.MinMonsterSpriteID + num; i++)
            {
                MonsterObj obj = new MonsterObj(i);
                m_MonsterPool.Enqueue(obj);
                m_MaxSeq = i;
            }
        }

        public MonsterObj Pop()
        {
            lock (m_MonsterPool)
            {
                if (m_MonsterPool.Count == 0)
                {
                    for (int i = 1; i <= m_InitNum; i++)
                    {
                        m_MaxSeq++;
                        MonsterObj obj = new MonsterObj(m_MaxSeq);
                        m_MonsterPool.Enqueue(obj);
                    }
                }
                return m_MonsterPool.Dequeue();
            }
        }

        public void Push(MonsterObj obj)
        {
            obj.Clear();
            lock (m_MonsterPool)
            {
                m_MonsterPool.Enqueue(obj);
            }
        }
    }
}
