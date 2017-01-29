using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLCGame.Unity;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
    public GameObject pre;
	// Use this for initialization
	void Start () {
        //UIMgr.Instance.ShowWindow<TestPanel>(this.gameObject);

        //AssetBundle ab = AssetBundle.LoadFromFile(PathMod.DataPath + "/cube-bundle");  
        GameObject bb = Instantiate(pre); 
        bb.transform.parent = this.transform; 
        LuaBehaviour tt = bb.AddComponent<LuaBehaviour>();
        tt.OnInit();
        //Image btn = tt.GetComponentInChildren<Image>();
        //btn.sprite = loadSprite("1");  



    }
    private Sprite loadSprite(string spriteName)
    {
        return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
    }

}
