using UnityEngine;
using System.Collections;

public abstract class ActionBase
{
    public int ActionId { get; set; }
    public ActionParam ActParam { get; set; }
    public abstract bool ProcessAction();

}