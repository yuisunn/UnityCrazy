using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    public class LogicAction_ShowPlayerAttr : LogicAction
    {
        public override bool ProcessAction()
        {
            try
            {
                if (null == ActParam)
                {
                    return false;
                }

                MyPlayer myPlayer = Local.Instance.GetMyPlayer;
                
                if (null == myPlayer)
                {
                    return false;
                }

                MyPlayerAttr playerAttr = myPlayer.MyAttr;

                ActionParam param = new ActionParam();
                param["Force"] = playerAttr.Strength;
                param["Speed"] = playerAttr.Dex;
                param["Wit"] = playerAttr.Inte;
                param["ForceUp"] = playerAttr.StrengthAdd;
                param["SpeedUp"] = playerAttr.DexAdd;
                param["WitUp"] = playerAttr.InteAdd;

                param["MaxHp"] = playerAttr.HpMax;
                param["PhyATK"] = playerAttr.PhyAttack;
                param["MagicATK"] = playerAttr.MagAttack;
                param["PhysicProtect"] = playerAttr.PhyDef;
                param["MagicProtect"] = playerAttr.MagDef;
                param["PhyCrit"] = playerAttr.HeavyAttack;
                param["CritHit"] = playerAttr.HeavyAttackDamage;

                Local.Instance.CallUnityAction(UnityActionDefine.ShowRoleProperty, param);

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
