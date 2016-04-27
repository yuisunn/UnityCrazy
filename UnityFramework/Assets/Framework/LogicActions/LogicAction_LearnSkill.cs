using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;
using SPSGame.GameModule;

namespace SPSGame
{
    public class LogicAction_LearnSkill : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var typeid = ActParam["TypeID"];
                if (null == typeid)
                {
                    return false;
                }

                //var skillid = ActParam["SkillID"];
                //if (null == skillid)
                //{
                //    return false;
                //}

                //var charclass = ActParam["CharClass"];
                //if (null == charclass)
                //{
                //    return false;
                //}

                //var skilllevel = ActParam["SkillLevel"];
                //if (null == skilllevel)
                //{
                //    return false;
                //}

                switch (int.Parse(typeid.ToString()))
                {
                    case 1: Local.Instance.GetMyPlayer.m_objSkill.OnLogicPushSkillBtn(ActParam); break;
                    case 2: Local.Instance.GetMyPlayer.m_objSkill.OnLogicAskLearnSkill(ActParam); break;
                    case 3: Local.Instance.GetMyPlayer.m_objSkill.OnLogicAskSkillLevelUp(ActParam); break;
                    case 4: Local.Instance.GetMyPlayer.m_objSkill.OnLogicPushBackBtn(ActParam); break;
                    case 5: Local.Instance.GetMyPlayer.m_objSkill.OnLogicEnterGame(); break;
                    case 6: break;
                    case 7: break;

                    default: break;
                }

                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
