using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Unity;
using SPSGame.Tools;

namespace SPSGame
{
    public class UnityAction_DeleteChar : UnityAction
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
                
                var charclassTemp = ActParam["CharClass"];
                if (null == charclassTemp)
                    return false;
                short charclass = -1;
                if (!short.TryParse(charclassTemp.ToString(), out charclass))
                {
                    return false;
                }


                EventManager.Trigger<EventDeleteChar>(new EventDeleteChar(charid, charclass));

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