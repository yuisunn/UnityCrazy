using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.Unity;
using SPSGame.CsShare.Data;

//场景内设施
namespace SPSGame
{
    public enum SfxObjState
    {
        NotShow,
        Showing,
        Over,
    }

    public class SfxObjData
    {
        public long ShowTick;
        public int srcSpriteID;
        public int lifetime;        //micro seconds
        public int ResID;
        public int creatureType;
    }

    public class SfxObj : SceneObject
    {
        protected SfxObjData m_data = null;
        protected SfxObjState m_State = SfxObjState.NotShow;
        protected long m_CreateTick = 0;
        public SfxObj(int spriteid)
            :base(spriteid)
        {

        }

        public bool IsOver()
        {
            return m_State == SfxObjState.Over;
        }

        public void Create(SfxObjData data)
        {
            m_data = data;
            m_State = SfxObjState.NotShow;
            mEnable = true;
            m_CreateTick = DateTime.Now.Ticks / 10000;
            //DebugMod.Log(string.Format("Sfx Created {0}:{1}:{2}:{3}", SpriteID, m_data.creatureType, m_data.ResID, m_data.srcSpriteID));
        }

        public override void OnUpdate(long tickNow)
        {
            if (IsOver())
                return;

            if (null == m_data)
            {
                m_State = SfxObjState.Over;
                return;
            }

            if (tickNow < m_data.ShowTick)  //还没到开始显示的时刻
                return;

            if (m_data.lifetime > 0 && tickNow > m_data.ShowTick + m_data.lifetime)
            {
                m_State = SfxObjState.Over;    //如果lifetime为预设，则结束时不需要通知unity清除特效
                //DebugMod.Log(string.Format("Sfx Is Over {0}:{1}:{2}:{3}", SpriteID, m_data.creatureType, m_data.ResID, m_data.srcSpriteID));
                return;
            }

            ShowSfx();
        }

        protected void ShowSfx()
        {
            if (m_State != SfxObjState.NotShow)
                return;

            if (m_data.creatureType == (int)ECreatureType.Bullet)
                return;

            if (m_data.srcSpriteID == 0)
                return;

            ActionParam param = new ActionParam();
            param["CreatureID"] = SpriteID;
            param["CreatureType"] = m_data.creatureType;
            param["ResID"] = m_data.ResID;
            param["SpriteID"] = m_data.srcSpriteID;
            param["TargetSpriteID"] = 0;
            param["Speed"] = 0;
            param["Life"] = (m_data.lifetime < 0) ? -1 : (float)(m_data.lifetime / 1000.0f);

            Local.Instance.CallUnityAction(UnityActionDefine.LoadCreature, param);

            m_State = SfxObjState.Showing;

            //DebugMod.Log(string.Format("showsfx {0}:{1}:{2}:{3}", SpriteID, m_data.creatureType, m_data.ResID, m_data.srcSpriteID));
        }

        public void KillSfx()
        {
            ActionParam param = new ActionParam();
            param["CreatureID"] = SpriteID;
            Local.Instance.CallUnityAction(UnityActionDefine.KillCreature, param);
        }
    }
}
