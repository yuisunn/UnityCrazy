using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigSkillLevelUp
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected int mSkillID;
        public int SkillID
        {
            get { return mSkillID; }
        }

        protected long mSkillClass;
        public long SkillClass
        {
            get { return mSkillClass; }
        }

        protected string mSkillName;
        public string SkillName
        {
            get { return mSkillName; }
        }

        protected short mRoleLimitation;
        public short RoleLimitation
        {
            get { return mRoleLimitation; }
        }

        protected short mSkillStage;
        public short SkillStage
        {
            get { return mSkillStage; }
        }

        protected int mSkillLevel;
        public int SkillLevel
        {
            get { return mSkillLevel; }
        }

        protected int mRoleLevelRequirement;
        public int RoleLevelRequirement
        {
            get { return mRoleLevelRequirement; }
        }

        protected short mLevelUpConsumeMoneyType;
        public short LevelUpConsumeMoneyType
        {
            get { return mLevelUpConsumeMoneyType; }
        }

        protected int mLevelUpConsumeMoneyNum;
        public int LevelUpConsumeMoneyNum
        {
            get { return mLevelUpConsumeMoneyNum; }
        }

        protected string mLevelUpProbMsg1;
        public string LevelUpProbMsg1
        {
            get { return mLevelUpProbMsg1; }
        }

        protected string mLevelUpProbMsg2;
        public string LevelUpProbMsg2
        {
            get { return mLevelUpProbMsg2; }
        }

        protected string mLevelUpProbMsg3;
        public string LevelUpProbMsg3
        {
            get { return mLevelUpProbMsg3; }
        }

        protected string mLevelUpMsg1;
        public string LevelUpMsg1
        {
            get { return mLevelUpMsg1; }
        }

        protected string mLevelUpMsg2;
        public string LevelUpMsg2
        {
            get { return mLevelUpMsg2; }
        }

        protected string mLevelUpMsg3;
        public string LevelUpMsg3
        {
            get { return mLevelUpMsg3; }
        }

        public bool ImportData(Dictionary<string, string> data)//每一个实例存储csv文件中的一行
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("序号", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'序号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("技能编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'技能编号'的字段");
                    return false;
                }
                mSkillID = Convert.ToInt32(tmp);

                //if (!data.TryGetValue("技能类", out tmp))
                //{
                //    DebugMod.Log("ConfigSkillLevelUp 没有名为'技能类'的字段");
                //    return false;
                //}
                //mSkillClass = Convert.ToInt64(tmp);

                if (!data.TryGetValue("技能名称", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'技能名称'的字段");
                    return false;
                }
                mSkillName = tmp;

                if (!data.TryGetValue("角色限制", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'角色限制'的字段");
                    return false;
                }
                mRoleLimitation = Convert.ToInt16(tmp);

                if (!data.TryGetValue("技能阶位", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'技能阶位'的字段");
                    return false;
                }
                mSkillStage = Convert.ToInt16(tmp);

                if (!data.TryGetValue("技能等级", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'技能等级'的字段");
                    return false;
                }
                mSkillLevel = Convert.ToInt32(tmp);

                if (!data.TryGetValue("角色等级要求", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'角色等级要求'的字段");
                    return false;
                }
                mRoleLevelRequirement = int.Parse(tmp);

                if (!data.TryGetValue("升级消耗货币类型", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'升级消耗货币类型'的字段");
                    return false;
                }


                mLevelUpConsumeMoneyType = Convert.ToInt16(tmp);

                if (!data.TryGetValue("升级消耗货币数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'升级消耗货币数值'的字段");
                    return false;
                }
                mLevelUpConsumeMoneyNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("属性信息1", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'属性信息1'的字段");
                    return false;
                }
                mLevelUpProbMsg1 = tmp;
                if (!data.TryGetValue("属性信息2", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'属性信息2'的字段");
                    return false;
                }
                mLevelUpProbMsg2 = tmp;
                if (!data.TryGetValue("属性信息3", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'属性信息3'的字段");
                    return false;
                }
                mLevelUpProbMsg3 = tmp;


                if (!data.TryGetValue("升级信息1", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'升级信息1'的字段");
                    return false;
                }
                mLevelUpMsg1 = tmp;
                if (!data.TryGetValue("升级信息2", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'升级信息2'的字段");
                    return false;
                }
                mLevelUpMsg2 = tmp;
                if (!data.TryGetValue("升级信息3", out tmp))
                {
                    DebugMod.Log("ConfigSkillLevelUp 没有名为'升级信息3'的字段");
                    return false;
                }
                mLevelUpMsg3 = tmp;


                // 待续
            }
            catch (Exception ex)
            {
                DebugMod.Log("ConfigSkillLevelUp 读取数据异常" + ",ID" + mSkillID);
                DebugMod.Log(ex);

                return false;
            }

            return true;
        }



    }
}
