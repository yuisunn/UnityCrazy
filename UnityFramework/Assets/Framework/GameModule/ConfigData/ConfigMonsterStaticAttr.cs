using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigMonsterStaticAttr
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected int mLevel;
        public int Level
        {
            get { return mLevel; }
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

        public bool ImportData(Dictionary<string, string> data)
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("等级", out tmp))
                {
                    DebugMod.Log("ConfigMonsterAttr 没有名为'等级'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("等级", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'等级'的字段");
                    return false;
                }
                mLevel = Convert.ToInt32(tmp);

                if (!data.TryGetValue("最大生命", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'最大生命'的字段");
                    return false;
                }
                mHpMax = Convert.ToInt32(tmp);

                if (!data.TryGetValue("生命回复", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'生命回复'的字段");
                    return false;
                }
                mHpRestore = Convert.ToInt32(tmp);

                if (!data.TryGetValue("最大能量", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'最大能量'的字段");
                    return false;
                }
                mMpMax = Convert.ToInt32(tmp);

                if (!data.TryGetValue("物理攻击", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'物理攻击'的字段");
                    return false;
                }
                mPhyAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("法术强度", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'法术强度'的字段");
                    return false;
                }
                mMagAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("物理护甲", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'物理护甲'的字段");
                    return false;
                }
                mPhyDef = Convert.ToInt32(tmp);

                if (!data.TryGetValue("法术抗性", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'法术抗性'的字段");
                    return false;
                }
                mMagDef = Convert.ToInt32(tmp);

                if (!data.TryGetValue("暴击", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'暴击'的字段");
                    return false;
                }
                mHeavyAttack = Convert.ToInt32(tmp);

                if (!data.TryGetValue("暴击伤害", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'暴击伤害'的字段");
                    return false;
                }
                mHeavyAttackDamage = Convert.ToInt32(tmp);

                if (!data.TryGetValue("闪避", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'闪避'的字段");
                    return false;
                }
                mDodge = Convert.ToInt32(tmp);

                if (!data.TryGetValue("护甲穿透", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'护甲穿透'的字段");
                    return false;
                }
                mAntiArmor = Convert.ToInt32(tmp);

                if (!data.TryGetValue("忽视抗性", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'忽视抗性'的字段");
                    return false;
                }
                mAntiResist = Convert.ToInt32(tmp);

                if (!data.TryGetValue("吸血", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'吸血'的字段");
                    return false;
                }
                mAbsorbHp = Convert.ToInt32(tmp);

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
