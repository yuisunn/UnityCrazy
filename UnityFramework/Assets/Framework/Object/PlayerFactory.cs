using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame
{
    public class PlayerFactory : Singleton<PlayerFactory>
    {
        protected int m_InitNum;
        protected int m_MaxSeq;
        protected Queue<PlayerObj> m_Pool = new Queue<PlayerObj>();
        public void Init(int num)
        {
            m_InitNum = num;
            for (int i = ClientDefine.MinOtherPlayerSpriteID; i <= ClientDefine.MinOtherPlayerSpriteID + num; i++)
            {
                PlayerObj obj = new PlayerObj(i);
                m_Pool.Enqueue(obj);
                m_MaxSeq = i;
            }
        }

        public PlayerObj Pop()
        {
            lock (m_Pool)
            {
                if (m_Pool.Count == 0)
                {
                    for (int i = 1; i <= m_InitNum; i++)
                    {
                        m_MaxSeq++;
                        if (m_MaxSeq >= ClientDefine.MinMonsterSpriteID)
                            break;
                        PlayerObj obj = new PlayerObj(m_MaxSeq);
                        m_Pool.Enqueue(obj);
                    }
                }
                return m_Pool.Dequeue();
            }
        }

        public void Push(PlayerObj obj)
        {
            obj.Clear();
            lock (m_Pool)
            {
                m_Pool.Enqueue(obj);
            }
        }
    }
}
