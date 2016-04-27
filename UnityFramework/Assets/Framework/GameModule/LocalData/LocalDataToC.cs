using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class LocalDataToC
    {
        public LocalDataToC() { }

        public string CharStageName;
        public short CharStage = 1;//技能等级为1 以后要改（该从服务器获得）
        public string SkillIconID;
        public short SkillIconPos;//daiding
        public bool IsLearned;
        public int charLevel = 1;//以后要改（该从服务器获得）
        public bool IsChosen;


        //左下属性
        public int SkillID;
        public string SkillName;
        public int SkillLevel;

        public string PhyHurt;
        public string IncAttacSpead;
        public double CoolingTime;
        public int ConsumeEnergy;
        public int ConsumeMoney;
        public string SkillDescrib;

        //右下属性 
        public short LevelUpCond;
        public string LevelUpEffect;//写死 升级效果
        public string LevelUpMsg1;
        public string LevelUpMsg2;
        public string LevelUpMsg3;
    }
}
