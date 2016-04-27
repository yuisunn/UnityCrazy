using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_SendChar : UnityAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                    return false;
                var charidTemp = ActParam["CharID"];
                if (null == charidTemp)
                    return false;

                long charid = -1;
                if (!long.TryParse(charidTemp.ToString(), out charid))
                {
                    return false;
                }
                
                var firstnameTemp = ActParam["FirstName"];
                if (null == firstnameTemp)
                    return false;
                string firstname = firstnameTemp.ToString();

                var lastnameTemp = ActParam["LastName"];
                if (null == lastnameTemp)
                    return false;
                string lastname = lastnameTemp.ToString();

                var charclassTemp = ActParam["CharClass"];
                if (null == charclassTemp)
                    return false;
                short charclass = -1;
                if (!short.TryParse(charclassTemp.ToString(), out charclass))
                {
                    return false;
                }

                var chargradeTemp = ActParam["CharGrade"];
//                 if (null == chargradeTemp)
//                     return false;
                short chargrade = 1;
                if (chargradeTemp!=null&&!short.TryParse(chargradeTemp.ToString(), out chargrade))
                {
                    //return false;
                }

                var vipTemp = ActParam["VipLevel"];
                if (null == vipTemp)
                    return false;
                short viplevel = -1;
                if (!short.TryParse(vipTemp.ToString(), out viplevel))
                {
                    return false;
                }

                var charTemp = ActParam["CharLevel"];
                if (null == charTemp)
                    return false;
                short charlevel = -1;
                if (!short.TryParse(charTemp.ToString(), out charlevel))
                {
                    return false;
                }

                var mapTemp = ActParam["Map"];
                if (null == mapTemp)
                    return false;
                short mapid = -1;
                if (!short.TryParse(mapTemp.ToString(), out mapid))
                {
                    return false;
                }

                EventManager.Trigger<EventSendChar>(new EventSendChar(charid, firstname, lastname, charclass, chargrade, viplevel, charlevel, mapid));
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