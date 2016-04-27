using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigPlayerAttr
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected int mCharClass;
        public int CharClass
        {
            get { return mCharClass; }
        }

        protected int mPhaseClass;
        public int PhaseClass
        {
            get { return mPhaseClass; }
        }

        protected int mHpMax;
        public int HpMax
        {
            get { return mHpMax; }
        }

        protected int mHpRestore;
        public int HpRestore
        {
            get { return mHpRestore; }
        }

        protected int mMpMax;
        public int MpMax
        {
            get { return mMpMax; }
        }

        protected int mPhyAttack;
        public int PhyAttack
        {
            get { return mPhyAttack; }
        }

        protected int mMagAttack;
        public int MagAttack
        {
            get { return mMagAttack; }
        }

        protected int mPhyDef;
        public int PhyDef
        {
            get { return mPhyDef; }
        }

        protected int mMagDef;
        public int MagDef
        {
            get { return mMagDef; }
        }

        protected int mHeavyAttack;
        public int HeavyAttack
        {
            get { return mHeavyAttack; }
        }

        protected int mHeavyAttackDamage;
        public int HeavyAttackDamage
        {
            get { return mHeavyAttackDamage; }
        }

        protected int mDodge;
        public int Dodge
        {
            get { return mDodge; }
        }

        protected int mAntiArmor;
        public int AntiArmor
        {
            get { return mAntiArmor; }
        }

        protected int mAntiResist;
        public int AntiResist
        {
            get { return mAntiResist; }
        }

        protected int mAbsorbHp;
        public int AbsorbHp
        {
            get { return mAbsorbHp; }
        }

        protected int mStrength;
        public int Strength
        {
            get { return mStrength; }
        }

        protected int mDex;
        public int Dex
        {
            get { return mDex; }
        }

        protected int mInte;
        public int Inte
        {
            get { return mInte; }
        }

        protected int mStrengthAdd;
        public int StrengthAdd
        {
            get { return mStrengthAdd; }
        }

        protected int mDexAdd;
        public int DexAdd
        {
            get { return mDexAdd; }
        }

        protected int mInteAdd;
        public int InteAdd
        {
            get { return mInteAdd; }
        }

        protected int mAttackRange;
        public int AttackRange
        {
            get { return mAttackRange + 12; }
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
                if (!data.TryGetValue("ID", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'ID'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("角色编号", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'职业'的字段");
                    return false;
                }
                mCharClass = Convert.ToInt32(tmp);

                if (!data.TryGetValue("攻击距离", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'攻击距离'的字段");
                    return false;
                }
                mAttackRange = Convert.ToInt32(tmp);

                if (!data.TryGetValue("移动速度", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'移动速度'的字段");
                    return false;
                }
                mMoveSpeed = Convert.ToInt32(tmp);

                if (!data.TryGetValue("最大生命", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'最大生命'的字段");
                    return false;
                }
                mHpMax = Convert.ToInt32(tmp);

                if (!data.TryGetValue("生命回复", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'生命回复'的字段");
                    return false;
                }
                mHpRestore = Convert.ToInt32(tmp);

                if (!data.TryGetValue("最大能量", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'最大能量'的字段");
                    return false;
                }
                mMpMax = Convert.ToInt32(tmp);

                if (!data.TryGetValue("物理攻击", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'物理攻击'的字段");
                    return false;
                }
                mPhyAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("法术强度", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'法术强度'的字段");
                    return false;
                }
                mMagAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("物理护甲", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'物理护甲'的字段");
                    return false;
                }
                mPhyDef = Convert.ToInt32(tmp);

                if (!data.TryGetValue("法术抗性", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'法术抗性'的字段");
                    return false;
                }
                mMagDef = Convert.ToInt32(tmp);

                if (!data.TryGetValue("暴击", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'暴击'的字段");
                    return false;
                }
                mHeavyAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("暴击伤害", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'暴击伤害'的字段");
                    return false;
                }
                mHeavyAttackDamage = Convert.ToInt32(tmp);

                if (!data.TryGetValue("闪避", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'闪避'的字段");
                    return false;
                }
                mDodge = Convert.ToInt32(tmp);

                if (!data.TryGetValue("护甲穿透", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'护甲穿透'的字段");
                    return false;
                }
                mAntiArmor = Convert.ToInt32(tmp);

                if (!data.TryGetValue("忽视抗性", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'忽视抗性'的字段");
                    return false;
                }
                mAntiResist = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吸血", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'吸血'的字段");
                    return false;
                }
                mAbsorbHp = Convert.ToInt32(tmp);

                if (!data.TryGetValue("力量", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'力量'的字段");
                    return false;
                }
                mStrength = Convert.ToInt32(tmp);

                if (!data.TryGetValue("敏捷", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'敏捷'的字段");
                    return false;
                }
                mDex = Convert.ToInt32(tmp);

                if (!data.TryGetValue("智力", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'智力'的字段");
                    return false;
                }
                mInte = Convert.ToInt32(tmp);

                if (!data.TryGetValue("力量成长", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'力量成长'的字段");
                    return false;
                }
                mStrengthAdd = Convert.ToInt32(tmp);

                if (!data.TryGetValue("敏捷成长", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'敏捷成长'的字段");
                    return false;
                }
                mDexAdd = Convert.ToInt32(tmp);

                if (!data.TryGetValue("智力成长", out tmp))
                {
                    DebugMod.Log("ConfigPlayerAttr 没有名为'智力成长'的字段");
                    return false;
                }
                mInteAdd = Convert.ToInt32(tmp);

            }
            catch (Exception ex)
            {
                DebugMod.Log("ConfigPlayerAttr 读取数据异常" + ",ID=" + mID);
                DebugMod.Log(ex);
                return false;
            }
            return true;
        }
    }
}