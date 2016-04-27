using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class mSkillDynamicArgsBase
    {
        public mSkillDynamicArgsBase() { }
        
        //左下属性
        public int SkillID;
        public string SkillName;
        public int SkillLevel;
        public string PhyHurt;
        public string IncAttacSpead;
        public int CoolingTime;
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


