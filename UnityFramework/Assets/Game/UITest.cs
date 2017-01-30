using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLCGame.Unity;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITest : MonoBehaviour {
    public GameObject pre;
    public GameObject item;
	// Use this for initialization
	void Start () {
        //UIMgr.Instance.ShowWindow<TestPanel>(this.gameObject);

        //AssetBundle ab = AssetBundle.LoadFromFile(PathMod.DataPath + "/cube-bundle");  
        GameObject bb = Instantiate(pre); 
        bb.transform.parent = this.transform; 
        LuaBehaviour tt = bb.AddComponent<LuaBehaviour>(); 
        //bb.GetComponentInChildren<ToggleGroup>().SetAllTogglesOff();
        Dropdown obj1 = bb.GetComponentInChildren<Dropdown>();  
        Slider s = bb.GetComponentInChildren<Slider>();  
        //obj1.onValueChanged.AddListener(CLick);
      

        //Image btn = tt.GetComponentInChildren<Image>();
        //btn.sprite = loadSprite("1");    
        Button btn = this.GetComponentInChildren<Button>();
        btn.onClick.AddListener(CLick);
    }

    public void CLick()
    { 
        LuaScriptMgr.Instance.TestPush("CreatePrefab", this.gameObject, (UnityEngine.GameObject)item);
    }
    private Sprite loadSprite(string spriteName)
    {
        return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
    }

}
