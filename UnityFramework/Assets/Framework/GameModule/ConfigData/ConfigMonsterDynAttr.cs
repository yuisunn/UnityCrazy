using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigMonsterDynAttr
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected float mHpMax;
        public float HpMax
        {
            get { return mHpMax; }
        }

        protected float mHpRestore;
        public float HpRestore
        {
            get { return mHpRestore; }
        }

        protected float mMpMax;
        public float MpMax
        {
            get { return mMpMax; }
        }

        protected float mPhyAttack;
        public float PhyAttack
        {
            get { return mPhyAttack; }
        }

        protected float mMagAttack;
        public float MagAttack
        {
            get { return mMagAttack; }
        }

        protected float mPhyDef;
        public float PhyDef
        {
            get { return mPhyDef; }
        }

        protected float mMagDef;
        public float MagDef
        {
            get { return mMagDef; }
        }

        protected float mHeavyAttack;
        public float HeavyAttack
        {
            get { return mHeavyAttack; }
        }

        protected float mHeavyAttackDamage;
        public float HeavyAttackDamage
        {
            get { return mHeavyAttackDamage; }
        }

        protected float mDodge;
        public float Dodge
        {
            get { return mDodge; }
        }

        protected float mAntiArmor;
        public float AntiArmor
        {
            get { return mAntiArmor; }
        }

        protected float mAntiResist;
        public float AntiResist
        {
            get { return mAntiResist; }
        }

        protected float mAbsorbHp;
        public float AbsorbHp
        {
            get { return mAbsorbHp; }
        }

        public bool ImportData(Dictionary<string, string> data)
        {
            try
            {
                string tmp = null;
                if (!data.TryGetValue("动态方案编号", out tmp))
                {
                    DebugMod.Log("ConfigMonsterAttr 没有名为'动态方案编号'的字段");
                    return false;
                }
                mID = Convert.ToInt32(tmp);

                if (!data.TryGetValue("最大生命系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'最大生命系数'的字段");
                    return false;
                }
                mHpMax = Convert.ToSingle(tmp);

                if (!data.TryGetValue("生命回复系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'生命回复系数'的字段");
                    return false;
                }
                mHpRestore = Convert.ToSingle(tmp);

                if (!data.TryGetValue("最大能量系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'最大能量系数'的字段");
                    return false;
                }
                mMpMax = Convert.ToSingle(tmp);

                if (!data.TryGetValue("物理攻击系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'物理攻击系数'的字段");
                    return false;
                }
                mPhyAttack = Convert.ToSingle(tmp);

                if (!data.TryGetValue("法术强度系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'法术强度系数'的字段");
                    return false;
                }
                mMagAttack = Convert.ToSingle(tmp);

                if (!data.TryGetValue("物理护甲系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'物理护甲系数'的字段");
                    return false;
                }
                mPhyDef = Convert.ToSingle(tmp);

                if (!data.TryGetValue("法术抗性系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'法术抗性系数'的字段");
                    return false;
                }
                mMagDef = Convert.ToSingle(tmp);

                if (!data.TryGetValue("暴击系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'暴击系数'的字段");
                    return false;
                }
                mHeavyAttack = Convert.ToSingle(tmp);

                if (!data.TryGetValue("暴击伤害系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'暴击伤害系数'的字段");
                    return false;
                }
                mHeavyAttackDamage = Convert.ToSingle(tmp);

                if (!data.TryGetValue("闪避系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'闪避系数'的字段");
                    return false;
                }
                mDodge = Convert.ToSingle(tmp);

                if (!data.TryGetValue("护甲穿透系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'护甲穿透系数'的字段");
                    return false;
                }
                mAntiArmor = Convert.ToSingle(tmp);

                if (!data.TryGetValue("忽视抗性系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'忽视抗性系数'的字段");
                    return false;
                }
                mAntiResist = Convert.ToSingle(tmp);

                if (!data.TryGetValue("吸血系数", out tmp))
                {
                    DebugMod.Log("ConfigScene 没有名为'吸血系数'的字段");
                    return false;
                }
                mAbsorbHp = Convert.ToSingle(tmp);

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
