using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;


namespace SPSGame
{
    public class ConfigManager : SPSGame.Tools.Singleton<ConfigManager> 
    {
        public Dictionary<int, ConfigMonster> mConfigMonster = new Dictionary<int, ConfigMonster>();
        public Dictionary<int, ConfigMonsterDynAttr> mConfigMonsterDynAttr = new Dictionary<int, ConfigMonsterDynAttr>();
        public Dictionary<int, ConfigMonsterStaticAttr> mConfigMonsterStaticAttr = new Dictionary<int, ConfigMonsterStaticAttr>();

        public Dictionary<int, ConfigSkillAll> mConfigSkillAll = new Dictionary<int, ConfigSkillAll>();
        public Dictionary<int, ConfigSkillEffect> mConfigSkillEffect = new Dictionary<int, ConfigSkillEffect>();
        public Dictionary<int, ConfigSkillLevelUp> mConfigSkillLevelUp = new Dictionary<int, ConfigSkillLevelUp>();
        public Dictionary<int, ConfigSkillState> mConfigSkillState = new Dictionary<int, ConfigSkillState>();
        public Dictionary<int, ConfigCharData> mConfigCharData = new Dictionary<int, ConfigCharData>();
        public Dictionary<int, ConfigPlayerAttr> mConfigPlayerAttr = new Dictionary<int, ConfigPlayerAttr>();


        public bool Init(string[] skilldata)
        {
            if (!ReadConfigSkillAll(skilldata[0]))
            {
                return false;
            }

            if (!ReadConfigSkillLevelUp(skilldata[1]))
            {
                return false;
            }

            if (!ReadConfigCharData(skilldata[2]))
            {
                return false;
            }

            if (!ReadConfigSkillEffect(skilldata[3]))
            {
                return false;
            }

            if (!ReadConfigSkillState(skilldata[4]))
            {
                return false;
            }

            if (!ReadConfigMonster(skilldata[5]))
            {
                return false;
            }

            if (!ReadConfigMonsterDynAttr(skilldata[6]))
            {
                return false;
            }

            if (!ReadConfigMonsterStaticAttr(skilldata[7]))
            {
                return false;
            }

            if (!ReadConfigPlayerAttr(skilldata[8]))
            {
                return false;
            }

            return true;
        }

        public bool ReadConfigMonster(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);//下面都差个文件名 ""
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigMonster line = new ConfigMonster();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigMonster[line.ID] = line;
            }

            return true;
        }

        public bool ReadConfigMonsterStaticAttr(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);//下面都差个文件名 ""
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigMonsterStaticAttr line = new ConfigMonsterStaticAttr();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigMonsterStaticAttr[line.Level] = line;
            }

            return true;
        }

        public bool ReadConfigMonsterDynAttr(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);//下面都差个文件名 ""
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigMonsterDynAttr line = new ConfigMonsterDynAttr();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigMonsterDynAttr[line.ID] = line;
            }

            return true;
        }

        public bool ReadConfigSkillAll(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);//下面都差个文件名 ""
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigSkillAll line = new ConfigSkillAll();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigSkillAll[line.SkillID] = line;
            }

            return true;
        }

        public bool ReadConfigSkillEffect(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigSkillEffect line = new ConfigSkillEffect();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigSkillEffect[line.EffectID] = line;
            }

            return true;
        }

        public bool ReadConfigSkillLevelUp(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            { 
                ConfigSkillLevelUp line = new ConfigSkillLevelUp();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigSkillLevelUp[line.ID] = line;
            }

            return true;
        }

        public bool ReadConfigSkillState(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigSkillState line = new ConfigSkillState();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigSkillState[line.StateID] = line;
            }

            return true;
        }

        public bool ReadConfigCharData(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigCharData line = new ConfigCharData();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigCharData[line.ID] = line;//待定
            }

            return true;
        }

        public bool ReadConfigPlayerAttr(string filedata)
        {
            CSVMod csvmod = new CSVMod(filedata, false);
            if (!csvmod.LoadCsvStr())
            {
                return false;
            }

            List<Dictionary<string, string>> allData = csvmod.GetAllData();
            foreach (var dic in allData)
            {
                ConfigPlayerAttr line = new ConfigPlayerAttr();
                if (!line.ImportData(dic))
                {
                    return false;
                }

                mConfigPlayerAttr[line.ID] = line;
            }

            return true;
        }

        //************
        public ConfigPlayerAttr GetConfigPlayerAttr(int id)
        {
            ConfigPlayerAttr data = null;

            if (!mConfigPlayerAttr.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigMonster GetConfigMonster(int id)
        {
            ConfigMonster data = null;

            if (!mConfigMonster.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigMonsterStaticAttr GetConfigMonsterStaticAttr(int id)
        {
            ConfigMonsterStaticAttr data = null;

            if (!mConfigMonsterStaticAttr.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigMonsterDynAttr GetConfigMonsterDynAttr(int id)
        {
            ConfigMonsterDynAttr data = null;

            if (!mConfigMonsterDynAttr.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }

   
        public ConfigSkillAll GetConfigSkillAll(int skillid)
        {
            ConfigSkillAll data = null;

            if (!mConfigSkillAll.TryGetValue(skillid, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigSkillEffect GetConfigSkillEffect(int effectid)
        {
            ConfigSkillEffect data = null;

            if (!mConfigSkillEffect.TryGetValue(effectid, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigSkillLevelUp GetConfigSkillLevelUp(int id)
        {
            ConfigSkillLevelUp data = null;

            if (!mConfigSkillLevelUp.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigSkillState GetConfigSkillState(int skillid)
        {
            ConfigSkillState data = null;

            if (!mConfigSkillState.TryGetValue(skillid, out data))
            {
                return null;
            }

            return data;
        }

        public ConfigCharData GetConfigCharData(int id)
        {
            ConfigCharData data = null;

            if (!mConfigCharData.TryGetValue(id, out data))
            {
                return null;
            }

            return data;
        }
    }
}
