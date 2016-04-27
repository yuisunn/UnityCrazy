using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.CsShare.Data;
using SPSGame.Unity;

//场景内设施
namespace SPSGame
{
    public class SfxObjManager
    {
        public Dictionary<int, SfxObj> m_ObjDic = new Dictionary<int, SfxObj>();
        protected List<int> m_RemoveKeys = new List<int>();
        public void OnUpdate(long tickNow)
        {
            m_RemoveKeys.Clear();
            foreach(var pair in m_ObjDic)
            {
                if (pair.Value.Enable)
                {
                    pair.Value.OnUpdate(tickNow);
                    if (pair.Value.IsOver())
                        m_RemoveKeys.Add(pair.Key);
                }
            }

            foreach(var key in m_RemoveKeys)
            {
                m_ObjDic.Remove(key);
            }
        }

        public void ClearAll()
        {
            foreach (var data in m_ObjDic)
            {
                SfxObjFactory.Instance.Push(data.Value);
            }
            m_ObjDic.Clear();
        }

        public SfxObj GetObj(int spriteid)
        {
            SfxObj obj = null;
            m_ObjDic.TryGetValue(spriteid, out obj);
            return obj;
        }

        public SfxObj CreateSfxObj(SfxObjData data)
        {
            SfxObj newObj = SfxObjFactory.Instance.Pop();
            if (null == newObj)
                return null;

            m_ObjDic[newObj.SpriteID] = newObj;
            newObj.Create(data);
            return newObj;
        }
    }
}
