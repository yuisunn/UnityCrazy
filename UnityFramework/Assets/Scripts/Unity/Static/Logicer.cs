using UnityEngine;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class Logicer
    {
        public static bool isLocal = false;

        public static void LoginServer(string username, string password,string ip)
        {
            if(isLocal)
                Test_FakeLogic.Instance.ForceToStageRole(null);
            else
            {
                ActionParam param = new ActionParam();
                param["username"] = username;
                param["password"] = password;
                param["ip"] = ip;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.LoginServer, param);
            }        
        }

        public static void EnterGame(long charid, int line)
        {         
            if(isLocal)
            {
                Test_FakeLogic.Instance.ForceToStageGame((int)charid, 12345);
            }
            else
            {
                ActionParam param = new ActionParam();
                param["CharID"] = charid;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.EnterGame, param);                
            }       
        }

        public static void ChangeScene(int sceneid)
        {
            if (isLocal)
            {              
                Test_FakeLogic.Instance.GoToScene(sceneid);
            }
            else
            {
                ActionParam param = new ActionParam();
                param["SceneID"] = sceneid;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeScene, param);
            }
        }


        public static void UseSkill( int skillindex )
        {
            if (isLocal)
            {
                
            }
            else
            {
                ActionParam param = new ActionParam();
                param["SkillID"] = skillindex;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.UseSkill, param);
            }


        }

        public static void ChangeStageResult( int result,string stage  )
        {        
            if(isLocal)
            {
            }
            else
            {
                ActionParam param = new ActionParam();
                param["Result"] = result;
                param["GameStage"] =  stage;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeStageResult, param); 
            }
        }

        public static void ChangeSceneResult(int result, int sceneid)
        {

            if (isLocal)
            {
                Test_FakeLogic.Instance.InitScene(sceneid);
            }
            else
            {
                ActionParam param = new ActionParam();
                param["SceneID"] = sceneid;
                param["Result"] = result;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeMapResult, param);
            }
        }
        
        public static void DeleteChar( long charclass )
        {
            if (isLocal)
            {
                Test_FakeLogic.Instance.DeleteChar((int)charclass);
            }
            else
            {
                ActionParam param = new ActionParam();
                param["CharID"] = charclass;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.DeleteChar, param);
            }
        }


        public static void CreateChar( string firstname,string lastname,short charclass )
        {
            if (isLocal)
            {
                Test_FakeLogic.Instance.CreateChar((int)charclass, firstname, lastname);
            }
            else
            {
                ActionParam param = new ActionParam();
                param["FirstName"] = firstname;
                param["LastName"] = lastname;
                param["CharClass"] = charclass;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.CreateChar, param);
            }
        }

        public static void ReturnToLogin()
        {
            if (isLocal)
            {
                Test_FakeLogic.Instance.ForceToStageLogin(null);
            }
            else
            {
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ReturnToLogin, null);            
            }
        }


        public static void ChangeFloorTo( int spriteid, float destX, float destY, float destZ)
        {
            if (isLocal)
            {
                EventManager.Trigger<EventLocatSprite>(new EventLocatSprite(spriteid, new Vector3(destX, destY, destZ),Vector3.right));
                EventManager.Trigger<EventShowSprite>(new EventShowSprite(100000, true));
            }
            else
            {
                ActionParam param = new ActionParam();
                param["DestX"] = (short)(destX * 10);
                param["DestY"] = (short)(destY);
                param["DestZ"] = (short)(destZ * 10);
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ChangeFloor, param);
            }
        }

        public static void LeaveBattle()
        {
            if (isLocal)
            {
                EventManager.Trigger<EventButtonEvent>(new EventButtonEvent(ButtonEventType.LeaveBattle));
            }
            else
            {
                ActionParam param = new ActionParam();
                param["ButtonEvent"] = ButtonEventType.LeaveBattle;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.ButtonEvent, param);
            }
        }

        public static void MovePlayerTo( float currX,float currZ,float destX,float dextZ )
        {
            if (isLocal)
            {
                
            }
            else
            {
                ActionParam param = new ActionParam();
                ParamMoveTo mt = new ParamMoveTo();
                mt.CurrX = (short)(currX * 10);
                mt.CurrZ = (short)(currZ * 10);
                mt.DestX = (short)(destX * 10);
                mt.DestZ = (short)(dextZ * 10);

                param["ParamMoveTo"] = mt;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.MoveTo, param);
            }
        }

        public static void HitSprite( int creatureid,int spriteid )
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                param["CreatureID"] = creatureid;
                param["SpriteID"] = spriteid;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.HitSprite, param);
            }
        }

        public static void StopPlayer(float currX, float currZ)
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                ParamStopMove mt = new ParamStopMove();
                mt.CurrX = (short)(currX * 10);
                mt.CurrZ = (short)(currZ * 10);

                param["StopPlayer"] = mt;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.StopMove, param);
            }
        }

        public static void SendSpritePos(float currX, float currZ)
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                ParamStopMove mt = new ParamStopMove();
                mt.CurrX = (short)(currX * 10);
                mt.CurrZ = (short)(currZ * 10);

                param["Position"] = mt;
                //LogicMain.Instance.CallLogicAction(LogicActionDefine.GetSpritePosition, param);
            }
        }

        public static void GM(string gm )
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                param["GM"] = gm;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.GMCommand, param);
            }
        }

        public static void RequestLineGroup( string group )
        {
            if (isLocal)
            {

            }
            else
            {
//             ActionParam param = new ActionParam();
//             param["LineGroup"] = group;
//             LogicMain.Instance.CallLogicAction(LogicActionDefine.GMCommand, param);
            }
        }

        public static void LeanSkillTypeID(int typeid, int skillid = -1, int charclass = -1, int skilllevel = -1)
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                param["TypeID"] = typeid;
                param["SkillID"] = skillid;
                param["CharClass"] = charclass;
                param["SkillLevel"] = skilllevel;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.LearnSkill, param);
            }
        }

        public static void StudyAndUpSkillTypeID(int typeid, int skillid = -1, int charlevel = -1, int charstage = -1, int consumemoney = 0, double coolingtime = 0.0, string iconname = null)
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                param["TypeID"] = typeid;
                param["SkillID"] = skillid;
                param["CharLevel"] = charlevel;
                param["CharStage"] = charstage;
                param["ConsumeMoney"] = consumemoney;
                param["CoolingTime"] = coolingtime;
                param["IconName"] = iconname;

                LogicMain.Instance.CallLogicAction(LogicActionDefine.LearnSkill, param);
            }
        }

        public static void ShortSkillDic(int typeid, Dictionary<int ,int> shortDic)
        {
            if (isLocal)
            {

            }
            else
            {
                ActionParam param = new ActionParam();
                param["TypeID"] = typeid;
                param["ShortDic"] = shortDic;
                LogicMain.Instance.CallLogicAction(LogicActionDefine.LearnSkill, param);
            }
        }



    }
}