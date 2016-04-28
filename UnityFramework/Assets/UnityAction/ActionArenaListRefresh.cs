using UnityEngine;
using System.Collections;
using System;

public class ActionArenaList : LogicAction
{
    public override bool ProcessAction()
    {
        if (ActParam == null)
            return false;
        ArenaPop pop = UIManager.Instance.GetWindow<ArenaPop>();
        C2sSprotoType.ara_rfh.response resp = ActParam["resp"] as C2sSprotoType.ara_rfh.response;

        ArenaMgr.Instance.refreshCount = 0;
        pop.battleNum.text = ArenaMgr.Instance.refreshCount.ToString();

        return true;
    }
}
