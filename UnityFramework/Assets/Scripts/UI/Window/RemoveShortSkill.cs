using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Unity;
using System.Linq;
using SPSGame;

public class RemoveShortSkill : UIDragDropItem
{
    public GameObject wndSkill = null;

    private UIWndSkill mWndSkill = null;
    public UISprite mus = null;


    protected override void Start()
    {
        base.Start();
        mWndSkill = wndSkill.GetComponent<UIWndSkill>();
        mus = this.transform.GetComponent<UISprite>();
    }

    int key = -1;
    int value = -1;

    protected override void OnDragDropStart()
    {
        //在克隆的icon上调用的
        base.OnDragDropStart();

        switch (this.name)
        {
            case "ShortCutSkill":
                GetValue(1);
                transform.parent = this.transform;
                this.GetComponent<UISprite>().depth = 20;
                break;
            case "ShortCutSkill2":
                GetValue(2);
                transform.parent = this.transform;
                this.GetComponent<UISprite>().depth = 20;
                break;
            case "ShortCutSkill3":
                GetValue(3);
                transform.parent = this.transform;
                this.GetComponent<UISprite>().depth = 20;
                break;
            case "ShortCutSkill4":
                GetValue(4);
                transform.parent = this.transform;
                this.GetComponent<UISprite>().depth = 20;
                break;
        }
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        print(surface.name);
        if (surface != null)
        { 
            //当一个技能拖到了快捷方式上的时候
            switch (surface.name)
            {
                case "ShortCutSkill":
                    if (mWndSkill.shortSkillDic.ContainsKey(1))
                    {
                        string tempName = surface.GetComponent<UISprite>().spriteName;
                        surface.GetComponent<UISprite>().spriteName = mus.spriteName;
                        mus.spriteName = tempName;

                        this.transform.localPosition = Vector3.zero;
                        this.GetComponent<UISprite>().depth = 10;
                        mWndSkill.shortSkillDic[key] = mWndSkill.shortSkillDic[1];
                        mWndSkill.shortSkillDic[1] = value;

                    }
                    else
                    {
                        SpriteName(surface);
                        if (!mWndSkill.shortSkillDic.Remove(key))
                        {
                            Debug.LogError("删除失败");
                        }
                        mWndSkill.shortSkillDic[1] = value;

                    }
                    
                    break;

                case "ShortCutSkill2":
                    if (mWndSkill.shortSkillDic.ContainsKey(2))
                    {
                        string tempName = surface.GetComponent<UISprite>().spriteName;
                        surface.GetComponent<UISprite>().spriteName = mus.spriteName;
                        mus.spriteName = tempName;
                        this.transform.localPosition = Vector3.zero;
                        this.GetComponent<UISprite>().depth = 10;


                        mWndSkill.shortSkillDic[key] = mWndSkill.shortSkillDic[2];
                        mWndSkill.shortSkillDic[2] = value;
                    }
                    else
                    {

                        SpriteName(surface);
                        if (!mWndSkill.shortSkillDic.Remove(key))
                        {
                            Debug.LogError("删除失败");
                        }
                        mWndSkill.shortSkillDic[2] = value;
                    }
                    
                    break;

                case "ShortCutSkill3":
                    if (mWndSkill.shortSkillDic.ContainsKey(3))
                    {
                        string tempName = surface.GetComponent<UISprite>().spriteName;
                        surface.GetComponent<UISprite>().spriteName = mus.spriteName;
                        mus.spriteName = tempName;

                        this.transform.localPosition = Vector3.zero;
                        this.GetComponent<UISprite>().depth = 10;

                        mWndSkill.shortSkillDic[key] = mWndSkill.shortSkillDic[3];
                        mWndSkill.shortSkillDic[3] = value;
                    }
                    else
                    {
                        SpriteName(surface);
                        if (!mWndSkill.shortSkillDic.Remove(key))
                        {
                            Debug.LogError("删除失败");
                        }
                        mWndSkill.shortSkillDic[3] = value;
                    }

                    break;

                case "ShortCutSkill4":

                    if (mWndSkill.shortSkillDic.ContainsKey(4))
                    {
                        string tempName = surface.GetComponent<UISprite>().spriteName;
                        surface.GetComponent<UISprite>().spriteName = mus.spriteName;
                        mus.spriteName = tempName;

                        this.transform.localPosition = Vector3.zero;
                        this.GetComponent<UISprite>().depth = 10;

                        mWndSkill.shortSkillDic[key] = mWndSkill.shortSkillDic[4];
                        mWndSkill.shortSkillDic[4] = value;
                    }
                    else
                    {
                        SpriteName(surface);
                        if (!mWndSkill.shortSkillDic.Remove(key))
                        {
                            Debug.LogError("删除失败");
                        }
                        mWndSkill.shortSkillDic[4] = value;
                    }

                    break;

                default:
                    mus.spriteName = "hahahah";
                    this.transform.localPosition = Vector3.zero;

                    if( !mWndSkill.shortSkillDic.Remove(key))
                    {
                        Debug.LogError("删除失败");
                    }
                    break;
            }
        }
        else
        {
            mus.spriteName = "hahahah";
        }

    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="keys">Keys.</param>
    void GetValue(int keys)
    {
        if (mWndSkill.shortSkillDic.ContainsKey(keys))
        {
            this.key = keys;
            value = mWndSkill.shortSkillDic[key];
        }
    }

    /// <summary>
    /// Sprites the Spritename.
    /// </summary>
    /// <param name="obj">Object.</param>

    void SpriteName(GameObject obj)
    {
        obj.GetComponent<UISprite>().spriteName = mus.spriteName;
        mus.spriteName = "hahahah";
        this.transform.localPosition = Vector3.zero;
        this.GetComponent<UISprite>().depth = 10;

    }

}
