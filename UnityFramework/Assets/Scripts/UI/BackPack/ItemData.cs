using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public enum SortType
{
    NumMaxToMin
}
public class ItemData : IComparable
{
    /// <summary>
    /// 物品排序规则
    /// </summary>
    public static SortType sortType = SortType.NumMaxToMin;
    /// <summary>
    /// 物品ID
    /// </summary>
    public long ID;
    /// <summary>
    /// 物品名称
    /// </summary>
    public string name;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int num;
    /// <summary>
    /// 道具类型：0武器，1卷轴，2碎片，3其他
    /// </summary>
    public int type;
    /// <summary>
    /// 位置
    /// </summary>
    public int location;

    /// <summary>
    /// 精灵图片的名称
    /// </summary>
    public string spriteName;
    public int CompareTo(object obj)
    {
        ItemData tmpData = obj as ItemData;
        //if (sortType == SortType.NumMaxToMin)
        //{
            return NumCompareTo(tmpData);
        //}
        //else
        //{
        //    return 其他排序规则(tmpData);
        //}
    }
    public int NumCompareTo(ItemData itemData)
    { 
        if(this.num == itemData.num)
        {
            return 0;
        }
        else if (this.num > itemData.num)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
