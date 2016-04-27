using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigSkillState
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected int mStateID;
        public int StateID
        {
            get { return mStateID; }
        }

        protected int mStateLevel;
        public int StateLevel
        {
            get { return mStateLevel; }
        }

        protected string mStateName;
        public string StateName
        {
            get { return mStateName; }
        }

        protected string mStateDetail;
        public string StateDetail
        {
            get { return mStateDetail; }
        }

        protected int mStateIconID;
        public int StateIconID
        {
            get { return mStateIconID; }
        }

        protected int mKeepActionID;
        public int KeepActionID
        {
            get { return mKeepActionID; }
        }

        protected int mKeepEffectID;
        public int KeepEffectID
        {
            get { return mKeepEffectID; }
        }

        protected int mStateNature;
        public int StateNature
        {
            get { return mStateNature; }
        }

        protected int mMaxStateLayer;
        public int MaxStateLayer
        {
            get { return mMaxStateLayer; }
        }

        protected long mStateLastTime;
        public long StateLastTime
        {
            get { return mStateLastTime; }
        }

        protected string mStateChangeProbID;
        public string StateChangeprobID
        {
            get { return mStateChangeProbID; }
        }


        protected string mStateProbChangedNum;
        public string StateProbChangedNum
        {
            get { return mStateProbChangedNum; }
        }


        protected string mStateProbChangedArg;
        public string StateProbChangedArg
        {
            get { return mStateProbChangedArg; }
        }

        protected int mStatehaloStateID;//光环
        public int StatehaloStateID
        {
            get { return mStatehaloStateID; }
        }

        protected int mStateUnSelectSign;
        public int StateUnSelectSign
        {
            get { return mStateUnSelectSign; }
        }

        protected int mImuneStateProb;
        public int ImuneStateProb
        {
            get { return mImuneStateProb; }
        }

        protected int mStateImuneEffectID;
        public int StateImuneEffectID
        {
            get { return mStateImuneEffectID; }
        }

       
        protected int mStateDeathStateSign;
        public int StateDeathStateSign
        {
            get { return mStateDeathStateSign; }
        }




        protected int mStateDisappearCondition;
        public int StateDisappearCondition
        {
            get { return mStateDisappearCondition; }
        }

        protected int mStateRecycleNum;
        public int StateRecycleNum
        {
            get { return mStateRecycleNum; }
        }

        protected int mStateSilenceSign;
        public int StateSilencesign
        {
            get { return mStateSilenceSign; }
        }

        protected int mStateImmovebleSign;
        public int StateImmovebleSign
        {
            get { return mStateImmovebleSign; }
        }

        protected int mStateUncontrolSign;
        public int StateUncontrolSign
        {
            get { return mStateUncontrolSign; }
        }

        protected double mStateHurtAvoidArg;
        public double StateHurtAvoidArg
        {
            get { return mStateHurtAvoidArg; }
        }

        protected double mStateHurtGainArg;
        public double StateHurtGainArg
        {
            get { return mStateHurtGainArg; }
        }

        protected int mStateHurtAvoidNum;
        public int StateHurtAvoidNum
        {
            get { return mStateHurtAvoidNum; }
        }

        protected int mStateHurtGainNum;
        public int StateHutyGainNum
        {
            get { return mStateHurtGainNum; }
        }

        protected int mStatePhysicalImuneSign;
        public int StatePhysicalImuneSign
        {
            get { return mStatePhysicalImuneSign; }
        }

        protected int mStateMagicImuneSign;
        public int StateMagicImuneSign
        {
            get { return mStateMagicImuneSign; }
        }

        protected int mStateHurtReboundNum;
        public int StateHurtReboundNum
        {
            get { return mStateHurtReboundNum; }
        }

        protected int mStateTransformID;
        public int StateTransformID
        {
            get { return mStateHurtReboundNum; }
        }

        protected int  mStateCommonAtacEffectID;
        public int StateCommonAtacEffectID
        {
            get{ return mStateCommonAtacEffectID; }
        }

        protected int mStateCommonAtacEffectGap;
        public int StateCommonAtacEffectGap
        {
            get{return mStateCommonAtacEffectGap;}
        }

        protected int mStateHurtTriggerEffectID;
        public int StateHurtTriggerEffectID
        {
            get { return mStateHurtTriggerEffectID; }
        }

        protected double mStateHurtTriggerEffectProba;
        public double StateHurtTriggerEffectProba
        {
            get { return mStateHurtTriggerEffectProba; }
        }

        protected int mStateHurtTriggerEffectTimeGap;
        public int StateHurtTriggerEffectTimeGap
        {
            get { return mStateHurtTriggerEffectTimeGap; }
        }

        protected int mStateAbsorbPhysicalHurtNum;
        public int StateAbsorbPhysicalHurtNum
        {
            get { return mStateAbsorbPhysicalHurtNum; }
        }

        protected int mStateAbsorbMagicHurtNum;
        public int StateAbsorbMagicHurtNum
        {
            get { return mStateAbsorbMagicHurtNum; }
        }

        protected int mStateAbsorbDeathHurtNum;
        public int StateAbsorbDeathHurtNum
        {
            get{return mStateAbsorbDeathHurtNum;}
        }

        public bool ImportData(Dictionary<string, string> data)//每一个实例存储csv文件中的一行
        {
            try
            {
                //return true;
                string tmp = null;
                if (!data.TryGetValue("序号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'序号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("状态编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态编号'的字段");
                    return false;
                }
                mStateID = Convert.ToInt32(tmp);


                if (!data.TryGetValue("状态等级", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态等级'的字段");
                    return false;
                }
                mStateLevel = Convert.ToInt32(tmp);

                if (!data.TryGetValue("状态名称", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态名称'的字段");
                    return false;
                }
                mStateName = tmp;

                if (!data.TryGetValue("状态描述", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态描述'的字段");
                    return false;
                }
                mStateDetail = tmp;

                if (!data.TryGetValue("图标编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'图标编号'的字段");
                    return false;
                }
                mStateIconID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("持续动作编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'持续动作编号'的字段");
                    return false;
                }
                mKeepActionID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("持续特效编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'持续特效编号'的字段");
                    return false;
                }
                mKeepEffectID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("状态性质", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态性质'的字段");
                    return false;
                }
                mStateNature = Convert.ToInt32(tmp);

                if (!data.TryGetValue("状态层数上限", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态层数上限'的字段");
                    return false;
                }
                mMaxStateLayer = Convert.ToInt32(tmp);

                if (!data.TryGetValue("状态持续时间", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'状态层数上限'的字段");
                    return false;
                }
                mStateLastTime = Convert.ToInt64(tmp);

                if (!data.TryGetValue("改变属性编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'改变属性编号'的字段");
                    return false;
                }
                mStateChangeProbID = tmp;

                if (!data.TryGetValue("属性改变数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'属性改变数值'的字段");
                    return false;
                }
                mStateProbChangedNum = tmp;

                if (!data.TryGetValue("属性改变系数", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'属性改变系数'的字段");
                    return false;
                }
                mStateProbChangedArg = tmp;

                if (!data.TryGetValue("光环状态编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'光环状态编号'的字段");
                    return false;
                }
                mStatehaloStateID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("不可选中标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'不可选中标识'的字段");
                    return false;
                }
                mStateUnSelectSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("免疫状态性质", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'免疫状态性质'的字段");
                    return false;
                }
                mImuneStateProb = Convert.ToInt32(tmp);

                if (!data.TryGetValue("免疫效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'免疫效果编号'的字段");
                    return false;
                }
                mStateImuneEffectID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("死亡保留标记", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'死亡保留标记'的字段");
                    return false;
                }
                mStateDeathStateSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("消失条件", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'消失条件'的字段");
                    return false;
                }
                mStateDisappearCondition = Convert.ToInt32(tmp);

                if (!data.TryGetValue("周期次数", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'周期次数'的字段");
                    return false;
                }
                mStateRecycleNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("沉默标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'沉默标识'的字段");
                    return false;
                }
                mStateSilenceSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("不可移动标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'不可移动标识'的字段");
                    return false;
                }
                mStateImmovebleSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("不可控制标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'不可控制标识'的字段");
                    return false;
                }
                mStateUncontrolSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("伤害减免系数", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'伤害减免系数'的字段");
                    return false;
                }
                mStateHurtAvoidArg = Convert.ToDouble(tmp);

                if (!data.TryGetValue("伤害增加系数", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'伤害增加系数'的字段");
                    return false;
                }
                mStateHurtGainArg = Convert.ToDouble(tmp);

                if (!data.TryGetValue("伤害减免数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'伤害减免数值'的字段");
                    return false;
                }
                mStateHurtAvoidNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("伤害增加数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'伤害增加数值'的字段");
                    return false;
                }
                mStateHurtGainNum = Convert.ToInt32(tmp);


                if (!data.TryGetValue("物理免疫标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'物理免疫标识'的字段");
                    return false;
                }
                mStatePhysicalImuneSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("魔法免疫标识", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'魔法免疫标识'的字段");
                    return false;
                }
                mStateMagicImuneSign = Convert.ToInt32(tmp);

                if (!data.TryGetValue("伤害反弹数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'伤害反弹数值'的字段");
                    return false;
                }
                mStateHurtReboundNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("变身编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'变身编号'的字段");
                    return false;
                }
                mStateTransformID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("普攻附带效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'普攻附带效果编号'的字段");
                    return false;
                }
                mStateCommonAtacEffectID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("普攻附带效果间隔", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'普攻附带效果间隔'的字段");
                    return false;
                }
                mStateCommonAtacEffectGap = Convert.ToInt32(tmp);

                if (!data.TryGetValue("受到伤害触发效果编号", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'受到伤害触发效果编号'的字段");
                    return false;
                }
                mStateHurtTriggerEffectID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("受到伤害触发效果概率", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'受到伤害触发效果概率'的字段");
                    return false;
                }
                mStateHurtTriggerEffectProba = Convert.ToDouble(tmp);

                if (!data.TryGetValue("受到伤害触发效果间隔", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'受到伤害触发效果间隔'的字段");
                    return false;
                }
                mStateHurtTriggerEffectTimeGap = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吸收物理伤害数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'吸收物理伤害数值'的字段");
                    return false;
                }
                mStateAbsorbPhysicalHurtNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吸收魔法伤害数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'吸收魔法伤害数值'的字段");
                    return false;
                }
                mStateAbsorbMagicHurtNum = Convert.ToInt32(tmp);

                if (!data.TryGetValue("至死伤害吸收数值", out tmp))
                {
                    DebugMod.Log("ConfigSkillState 没有名为'至死伤害吸收数值'的字段");
                    return false;
                }
                mStateAbsorbDeathHurtNum = Convert.ToInt32(tmp);
            }
            catch (Exception ex)
            {
                DebugMod.Log("ConfigSkillState 读取数据异常" + ",ID" + mStateID);
                DebugMod.Log(ex);

                return false;
            }

            return true;
        }
    }
}
