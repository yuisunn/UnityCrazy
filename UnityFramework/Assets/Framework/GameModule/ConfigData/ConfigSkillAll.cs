using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class SkillEffectCol
    {
        public int EffectID;
        public double EffectTime;
        public double EffectOdds;
    }

    public class ConfigSkillAll
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

        //protected int mSkillClass;
        //public int SkillClass
        //{
        //    get { return mSkillClass; }
        //}

        protected int mRoleLimitation;
        public int RoleLimitation
        {
            get { return mRoleLimitation; }
        }

        protected int mSkillPosID;
        public int SkillPosID
        {
            get { return mSkillPosID; }
        }

        protected string mSkillIconID;
        public string SkillIconID
        {
            get { return mSkillIconID; }
        }

        protected string mSkillName;
        public string SkillName
        {
            get { return mSkillName; }
        }

        protected string mSkillFunctionDetails;
        public string SkillFunctionDetails
        {
            get { return mSkillFunctionDetails; }
        }

        protected int mSkillSingActionID;
        public int SkillSingActionID
        {
            get { return mSkillSingActionID; }
        }

        protected int mSkillSingActionEffectID;
        public int SkillSingActionEffectID
        {
            get { return mSkillSingActionEffectID; }
        }

        protected int mSkillTypeID;
        public int SkillTypeID
        {
            get { return mSkillTypeID; }
        }

        protected int mSkillPlayMethod;
        public int SkillPlayMethod
        {
            get { return mSkillPlayMethod; }
        }

        protected int mSkillTargetSelect;
        public int SkillTargetSelect
        {
            get { return mSkillTargetSelect; }
        }

        protected int mSkillAttactDistance;
        public int SkillAttactDistance
        {
            get { return mSkillAttactDistance; }
        }

        protected double mSkillSingTime;
        public double SkillSingTime
        {
            get { return mSkillSingTime; }
        }

        protected double mSkillGuideTime;
        public double SkillGuideTime
        {
            get { return mSkillGuideTime; }
        }

        protected double mSkillCoolingTime;
        public double SkillCoolingTime
        {
            get { return mSkillCoolingTime; }
        }

        protected double mSkillPublicCoolingTime;
        public double SkillPublicCoolingTime
        {
            get { return mSkillPublicCoolingTime; }
        }

        protected double mSkillEndTime;
        public double SkillEndTime
        {
            get { return mSkillEndTime; }
        }

        protected double mSkillYingzhiTime;
        public double SkillYingzhiTime
        {
            get { return mSkillYingzhiTime; }
        }
        
        protected int mSkillEnergyConsume;
        public int SkillEnergyConsume
        {
            get { return mSkillEnergyConsume; }
        }

        protected List<SkillEffectCol> mEffectCols = new List<SkillEffectCol>();
        public List<SkillEffectCol> EffectCols
        {
            get { return mEffectCols; }
        }

        //后续


        public bool ImportData(Dictionary<string, string> data)//每一个实例存储csv文件中的一行
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("序号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'序号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("技能编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能编号'的字段");
                    return false;
                }
                mSkillID = Convert.ToInt32(tmp);

                //if (!data.TryGetValue("技能类", out tmp))
                //{
                //    DebugMod.Log("ConfigSkillAll 没有名为'技能类'的字段");
                //    return false;
                //}
                //mSkillClass = Convert.ToInt32(tmp);

                if (!data.TryGetValue("角色限制", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'角色限制'的字段");
                    return false;
                }
                mRoleLimitation = Convert.ToInt32(tmp);

                if (!data.TryGetValue("位置编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'位置编号'的字段");
                    return false;
                }
                mSkillPosID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("图标编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'图标编号'的字段");
                    return false;
                }
                mSkillIconID = tmp;

                if (!data.TryGetValue("技能名称", out tmp))//daiding
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能名称'的字段");
                    return false;
                }
                mSkillName = tmp;

                if (!data.TryGetValue("技能描述", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能描述'的字段");
                    return false;
                }
                mSkillFunctionDetails = tmp;

                if (!data.TryGetValue("吟唱动作编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'吟唱动作编号'的字段");
                    return false;
                }
                mSkillSingActionID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吟唱特效编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'吟唱动作编号'的字段");
                    return false;
                }
                mSkillSingActionEffectID = Convert.ToInt32(tmp); 

                if (!data.TryGetValue("技能类型", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能类型'的字段");
                    return false;
                }
                mSkillTypeID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("释放方式", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'释放方式'的字段");
                    return false;
                }
                mSkillPlayMethod = Convert.ToInt32(tmp);

                if (!data.TryGetValue("技能目标筛选", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能目标筛选'的字段");
                    return false;
                }
                mSkillTargetSelect = Convert.ToInt32(tmp);

                if (!data.TryGetValue("攻击距离", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'攻击距离'的字段");
                    return false;
                }
                mSkillAttactDistance = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吟唱时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'吟唱时间'的字段");
                    return false;
                }
                mSkillSingTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("引导时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'引导时间'的字段");
                    return false;
                }
                mSkillGuideTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("冷却时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'冷却时间'的字段");
                    return false;
                }
                mSkillCoolingTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("公共冷却时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'公共冷却时间'的字段");
                    return false;
                }

                mSkillPublicCoolingTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("技能持续时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能持续时间'的字段");
                    return false;
                }
                mSkillEndTime = Math.Max(0.5, Convert.ToDouble(tmp));

                if (!data.TryGetValue("技能硬直时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'技能硬直时间'的字段");
                    return false;
                }
                mSkillYingzhiTime = Math.Max(0.01, Convert.ToDouble(tmp));

                if (!data.TryGetValue("能量消耗", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'能量消耗'的字段");
                    return false;
                }
                mSkillEnergyConsume = Convert.ToInt32(tmp);

                SkillEffectCol col = new SkillEffectCol();
                if (!data.TryGetValue("效果时间点1", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点1'的字段");
                    return false;
                }
                col.EffectTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("效果时间点1概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点1概率'的字段");
                    return false;
                }
                col.EffectOdds = Convert.ToDouble(tmp);

                if (!data.TryGetValue("时间点1效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'时间点1效果编号'的字段");
                    return false;
                }
                col.EffectID = Convert.ToInt32(tmp);

                mEffectCols.Add(col);
                col = new SkillEffectCol();

                if (!data.TryGetValue("效果时间点2", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点2'的字段");
                    return false;
                }
                col.EffectTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("效果时间点2概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点2概率'的字段");
                    return false;
                }
                col.EffectOdds = Convert.ToDouble(tmp);

                if (!data.TryGetValue("时间点2效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'时间点2效果编号'的字段");
                    return false;
                }
                col.EffectID = Convert.ToInt32(tmp);

                mEffectCols.Add(col);
                col = new SkillEffectCol();

                if (!data.TryGetValue("效果时间点3", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点3'的字段");
                    return false;
                }
                col.EffectTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("效果时间点3概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点3概率'的字段");
                    return false;
                }
                col.EffectOdds = Convert.ToDouble(tmp);

                if (!data.TryGetValue("时间点3效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'时间点3效果编号'的字段");
                    return false;
                }
                col.EffectID = Convert.ToInt32(tmp);

                mEffectCols.Add(col);

                col = new SkillEffectCol();

                if (!data.TryGetValue("效果时间点4", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点4'的字段");
                    return false;
                }
                col.EffectTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("效果时间点4概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点4概率'的字段");
                    return false;
                }
                col.EffectOdds = Convert.ToDouble(tmp);

                if (!data.TryGetValue("时间点4效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'时间点4效果编号'的字段");
                    return false;
                }
                col.EffectID = Convert.ToInt32(tmp);

                mEffectCols.Add(col);
                col = new SkillEffectCol();

                if (!data.TryGetValue("效果时间点5", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点5'的字段");
                    return false;
                }
                col.EffectTime = Convert.ToDouble(tmp);

                if (!data.TryGetValue("效果时间点5概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'效果时间点5概率'的字段");
                    return false;
                }
                col.EffectOdds = Convert.ToDouble(tmp);

                if (!data.TryGetValue("时间点5效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillAll 没有名为'时间点5效果编号'的字段");
                    return false;
                }
                col.EffectID = Convert.ToInt32(tmp);

                mEffectCols.Add(col);
                // 待续
            }
            catch (Exception ex)
            {
                DebugMod.Log("ConfigSkllAll 读取数据异常" + ",ID" + mSkillID);
                DebugMod.Log(ex);

                return false;
            }

            return true;
        }
    }
}
