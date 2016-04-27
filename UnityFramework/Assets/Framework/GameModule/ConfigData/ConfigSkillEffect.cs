using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public enum MoveEffectType
    {
        Target_Front = 1,
        Foward = 2,
        Backward = 3,
        Target_Back = 4,
    }

    public class ConfigSkillEffect
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected int mEffectID;
        public int EffectID
        {
            get { return mEffectID; }
        }

        protected string mEffectName;
        public string EffectName
        {
            get { return mEffectName; }
        }

        protected int mEffectLevel;
        public int EffectLevel
        {
            get { return mEffectLevel; }
        }

        protected double mEffectDelayTime;
        public double EffectDelayTime
        {
            get { return mEffectDelayTime; }
        }

        protected int mCastActionID;
        public int CastActionID
        {
            get { return mCastActionID; }
        }

        protected int mCastActEffID;
        public int CastActEffID
        {
            get { return mCastActEffID; }
        }

        protected int mFlyResourceID;
        public int FlyResourceID
        {
            get { return mFlyResourceID; }
        }

        protected int mMoveResourceID;
        public int MoveResourceID
        {
            get { return mMoveResourceID; }
        }

        protected int mHitActID;
        public int HitActID
        {
            get { return mHitActID; }
        }

        protected int mHitActEffID;
        public int HitActEffID
        {
            get { return mHitActEffID; }
        }

        protected int mEndActID;
        public int EndActID
        {
            get { return mEndActID; }
        }

        protected int mEndActEffID;
        public int EndActEffID
        {
            get { return mEndActEffID; }
        }

        protected int mEffectType;
        public int EffectType
        {
            get { return mEffectType; }
        }

        protected int mEffectBulletSpead;
        public int EffectBulletSpead
        {
            get { return mEffectBulletSpead; }
        }

        protected int mEffectAffectObjMethod;
        public int EffectAffectObjMethod
        {
            get { return mEffectAffectObjMethod; }
        }

        protected int mEffectAffectObjRedius;
        public int EffectAffectObjRedius
        {
            get { return mEffectAffectObjRedius; }
        }

        protected int mEffectAffectObjNum;
        public int EffectAffectObjNum
        {
            get { return mEffectAffectObjNum; }
        }

        protected int mEffectTargetSelect;
        public int EffectTargetSelect
        {
            get { return mEffectTargetSelect; }
        }

        protected int mSkillDisturbPlaySkill;
        public int SkillDisturbPlaySkill
        {
            get { return mSkillDisturbPlaySkill; }
        }

        protected int mEffectHPChangeType;
        public int EffectHPChangeType
        {
            get { return mEffectHPChangeType; }
        }

        protected int mEffectForumID;
        public int EffectForumID
        {
            get { return mEffectForumID; }
        }

        protected int mEffectSkillArg;
        public int EffectSkillArg
        {
            get { return mEffectSkillArg; }
        }

        protected int mEffectExtraValue;
        public int EffectExtraValue
        {
            get { return mEffectExtraValue; }
        }

        //protected long mEffectLevelUpGainExtrVal;
        //public long EffectLevelUpGainExtrVal
        //{
        //    get { return mEffectLevelUpGainExtrVal; }
        //}

        protected double mEffectHPAbsortArg;
        public double EffectHPAbsortArg
        {
            get { return mEffectHPAbsortArg; }
        }

        protected int mEffectStateID;
        public int EffectStateID
        {
            get { return mEffectStateID; }
        }

        protected int mEffectRemoveStateID;
        public int EffectRemoveStateID
        {
            get { return mEffectRemoveStateID; }
        }

        protected int mEffectRemoveStateProb;
        public int EffectRemoveStateProb
        {
            get { return mEffectRemoveStateProb; }
        }

        protected int mMoveEffectType;
        public int MoveEffectType
        {
            get { return mMoveEffectType; }
        }

        protected int mMoveEffectDistance;
        public int MoveEffectDistance
        {
            get { return mMoveEffectDistance; }
        }

        protected int mMoveEffectSpeed;
        public int MoveEffectSpeed
        {
            get { return mMoveEffectSpeed; }
        }

        protected int mTargetMoveEffectType;
        public int TargetMoveEffectType
        {
            get { return mTargetMoveEffectType; }
        }

        protected int mTargetMoveEffectDistance;
        public int TargetMoveEffectDistance
        {
            get { return mTargetMoveEffectDistance; }
        }

        protected int mTargetMoveEffectSpeed;
        public int TargetMoveEffectSpeed
        {
            get { return mTargetMoveEffectSpeed; }
        }

        public bool ImportData(Dictionary<string, string> data) 
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("序号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'序号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果编号'的字段");
                    return false;
                }
                mEffectID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("效果名称", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果名称'的字段");
                    return false;
                }
                mEffectName = tmp;

                if (!data.TryGetValue("效果等级", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果等级'的字段");
                    return false;
                }
                mEffectLevel = Convert.ToInt32(tmp);

                if (!data.TryGetValue("效果延迟时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果延迟时间'的字段");
                    return false;
                }
                mEffectDelayTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("施放动作编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'施放动作编号'的字段");
                    return false;
                }
                mMoveResourceID = mCastActionID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("施放特效编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'施放特效编号'的字段");
                    return false;
                }
                mCastActEffID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("飞行资源编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'飞行资源编号'的字段");
                    return false;
                }
                mFlyResourceID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("命中动作编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'命中动作编号'的字段");
                    return false;
                }
                mHitActID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("命中特效编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'命中特效编号'的字段");
                    return false;
                }
                mHitActEffID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("结束动作编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'结束动作编号'的字段");
                    return false;
                }
                mEndActID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("结束特效编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'结束特效编号'的字段");
                    return false;
                }
                mEndActEffID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("效果类型", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果类型'的字段");
                    return false;
                }
                mEffectType = Convert.ToInt32(tmp);

                if (!data.TryGetValue("弹道速度", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'弹道速度'的字段");
                    return false;
                }
                mEffectBulletSpead = Convert.ToInt32(tmp);

                if (!data.TryGetValue("影响对象方式", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'影响对象方式'的字段");
                    return false;
                }
                mEffectAffectObjMethod = Convert.ToInt32(tmp);

                if (!data.TryGetValue("影响对象半径", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'影响对象半径'的字段");
                    return false;
                }
                mEffectAffectObjRedius = Convert.ToInt32(tmp);

                if (!data.TryGetValue("影响对象数目", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'影响对象数目'的字段");
                    return false;
                }
                mEffectAffectObjNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("效果目标筛选", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'效果目标筛选'的字段");
                    return false;
                }
                mEffectTargetSelect = Convert.ToInt32(tmp);

                if (!data.TryGetValue("打断施法", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'打断施法'的字段");
                    return false;
                }
                mSkillDisturbPlaySkill = Convert.ToInt32(tmp);

                if (!data.TryGetValue("HP改变类型", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'HP改变类型'的字段");
                    return false;
                }
                mEffectHPChangeType = Convert.ToInt32(tmp);

                if (!data.TryGetValue("公式编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'公式编号'的字段");
                    return false;
                }
                mEffectForumID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("技能系数", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'技能系数'的字段");
                    return false;
                }
                mEffectSkillArg = Convert.ToInt32(tmp);

                if (!data.TryGetValue("额外值", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'额外值'的字段");
                    return false;
                }
                mEffectExtraValue = Convert.ToInt32(tmp);

                //if (!data.TryGetValue("升级增加额外值", out tmp))
                //{
                //    DebugMod.Log("ConfigSkillEffect 没有名为'升级增加额外值'的字段");
                //    return false;
                //}
                //mEffectLevelUpGainExtrVal = Convert.ToInt64(tmp);

                if (!data.TryGetValue("HP吸收系数", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'HP吸收系数'的字段");
                    return false;
                }
                mEffectHPAbsortArg = Convert.ToDouble(tmp);

                if (!data.TryGetValue("状态编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'状态编号'的字段");
                    return false;
                }
                mEffectStateID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("移除状态编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'移除状态编号'的字段");
                    return false;
                }
                mEffectRemoveStateID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("移除状态性质", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'移除状态性质'的字段");
                    return false;
                }
                mEffectRemoveStateProb = Convert.ToInt32(tmp);

                if (!data.TryGetValue("自己位移效果", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移效果'的字段");
                    return false;
                }
                mMoveEffectType = Convert.ToInt32(tmp);

                if (!data.TryGetValue("自己位移距离", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移距离'的字段");
                    return false;
                }
                mMoveEffectDistance = Convert.ToInt32(tmp);

                if (!data.TryGetValue("自己位移速度", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移速度'的字段");
                    return false;
                }
                mMoveEffectSpeed = Convert.ToInt32(tmp);

                if (!data.TryGetValue("目标位移效果", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移效果'的字段");
                    return false;
                }
                mTargetMoveEffectType = Convert.ToInt32(tmp);

                if (!data.TryGetValue("目标位移距离", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移距离'的字段");
                    return false;
                }
                mTargetMoveEffectDistance = Convert.ToInt32(tmp);

                if (!data.TryGetValue("目标位移速度", out tmp))
                {
                    DebugMod.Log("ConfigSkillEffect 没有名为'位移速度'的字段");
                    return false;
                }
                mTargetMoveEffectSpeed = Convert.ToInt32(tmp);
            }
            catch(Exception ex)
            {
                DebugMod.Log("ConfigSkillEffect 读取数据异常" + ",ID" + mEffectID);
                DebugMod.Log(ex);

                return false;
            }

            return true;
        }
    }
}
