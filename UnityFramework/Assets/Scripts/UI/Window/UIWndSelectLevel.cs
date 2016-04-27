using UnityEngine;
using System.Collections;
using SPSGame.Unity;

public class UIWndSelectLevel : UIWndBase
{
    public GameObject btnBack = null;///返回按钮
    public GameObject anyKeyBack = null;//点击屏幕任意位置
    public GameObject monsterDescript = null;//描述面板

    public UILabel modeNameLbl = null;//怪物名称
    public UILabel figuresshowLbl = null;//人物说明
    public UILabel attackCharacteristicsLbl = null;//攻击特色
    public UILabel styleLbl = null;//攻击特色

    public UISprite monsterHead = null;

    
    public GameObject[] btnGo = null;//前往按钮的数组
    public GameObject[] normal = null;//普通怪物的数组
    public GameObject[] elite = null;//精英怪物的数组
    public GameObject[] boss = null;//boss怪物的数组
    public GameObject[] clockSprite = null;//锁图片
    public GameObject[] mask = null;//锁图片

    public UISprite[] kuangSpr = null;

    public UILabel[] openOrMonsterLevelLbl = null;//开启级别或怪物级别的描述

    public UILabel[] sceneLbl = null;//开启级别或怪物级别的描述
    //private bool isOpen = false;//是否开启
    protected override void Start()
    {
        base.Start();
        ListenOnClick(btnGo[0], OnClickBtnGo);
        ListenOnClick(btnGo[1], OnClickBtnGo);

        ListenOnClick(btnBack, OnClickBtnBack);
       // //ListenOnClick(normal[0], OnClickNormal);
       // ListenOnClick(elite[0], OnClickElite);
       // ListenOnClick(boss[0], OnClickBoss);

       //// ListenOnClick(normal[1], OnClickNormal);
       // ListenOnClick(elite[1], OnClickElite);
       // ListenOnClick(boss[1], OnClickBoss);

       // ListenOnClick(anyKeyBack, OnClickanyKeyBack);
        U3DMod.SetActive(monsterDescript, false);

        ListenOnPress(normal[0], OnPressActive);
        ListenOnPress(elite[0], OnPressActive);
        ListenOnPress(boss[0], OnPressActive);

        ListenOnPress(normal[1], OnPressActive);
        ListenOnPress(elite[1], OnPressActive);
        ListenOnPress(boss[1], OnPressActive);
    }





    bool appearMonDes = false;
    void OnPressActive(GameObject obj,bool appear)
    {
        appearMonDes = appear;
        foreach (var item in normal)
        {
             if (item == obj)
            {
                if (appearMonDes == true)
                {
                    ShowDescript(0);
                }
                else
                {
                    U3DMod.SetActive(monsterDescript, false);
                }
            }
        }

        foreach (var item in elite)
        {
            if (item == obj)
            {
                if (appearMonDes == true)
                {
                    ShowDescript(1);
                }
                else
                {
                    U3DMod.SetActive(monsterDescript, false);
                }
            }
        }

        foreach (var item in boss)
        {
            if (item == obj)
            {
                if (appearMonDes == true)
                {
                    ShowDescript(2);
                }
                else
                {
                    U3DMod.SetActive(monsterDescript, false);
                }
            }
        }
    }

    /// <summary>
    /// 点击BtnGo1的处理
    /// </summary>
    /// <param name="obj"></param>
    void OnClickBtnGo(GameObject obj)
    {
        for (int i = 0; i < btnGo.Length; i++)
        {
            if (btnGo[i] == obj)
            {
                BtnGoHandle(i);
            }
        }
    }

    /// <summary>
    /// 点击BtnBack的处理
    /// </summary>
    /// <param name="obj"></param>
    void OnClickBtnBack(GameObject obj)
    {
        Hide();
    }

    ///// <summary>
    ///// 地图1中的普通小怪描述
    ///// </summary>
    ///// <param name="obj"></param>
    //void OnClickNormal(GameObject obj)
    //{
    //    if (obj == normal[0])
    //        ShowDescript(0);
    //    if (obj == normal[1])
    //        ShowDescript(0);
    //}

    ///// <summary>
    ///// 地图1中的精英小怪描述
    ///// </summary>
    ///// <param name="obj"></param>
    //void OnClickElite(GameObject obj)
    //{
    //    if (obj == elite[0])
    //        ShowDescript(1);
    //    if (obj == elite[1])
    //        ShowDescript(1);
    //}

    ///// <summary>
    ///// 地图1中的Boss描述
    ///// </summary>
    ///// <param name="obj"></param>
    //void OnClickBoss(GameObject obj)
    //{
    //    if (obj == boss[0])
    //        ShowDescript(2);
    //    if (obj == boss[1])
    //        ShowDescript(2);
    //}

    ///// <summary>
    ///// 点击任意位置 关闭描述面板
    ///// </summary>
    ///// <param name="obj"></param>
    //void OnClickanyKeyBack(GameObject obj)
    //{
    //    U3DMod.SetActive(monsterDescript, false);
    //}

    /// <summary>
    /// 地图开启等级或者已开启地图怪物等级描述
    /// </summary>
    /// <param name="id">地图id</param>
    /// <param name="isOpen">是否开启</param>
    void OpenOrMonsterLevelLblContent(int id,bool isOpen)
    {
        switch (id)
        {
            case 0:
                openOrMonsterLevelLbl[0].text = isOpen ? "怪物等  级Lv50 -59" : "开启等级Lv50";
                ChangeSpr(id, isOpen);
                break;
            case 1:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv40 -49" : "开启等级Lv40";
                ChangeSpr(id, isOpen);

                break;
            case 2:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv30 -39" : "开启等级Lv30";
                ChangeSpr(id, isOpen);

                break;
            case 3:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv20 -29" : "开启等级Lv20";
                ChangeSpr(id, isOpen);

                break;
            case 4:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv10 -19" : "开启等级Lv10";
                ChangeSpr(id, isOpen);

                break;
            case 5:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv1 -9" : "开启等级Lv1";
                ChangeSpr(id, isOpen);

                break;
            case 6:
                openOrMonsterLevelLbl[1].text = isOpen ? "怪物等级Lv10 -19" : "开启等级Lv10";
                ChangeSpr(id, isOpen);

                break;
        }
    }


    void ChangeSpr(int id, bool sb)
    {
        openOrMonsterLevelLbl[id].color = sb ? Color.white : Color.red;
        sceneLbl[id].color = sb ? Color.white : Color.yellow;

        normal[id].GetComponent<UISprite>().spriteName = sb ? "guaiwutuan1" : "guaiwutuan1hui";
        elite[id].GetComponent<UISprite>().spriteName = sb ? "gauiwutuan2" : "gauiwutuan2hui";
        boss[id].GetComponent<UISprite>().spriteName = sb ? "guaiwutuan3" : "guaiwutuan3hui";

        btnGo[id].GetComponent<UISprite>().spriteName = sb ? "qianwang" : "qianwanghui";
        GameObject go = U3DMod.FindChild(btnGo[id].transform, "Dise");
        go.GetComponent<UISprite>().spriteName = sb ? "qianwanganniu" : "qianwanganniuhui";

        kuangSpr[id].spriteName = sb ? "guaiwutouxiangwenankuang" : "touxiangwenankuanghui";

        if (sb == true)
        {
            clockSprite[id].SetActive(false);
            mask[id].SetActive(false);
        }
    }

    void ShowDescript(int id)
    {
        U3DMod.SetActive(monsterDescript, true);
        switch (id)
        {
            case 0:
                styleLbl.text = "小兵";
                monsterHead.spriteName = "guaiwutouxiang1";

                break;
            case 1:
                styleLbl.text = "精英";
                monsterHead.spriteName = "gauiwutouxiang2";

                break;
            case 2:
                styleLbl.text = "Boss";
                monsterHead.spriteName = "guaiwutouxiang3";

                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    /// <summary>
    /// 前往按纽的处理
    /// </summary>
    /// <param name="id"></param>
    void BtnGoHandle(int id)
    {
        switch (id)
        {
            case 0:
                Logicer.ChangeScene(102);
                break;
            case 1:
                Logicer.ChangeScene(102);
                break;
            case 2:
                Logicer.ChangeScene(104);

                break;
            case 3:
                Logicer.ChangeScene(105);

                break;
            case 4:
                Logicer.ChangeScene(106);

                break;
            case 5:
                Logicer.ChangeScene(107);

                break;
        }
    }
}
