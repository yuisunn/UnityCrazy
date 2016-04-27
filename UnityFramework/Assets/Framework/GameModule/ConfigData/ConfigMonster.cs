using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigMonster
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected string mName;
        public string Name
        {
            get { return mName; }
        }

        protected int mAIType;
        public int AIType
        {
            get { return mAIType; }
        }

        protected int mStaticAttr;
        public int StaticAttr
        {
            get { return mStaticAttr; }
        }

        protected int mDynAttr;
        public int DynAttr
        {
            get { return mDynAttr; }
        }

        protected int mAttackRange;
        public int AttackRange
        {
            get { return mAttackRange; }
        }

        protected int mMoveSpeed;
        public int MoveSpeed
        {
            get { return mMoveSpeed; }
        }

        public bool ImportData(Dictionary<string, string> data)
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("怪物编号", out tmp))
                {
                    DebugMod.Log("ConfigMonsterAttr 没有名为'怪物编号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("攻击距离", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'攻击距离'的字段");
                    return false;
                }
                mAttackRange = Convert.ToInt32(tmp);

                if (!data.TryGetValue("移动速度", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'移动速度'的字段");
                    return false;
                }
                mMoveSpeed = Convert.ToInt32(tmp);

                if (!data.TryGetValue("AI类型", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'AI类型'的字段");
                    return false;
                }
                mAIType = Convert.ToInt32(tmp);

                if (!data.TryGetValue("名称", out mName))
                {
                    DebugMod.Log("ConfigScene 没有名为'名称'的字段");
                    return false;
                }

                if (!data.TryGetValue("静态属性", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'静态属性'的字段");
                    return false;
                }
                mStaticAttr = Convert.ToInt32(tmp);

                if (!data.TryGetValue("动态属性", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'动态属性'的字段");
                    return false;
                }
                mDynAttr = Convert.ToInt32(tmp);

            }
            catch (Exception ex)
            {
                DebugMod.Log("ConfigMonsterAttr 读取数据异常" + ",ID=" + mID);
                DebugMod.Log(ex);
                return false;
            }
            return true;
        }
    }
}
