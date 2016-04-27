using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ActionFinish : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;

                var actionTypeTemp = ActParam["Action"];
                if (null == actionTypeTemp)
                    return false;

                LogicActionDefine actionType = (LogicActionDefine)actionTypeTemp;

                //var paramTmp = ActParam["Param"];

                EventManager.Trigger<EventActionFinish>(new EventActionFinish(actionType));

                switch (actionType)
                {
                    case LogicActionDefine.CreateChar:

                        break;
                    case LogicActionDefine.DeleteChar:                                               

                        break;
                    case LogicActionDefine.EnterGame:

                        break;
                    case LogicActionDefine.LoginServer:

                        break;
                    case LogicActionDefine.ReturnToLogin:

                        break;
                    default:
                        break;
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