using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.CsShare.Data;

//伤害（包括闪避）效果
namespace SPSGame
{
    public class HurtData
    {
        public long hurtID;
        public long actTick;
        public long srcID;
        public bool srcIsPlayer;
        public SkillTarget target;
        public ConfigSkillEffect effectCfg;
    }

    public class HurtObjManager
    {
        public Dictionary<int, HurtObj> m_ObjDic = new Dictionary<int, HurtObj>();
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

        public HurtObj FindMyTarget(long myID, long targetID, bool targetIsPlayer)
        {
            HurtObj obj = null;
            foreach(var pair in m_ObjDic)
            {
                obj = pair.Value;
                if (obj.IsMyTarget(myID, targetID, targetIsPlayer))
                    return obj;
            }
            return null;
        }

        public void ClearAll()
        {
            foreach (var data in m_ObjDic)
            {
                HurtObjFactory.Instance.Push(data.Value);
            }
            m_ObjDic.Clear();
        }

        public HurtObj GetObj(int spriteid)
        {
            HurtObj obj = null;
            m_ObjDic.TryGetValue(spriteid, out obj);
            return obj;
        }

        public HurtObj CreateHurtObj(HurtData data)
        {
            HurtObj newObj = HurtObjFactory.Instance.Pop();
            if (null == newObj)
                return null;

            data.hurtID = newObj.SpriteID;
            m_ObjDic[newObj.SpriteID] = newObj;
            newObj.Create(data);
            return newObj;
        }
    }
}
