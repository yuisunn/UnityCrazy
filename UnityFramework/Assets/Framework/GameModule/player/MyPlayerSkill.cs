using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class SkillData
    {
        public int Skillid;
        public int SkillNowlevel;
        //public int NowPhyHurt;
        //public int LevelUpEffectID;
        public int CharNowStage;
        //public int SkillPos;
        public bool IsLearned;
        //public bool IsChosen;
        public long CoolingTime;
        public long CoolEndTime;
        public string IconName;
    }

    public class MyPlayerSkill : GameModuleBase
    {
        //Dictionary<int, Dictionary<int, List<SkillData>>> mSkillSet = null;//玩家不同角色类型对应着各自学会的技能

        MyPlayer myPlayer = null;
        //long charid = myPlayer.CharID;

        public MyPlayerSkill()
        {
            //myPlayer = new MyPlayer(1);
        }

        public void OnLogicLearnSkillOver(ActionParam param)
        {

        }

        public void OnLogicLevelUpOver(ActionParam param)
        { }

        public void OnLogicAskLearnSkill(ActionParam param)
        {
            if (null == (myPlayer = Local.Instance.GetMyPlayer))//若是正常的版本，最好在netbase中加入角色当前所处阶位，便于查找技能。
            {
                return;
            }

            int CharNowLevel = myPlayer.MyInfo.Level;//实际应为当前角色的最新信息
            int CharNowStage = 1;
            int CharNowMoney = 1000;
            int CharNowClass = myPlayer.MyInfo.CharClass;

            int skillID = int.Parse(param["SkillID"].ToString());
            int CharLevelReq = int.Parse(param["CharLevel"].ToString());
            int SkillStageReq = int.Parse(param["CharStage"].ToString());
            int ConsumeMoney = int.Parse(param["ConsumeMoney"].ToString());
            double CoolingTime = double.Parse(param["CoolingTime"].ToString());
            string IconName = param["IconName"].ToString();

            ActionParam newparam = new ActionParam();

            if (CharLevelReq > CharNowLevel || SkillStageReq > CharNowStage)
            {
                newparam["Error"] = 1; //等级不满足学习条件   
            }
            else if (ConsumeMoney > CharNowMoney)
            {
                newparam["Error"] = 2;//资源消耗不足
            }
            else
            {
                SkillData newskill = new SkillData();

                newskill.IsLearned = true;
                newskill.Skillid = skillID;
                newskill.SkillNowlevel = 1;
                newskill.CharNowStage = CharNowStage;
                newskill.CoolingTime = (long)CoolingTime;
                newskill.IconName = IconName;

                newparam["Error"] = 0;
                MyPlayerSkillDBManager.Instance.DBManager[CharNowClass].Add(skillID, newskill);//学习后加到数据库.
            }

            newparam["IDSkill"] = skillID;
            Local.Instance.CallUnityAction(UnityActionDefine.RecvStudySkill, newparam);
        }
            
        public void OnLogicAskSkillLevelUp(ActionParam param)
        {   
            if (null == (myPlayer = Local.Instance.GetMyPlayer))//若是正常的版本，最好在netbase中加入角色当前所处阶位，便于查找技能。
            {
                return;
            }

            int CharNowLevel = myPlayer.MyInfo.Level;//实际应为当前角色的最新信息
            int CharNowStage = 1;
            int CharNowMoney = 1000;
            int CharNowClass = myPlayer.MyInfo.CharClass;

            int skillID = int.Parse(param["SkillID"].ToString());
            int CharLevelReq = int.Parse(param["CharLevel"].ToString());
            int SkillStageReq = int.Parse(param["CharStage"].ToString());
            int ConsumeMoney = int.Parse(param["ConsumeMoney"].ToString());


            ActionParam newparam = new ActionParam();

            if (CharLevelReq > CharNowLevel || SkillStageReq > CharNowStage)
            {
                newparam["Error"] = 1; //等级不满足升级条件   
            }
            else if (ConsumeMoney > CharNowMoney)
            {
                newparam["Error"] = 2;//资源消耗不足
            }
            else
            {
                LocalDataToC localdata = new LocalDataToC();
                SkillData dbdata = null;
                MyPlayerSkillDBManager.Instance.DBManager[CharNowClass].TryGetValue(skillID, out dbdata);

                dbdata.SkillNowlevel++;
                localdata.CharStage = (short)dbdata.CharNowStage;
                localdata.SkillLevel = dbdata.SkillNowlevel;

                foreach (var SkillLevelUp in ConfigManager.Instance.mConfigSkillLevelUp.Values)
                {
                    if (SkillLevelUp.SkillID == skillID && SkillLevelUp.SkillLevel == dbdata.SkillNowlevel)
                    {
                        localdata.SkillID = (int)SkillLevelUp.SkillID;
                        localdata.SkillLevel = dbdata.SkillNowlevel;

                        GetSendDataFromLevelUp(SkillLevelUp, localdata);
                        break;
                    }
                }

                newparam["NewData"] = localdata;
                newparam["Error"] = 0;
            }

            Local.Instance.CallUnityAction(UnityActionDefine.RecvLevelUp, newparam);
        }

        public void OnLogicPushSkillBtn(ActionParam param)
        {
            if (null == (myPlayer = Local.Instance.GetMyPlayer))
            {
                return;
            }
            //int CharClass = myPlayer.MyInfo.CharClass;

            int CharClassID = myPlayer.MyInfo.CharClass;//实际要是角色目前所在阶位. 

            List<LocalDataToC> LSkillData = new List<LocalDataToC>();
            Dictionary<int, LocalDataToC> DSkillData = new Dictionary<int, LocalDataToC>();

            Dictionary<int, SkillData> tmp = null;
            if (!MyPlayerSkillDBManager.Instance.DBManager.TryGetValue(CharClassID, out tmp))
            {
                DebugMod.LogError("CharInfo in DB is empty!");
                return;
            }

            //bool IsContainZero = false;
            string StageNames = null;

            foreach (var CharData in ConfigManager.Instance.mConfigCharData.Values)
            {
                //StageNames += CharData.CharName + ":";//表格数据不全，暂用记录名字

                if (CharData.CharClassID == CharClassID)
                {
                    StageNames += CharData.CharName + ":";//表格数据不全，暂用记录名字

                }
            }
            foreach (var SkillLevelUp in ConfigManager.Instance.mConfigSkillLevelUp.Values)
            {
                if (CharClassID == SkillLevelUp.RoleLimitation)
                {
                    SkillData dbdata = null;
                    LocalDataToC localdata = null;
                    bool IsFind = false;

                    if (tmp.TryGetValue((int)SkillLevelUp.SkillID, out dbdata))
                    {
                        if (dbdata.SkillNowlevel == SkillLevelUp.SkillLevel)
                        {
                            IsFind = true;

                            localdata = new LocalDataToC();

                            localdata.SkillLevel = dbdata.SkillNowlevel;
                            localdata.SkillID = dbdata.Skillid;
                            localdata.IsLearned = dbdata.IsLearned;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (1 == SkillLevelUp.SkillLevel)
                    {
                        //IsContainZero = true;
                        IsFind = true;

                        localdata = new LocalDataToC();

                        localdata.SkillLevel = SkillLevelUp.SkillLevel;
                        localdata.SkillID = (int)SkillLevelUp.SkillID;
                        localdata.IsLearned = false;

                    }

                    if (IsFind)
                    {
                        //localdata.CharStageName = CharData.CharName;
                        localdata.CharStage = SkillLevelUp.SkillStage;

                        GetSendDataFromLevelUp(SkillLevelUp, localdata);
                        ConfigSkillAll configdata = ConfigManager.Instance.GetConfigSkillAll(SkillLevelUp.SkillID);
                        GetSendDataFromAll(configdata, localdata);

                        LSkillData.Add(localdata);
                    }
                }
            }//foreach
            #region  有用
            //foreach
            //foreach (var CharData in ConfigManager.Instance.mConfigCharData.Values)
            //{
            //    //StageNames += CharData.CharName + ":";//表格数据不全，暂用记录名字

            //    if (CharData.CharClassID == CharClassID)
            //    {
            //        StageNames += CharData.CharName + ":";//表格数据不全，暂用记录名字

            //        foreach (var SkillLevelUp in ConfigManager.Instance.mConfigSkillLevelUp.Values)
            //        {
            //            if (CharClassID == SkillLevelUp.RoleLimitation)
            //            {
            //                SkillData dbdata = null;
            //                LocalDataToC localdata = null;
            //                bool IsFind = false;

            //                if (tmp.TryGetValue((int)SkillLevelUp.SkillID, out dbdata))
            //                {
            //                    if (dbdata.SkillNowlevel == SkillLevelUp.SkillLevel)
            //                    {
            //                        IsFind = true;

            //                        localdata = new LocalDataToC();

            //                        localdata.SkillLevel = dbdata.SkillNowlevel;
            //                        localdata.SkillID = dbdata.Skillid;
            //                        localdata.IsLearned = dbdata.IsLearned;
            //                    }
            //                    else
            //                    {
            //                        continue;
            //                    }
            //                }
            //                else if (1 == SkillLevelUp.SkillLevel)
            //                {
            //                    if (0 == SkillLevelUp.SkillStage && !IsContainZero || SkillLevelUp.SkillStage == CharData.CharNowStage)
            //                    {
            //                        IsContainZero = true;
            //                        IsFind = true;

            //                        localdata = new LocalDataToC();

            //                        localdata.SkillLevel = SkillLevelUp.SkillLevel;
            //                        localdata.SkillID = (int)SkillLevelUp.SkillID;
            //                        localdata.IsLearned = false;
            //                    }
            //                }

            //                if (IsFind)
            //                {
            //                    localdata.CharStageName = CharData.CharName;
            //                    localdata.CharStage = CharData.CharNowStage;

            //                    GetSendDataFromLevelUp(SkillLevelUp, localdata);
            //                    ConfigSkillAll configdata = ConfigManager.Instance.GetConfigSkillAll(SkillLevelUp.SkillID);
            //                    GetSendDataFromAll(configdata, localdata);

            //                    LSkillData.Add(localdata);
            //                }
            //            }
            //        }//foreach
            //    }//if
            #endregion   //}//foreach
            foreach (var item in LSkillData)
            {
                if (0 == item.CharStage)
                {
                    item.CharStage = 1;
                    break;
                }
            }

            LSkillData[0].CharStageName = StageNames;

            ActionParam recv = new ActionParam();
            recv["CharInfo"] = LSkillData;

            Dictionary<int, int> ChosenSkill = null;
            if (!MyPlayerSkillDBManager.Instance.ChosenSkillsHolder.TryGetValue(CharClassID, out ChosenSkill))
            {
                return;
            }
            recv["ChosenInfo"] = ChosenSkill;//MyPlayerSkillDBManager.Instance.ChosenSkillsHolder[CharClassID];

            Local.Instance.CallUnityAction(UnityActionDefine.RecvLearnSkill, recv);
        }

        private void GetSendDataFromAll(ConfigSkillAll csa, LocalDataToC sdawi)
        {
            sdawi.SkillName = csa.SkillName;
            sdawi.SkillIconID = csa.SkillIconID;
            sdawi.CoolingTime = Convert.ToInt32(csa.SkillCoolingTime);
            sdawi.ConsumeEnergy = Convert.ToInt32(csa.SkillEnergyConsume);
            sdawi.SkillDescrib = csa.SkillFunctionDetails;
            sdawi.SkillIconPos = (short)csa.SkillPosID;
        }

        private void GetSendDataFromLevelUp(ConfigSkillLevelUp data, LocalDataToC sdawi)
        {

            sdawi.PhyHurt = data.LevelUpProbMsg1;
            sdawi.IncAttacSpead = data.LevelUpProbMsg2;
            sdawi.LevelUpCond = (short)data.RoleLevelRequirement;
            sdawi.ConsumeMoney = data.LevelUpConsumeMoneyNum;
            sdawi.LevelUpEffect = "升级效果";
            sdawi.LevelUpMsg1 = data.LevelUpMsg1;
            sdawi.LevelUpMsg2 = data.LevelUpMsg2;
            sdawi.LevelUpMsg3 = data.LevelUpMsg3;

        }

        public bool OnLogicPushEquipBtn()
        {
            return true;
        }
        private void GetEquipSkill(int EquipSkillID, int EquipSkillLevel)
        {

        }

        public bool OnLogicPushBackBtn(ActionParam param)
        {

            if (null == (myPlayer = Local.Instance.GetMyPlayer))
            {
                DebugMod.LogError("GetMyPlayer Failed!");
                return false;
            }

            if (null == param)
            {
                DebugMod.LogError("PushBackBtn Failed!");
                return false;
            }

            Dictionary<int, int> ChosenSkill = param["ShortDic"] as Dictionary<int, int>;

            if (null == ChosenSkill)
            {
                DebugMod.LogError("No Data In ChosenSkill!");
                return false;
            }

            MyPlayerSkillDBManager.Instance.OnPushBackBtn(myPlayer.MyInfo.CharClass, ChosenSkill);


            return true;
        }

        public bool OnLogicEnterGame()
        {
            if (null == (myPlayer = Local.Instance.GetMyPlayer))
            {
                DebugMod.LogError("GetMyPlayer Failed!");
                return false;
            }

            Dictionary <int, ChosenSkillData> chosendata = null;

            chosendata = MyPlayerSkillDBManager.Instance.OnEnterGame(myPlayer.MyInfo.CharClass);

            ActionParam param = new ActionParam();
            param["ChosenData"] = chosendata;

            Local.Instance.CallUnityAction(UnityActionDefine.RecvChosenSkill, param);
            return true;
        }
    }
}
