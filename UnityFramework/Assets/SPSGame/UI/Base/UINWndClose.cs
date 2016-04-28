using UnityEngine;
using System.Collections;

public class UINWndClose : SPSGame.Unity.UIWndBase
{

    public GameObject close;
    // Use this for initialization
    void Awake () {
        ListenOnClick(close, CloseClick);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CloseClick(GameObject obj)
    {
        OnClickClose(obj);
    }
}
