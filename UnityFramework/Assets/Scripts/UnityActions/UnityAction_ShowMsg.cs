using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_ShowMsg : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var msgTemp = ActParam["Msg"];
                if (null == msgTemp)
                    return false;

                var showPosTmp = ActParam["Pos"];
                if (null == showPosTmp)
                    return false;

                string msgStr = msgTemp.ToString();
                //MsgPos showPos = (MsgPos)showPosTmp;

                //int result = -1;
                //if (int.TryParse(resultStr, out result))
                //{
                //    EventManager.Trigger<EventCreateChar>(new EventCreateChar(result, msgStr));
                //}      
                UIManager.MsgBox("", msgStr, MsgStyle.Yes, null);
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