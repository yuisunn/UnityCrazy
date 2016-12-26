using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderSort : MonoBehaviour {

    public UIPanel panel; 
    public int depth;
    public int renderQuene;
    [HideInInspector]
    public int drawCallID;
    [HideInInspector]
    public bool isDrity;

    void Awake()
    {
        panel.AddSortUI(this);
    }

    void Destory()
    {
        panel.RemoveSortUI(this);
    }

    public virtual void FillDrawCall()
    {
    }
}
