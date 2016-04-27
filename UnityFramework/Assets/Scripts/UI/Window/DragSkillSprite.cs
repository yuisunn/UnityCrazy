using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Unity;
using System.Linq;
using SPSGame;

public class DragSkillSprite : UIDragDropItem 
{
    public GameObject go;
    public GameObject wndSkill;
    private UIWndSkill mWndSkill;
    public UISprite mus;
    protected override void Start()
    {
        base.Start();
        mWndSkill = wndSkill.GetComponent<UIWndSkill>();
        mus = this.transform.GetComponent<UISprite>();
    }

    protected override void OnDragDropStart()
    {//在克隆的icon上调用的
        base.OnDragDropStart();
        transform.parent = go.transform;
        this.GetComponent<UISprite>().depth = 20;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface );
        if (surface != null)
        {
            //当一个技能拖到了快捷方式上的时候
            switch (surface.name )
            {
                case "ShortCutSkill":
                    if (mWndSkill.shortSkillDic.Values.Contains(mWndSkill.mid) && !DragShortSkillCondition())
                    {
                        UIManager.MsgBox("提示信息", "此技能已经放入快捷栏中", MsgStyle.Yes, null);
                        break;
                    }
                    else
                    {
                        if (DragShortSkillCondition())
                        {
                            UIManager.MsgBox("提示信息", "大招无法放入快捷栏中", MsgStyle.Yes, null);
                        }
                        else
                        {
                            SpriteName(0);
                            mWndSkill.shortSkillDic[1] = mWndSkill.mid;
                        }
                    }

                    break;
                case "ShortCutSkill2":
                    if (mWndSkill.shortSkillDic.Values.Contains(mWndSkill.mid) && !DragShortSkillCondition())
                    {
                        UIManager.MsgBox("提示信息", "此技能已经放入快捷栏中", MsgStyle.Yes, null);
                        break;
                    }
                    else
                    {
                        if (DragShortSkillCondition())
                        {
                            UIManager.MsgBox("提示信息", "大招无法放入快捷栏中", MsgStyle.Yes, null);
                        }
                        else
                        {
                            SpriteName(1);
                            mWndSkill.shortSkillDic[2] = mWndSkill.mid;
                        }
                    }
                    break;
                case "ShortCutSkill3":
                    if (mWndSkill.shortSkillDic.Values.Contains(mWndSkill.mid) && !DragShortSkillCondition())
                    {
                        UIManager.MsgBox("提示信息", "此技能已经放入快捷栏中", MsgStyle.Yes, null);
                        break;
                    }
                    else
                    {
                        if (DragShortSkillCondition())
                        {
                            UIManager.MsgBox("提示信息", "大招无法放入快捷栏中", MsgStyle.Yes, null);
                        }
                        else
                        {
                            SpriteName(2);
                            mWndSkill.shortSkillDic[3] = mWndSkill.mid;
                        }
                    }
                    break;
                case "ShortCutSkill4":
                    if (mWndSkill.shortSkillDic.Values.Contains(mWndSkill.mid) && !DragShortSkillCondition())
                    {
                        UIManager.MsgBox("提示信息", "此技能已经放入快捷栏中", MsgStyle.Yes, null);
                        break;
                    }
                    else
                    {
                        if (DragShortSkillCondition())
                        {
                            UIManager.MsgBox("提示信息", "大招无法放入快捷栏中", MsgStyle.Yes, null);
                        }
                        else
                        {
                            SpriteName(3);
                            mWndSkill.shortSkillDic[4] = mWndSkill.mid;
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 得到UISprite
    /// </summary>
    /// <param name="i">第几个格子</param>
    void SpriteName(int i)
    {
        mWndSkill.shortSkillIcon[i].GetComponent<UISprite>().spriteName = mus.spriteName;
    }

    bool DragShortSkillCondition()
    {
        return ( 0 == mWndSkill.pos1 )? true : false;
    }


}
