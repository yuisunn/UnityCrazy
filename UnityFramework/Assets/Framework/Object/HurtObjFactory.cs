using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame
{
    public class HurtObjFactory : Singleton<HurtObjFactory>
    {
        protected int m_InitNum;
        protected int m_MaxSeq;

        protected Queue<HurtObj> m_HurtPool = new Queue<HurtObj>();
        public void Init(int num)
        {
            m_InitNum = num;
            for (int i = ClientDefine.MinHurtSpriteID; i <= ClientDefine.MinHurtSpriteID + num; i++)
            {
                HurtObj obj = new HurtObj(i);
                m_HurtPool.Enqueue(obj);
                m_MaxSeq = i;
            }
        }

        public HurtObj Pop()
        {
            lock (m_HurtPool)
            {
                if (m_HurtPool.Count == 0)
                {
                    for (int i = 1; i <= m_InitNum; i++)
                    {
                        m_MaxSeq++;
                        HurtObj obj = new HurtObj(m_MaxSeq);
                        m_HurtPool.Enqueue(obj);
                    }
                }
                return m_HurtPool.Dequeue();
            }
        }

        public void Push(HurtObj obj)
        {
            obj.Clear();
            lock (m_HurtPool)
            {
                m_HurtPool.Enqueue(obj);
            }
        }
    }
}
