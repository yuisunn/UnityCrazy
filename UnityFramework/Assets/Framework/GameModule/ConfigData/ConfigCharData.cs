using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class ConfigCharData
    {
        protected int mID;
        public int ID
        {
            get { return mID; }
        }

        protected short mCharClassID;
        public short CharClassID
        {
            get { return mCharClassID; }
        }

        protected short mCharNowStage;
        public short CharNowStage
        {
            get { return mCharNowStage; }
        }

        protected string mCharName;
        public string CharName
        {
            get { return mCharName; }
        }

        public bool ImportData(Dictionary<string, string> data)
        {
            string tmp = null;

            if (!data.TryGetValue("ID", out tmp))
            {
                DebugMod.Log("ConfigCharData 没有名为'类型ID'的字段");
                return false;
            }
            mID = Convert.ToInt32(tmp);

            if (!data.TryGetValue("角色编号", out tmp))
            {
                DebugMod.Log("ConfigCharData 没有名为'类型ID'的字段");
                return false;
            }
            mCharClassID = Convert.ToInt16(tmp);

            if (!data.TryGetValue("阶位", out tmp))
            {
                DebugMod.Log("ConfigCharData 没有名为'类型ID'的字段");
                return false;
            }
            mCharNowStage = Convert.ToInt16(tmp);

            if (!data.TryGetValue("职业名称", out tmp))
            {
                DebugMod.Log("ConfigCharData 没有名为'类型ID'的字段");
                return false;
            }
            mCharName = tmp;

            return true;
        }
    }
}
