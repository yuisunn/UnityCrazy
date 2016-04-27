using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using System.IO;

namespace SPSGame
{
    public class MyPlayerSkillDBManager : Tools.Singleton<MyPlayerSkillDBManager>
    {

        public Dictionary<int, Dictionary<int, SkillData>> DBManager = null;//学习的技能 <CharClass, <SkillID, Skilldata>>
        public Dictionary<int, Dictionary<int, int>> ChosenSkillsHolder = null;//技能快捷栏中的技能<CharClass, <IconPos, SkillID>>
        public Dictionary<int, ChosenSkillData> ChosenSkillTimeInfoSet = null;//主界面显示的技能<SkillID, ChosenSkillData>
        
        public MyPlayerSkillDBManager()
        {
            DBManager = new Dictionary<int, Dictionary<int, SkillData>>();
            ChosenSkillsHolder = new Dictionary<int, Dictionary<int, int>>();
            ChosenSkillTimeInfoSet = new Dictionary<int, ChosenSkillData>();
        }

        public bool Init(string data)
        {
            ReadDataFromFile(data);
            return true;
        }

        public bool OnCreateCharacter(int CharClassID)
        {
            if (DBManager.ContainsKey(CharClassID))
            {
                DBManager[CharClassID].Add(0, null);

                return true;
            }

            return false;
        }

        public bool OnDeleteCharacter(int CharClassID)
        {
            if (DBManager != null && DBManager.ContainsKey(CharClassID))
            {
                DBManager[CharClassID].Clear();
                ChosenSkillsHolder[CharClassID].Clear();
                ChosenSkillTimeInfoSet.Clear();
                
                return true;
            }

            return false;
        }
            
        public bool OnPushBackBtn(short CharClass, Dictionary<int, int> ChosenSkills)
        {   
            if (null == ChosenSkills)
            {
                return false;
            }

            ChosenSkillsHolder[CharClass].Clear();

            foreach (var item in ChosenSkills)
            {
                ChosenSkillsHolder[CharClass].Add(item.Key, item.Value);
            }
            
            return true;
        }
            
        public int GetSkillIDByIconPos(int iconPos)
        {   
            int SkillID = 0;

            if (!ChosenSkillsHolder[Local.Instance.GetMyPlayer.MyInfo.CharClass].TryGetValue(iconPos, out SkillID))
            {
                DebugMod.LogError("IconPos" + iconPos);
                return -1;
            }

            return SkillID;
        }


        public bool OnLevelUpSkill(int CharID, int SkillID)
        {
            return true;
        }

        public bool ReadDataFromFile(string filedata)
        {
            if (null == filedata)
            {
                DebugMod.LogError("DBdata is empty!");
                return false;
            }

            string[] DBdata = filedata.Split(new string[] { "\r\n" }, StringSplitOptions.None);//*

            int Key = 0;
            string Value = null;
            for (int index = 0; index < DBdata.Length; index += 1)
            {
                if ((Value = DBdata[index]).Contains("#"))
                {
                    ++index;
                    Key = int.Parse(DBdata[index]);
                    DBManager.Add(Key, new Dictionary<int, SkillData>());
                    ChosenSkillsHolder.Add(Key, new Dictionary<int, int>());
                }
                else
                {
                    string[] TypeConts = Value.Split(':');

                    for (int index1 = 0; index1 < TypeConts.Length - 1; ++index1)
                    {
                        string[] sdConts = TypeConts[index1].Split(',');

                        SkillData sd = new SkillData();

                        sd.Skillid = Convert.ToInt32(sdConts[0]);
                        sd.SkillNowlevel = Convert.ToInt32(sdConts[1]);
                        sd.CharNowStage = Convert.ToInt32(sdConts[2]);
                        sd.IsLearned = Convert.ToBoolean(sdConts[3]);

                        DBManager[Key].Add(sd.Skillid, sd);
                    }
                }
            }

            return true;
        }


         public bool OnRecordTime(int SkillID, long tickNow, double publicCDTime)
        {
            int CharClass = Local.Instance.GetMyPlayer.MyInfo.CharClass;

            SkillData skilldata = null;
            if (null == DBManager || !DBManager[CharClass].TryGetValue(SkillID, out skilldata))
            {
                //if (null == ChosenSkillsHolder || !ChosenSkillsHolder[CharClass].TryGetValue(IconPos, out SkillID))
                //{
                //    DebugMod.LogError("No Skill In This Icon!");

                //    return false;
                //}
                return false;
            }
            skilldata.CoolEndTime = tickNow + skilldata.CoolingTime * 1000;//记录每个技能的CoolEndTime， CoolEndTime = CoolingTime + 接到通知时的时间 

            Dictionary<int, ChosenSkillData> dicData = new Dictionary<int, ChosenSkillData>();

            foreach(var pair in ChosenSkillTimeInfoSet)
            {
                ChosenSkillData chosenskilldata = pair.Value;
                if (pair.Key == SkillID)
                {
                    chosenskilldata.LeftTime = skilldata.CoolingTime;
                    dicData.Add(pair.Key, chosenskilldata);
                }
                else
                {
                    if (DBManager[CharClass].TryGetValue(pair.Key, out skilldata))
                    {
                        if (tickNow + publicCDTime*1000 > skilldata.CoolEndTime)
                        {
                            chosenskilldata.LeftTime = publicCDTime;
                            dicData.Add(pair.Key, chosenskilldata);
                        }
                    }
                }
            }

            ActionParam param = new ActionParam();
            param["ChosenData"] = dicData;

            Local.Instance.CallUnityAction(UnityActionDefine.RecvChosenOneSkill, param);
            return true;
        }
  

        public Dictionary<int, ChosenSkillData> OnEnterGame(int CharClass)
        {   
            return GetChosenSkillData(CharClass);
        }
                    
        private Dictionary<int, ChosenSkillData> GetChosenSkillData(int CharClass)
        {
            if (ChosenSkillTimeInfoSet.Count != 0)
            {
                ChosenSkillTimeInfoSet.Clear();
            }

            if (ChosenSkillsHolder != null && ChosenSkillsHolder[CharClass].Count > 0)
            {
                foreach (var item in ChosenSkillsHolder[CharClass])
                {
                    ChosenSkillData chosendata = new ChosenSkillData();

                    SkillData skilldata = null;
                    if (!DBManager[CharClass].TryGetValue(item.Value, out skilldata))
                    {
                        DebugMod.LogError("No Such Data In DB!");
                        return null;
                    }

                    chosendata.IconName = skilldata.IconName;
                    chosendata.IconPos = item.Key;
                    chosendata.LeftTime = skilldata.CoolEndTime - DateTime.Now.Ticks / 10000; //LeftTime = CoolEndTime - 当前时间戳.
                    chosendata.CoolingTime = skilldata.CoolingTime;

                    ChosenSkillTimeInfoSet.Add(item.Value, chosendata);
                }
            }

            return ChosenSkillTimeInfoSet;
        }

        public bool WriteDataBackToFile()
        {   
            return true;
        }   
    }       
}
