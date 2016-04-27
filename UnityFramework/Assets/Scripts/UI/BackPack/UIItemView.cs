using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIItemView : MonoBehaviour
{
    /// <summary>
    /// 道具图标
    /// </summary>
    public UISprite icon = null;
    /// <summary>
    /// 道具名称
    /// </summary>
    public UILabel name = null;
    /// <summary>
    /// 道具数量
    /// </summary>
    public UILabel itemNum = null;
    /// <summary>
    /// 装备等级图标
    /// </summary>
    public UISprite leveSpr = null;
    /// <summary>
    /// 此格子对应的ItemData的引用
    /// </summary>
    public long itemId = 0;
    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="_data"></param>
    public void init(long itemID, short num, short type, string spriteName, bool isEquiped)
    {
        //激活所有需要显示的
        itemNum.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);

        //如果数量为零
        if (0 >= num)
        {
            Clear();
        }
        else
        {
            //将_data赋值给itemData
            itemId = itemID;
            icon.spriteName = spriteName;
            itemNum.text = num.ToString();
            if (isEquiped)
            {
                name.gameObject.SetActive(true);
            }
            else
            {
                name.gameObject.SetActive(false);
               
            }
            if (1 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "bai";
            }
            else if(2 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "lv";
            }
            else if (3 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "lan";
            }
            else if (4 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "zi";
            }
            else if (5 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "cheng";
            }
            else if (6 == type)
            {
                leveSpr.gameObject.SetActive(true);
                leveSpr.spriteName = "hong";
            }
            else 
            {
                leveSpr.gameObject.SetActive(false);
            }
           
        }
    }
    /// <summary>
    /// 展示玩家的装备
    /// </summary>
    /// <param name="equipID"></param>
    /// <param name="num"></param>
    /// <param name="quality"></param>
    /// <param name="spriteName"></param>
    /// <param name="isEquiped"></param>
    public void ShowPlayerEquip(long equipID, int num, short quality, string spriteName, bool isEquiped)
    {
        if (0 >= num)
        {
            Clear();
            return;
        }

        if (!isEquiped)
        {
            Clear();
            return;
        }

        //激活所有需要显示的
        name.gameObject.SetActive(false);
        itemNum.gameObject.SetActive(false);
        icon.gameObject.SetActive(true);

        //将_data赋值给itemData
        itemId = equipID;

        name.text = "";
        icon.spriteName = spriteName;
    }

    /// <summary>
    /// 清空格子内的数据
    /// </summary>
    public void Clear ()
    {
        itemId = 0;
        itemNum.text = "";
        icon.spriteName = "";
        leveSpr.spriteName = "";
        name.gameObject.SetActive(false);
    }
    /// <summary>
    ///隐藏所有显示
    /// </summary>
    public void Hide()
    {
        icon.gameObject.SetActive(false);
        name.gameObject.SetActive(false);
        itemNum.gameObject.SetActive(false);
    }
}
