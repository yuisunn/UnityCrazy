using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.CsShare.Data;
using SPSGame.Tools;

namespace SPSGame
{
    public class SfxObjFactory : Singleton<SfxObjFactory>
    {
        protected int m_InitNum;
        protected int m_MaxSeq;

        protected Queue<SfxObj> m_ObjPool = new Queue<SfxObj>();
        public void Init(int num)
        {
            m_InitNum = num;
            for (int i = ClientDefine.MinSfxSpriteID; i <= ClientDefine.MinSfxSpriteID + num; i++)
            {
                SfxObj obj = new SfxObj(i);
                m_ObjPool.Enqueue(obj);
                m_MaxSeq = i;
            }
        }

        public SfxObj Pop()
        {
            lock (m_ObjPool)
            {
                if (m_ObjPool.Count == 0)
                {
                    for (int i = 1; i <= m_InitNum; i++)
                    {
                        m_MaxSeq++;
                        SfxObj obj = new SfxObj(m_MaxSeq);
                        m_ObjPool.Enqueue(obj);
                    }
                }
                return m_ObjPool.Dequeue();
            }
        }

        public void Push(SfxObj obj)
        {
            obj.Clear();
            lock (m_ObjPool)
            {
                m_ObjPool.Enqueue(obj);
            }
        }
    }
}
