using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLCGame.Unity;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System.Text;
using System;

[StructLayout(LayoutKind.Explicit)]//auto clr自动排列内存提高值类型效率
internal struct SType
{
    [FieldOffset(0)] //定义内存偏移
    private readonly byte a;//用公共字段表示内存重叠的字节
}

class Ctor
{
    private int a;
    private string s;
    public Ctor()
    {
        a = 1;
        s = "s";
    }

    public Ctor(int a) : this() //调用另一个构造器 避免重复生成 字段初始化在构造器il代码里
    {

    }
}

/// <summary>
/// 扩展方法
/// </summary>
public static class StringBudilerEx
{ 
    //第一个参数加 this 这个方法是第一个参数的class的扩展方法
    //条件
    public static int IndexOf(this StringBuilder s, char c)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == c) return i;
        }
        return -1;
    }

    public static void ShowItem<T>(this IEnumerable<T> collection)
    {
        foreach (var item in collection)
            Debug.Log(item);
    }
}


public interface IComparable<in T>
{
    int CompareTo(T other);
    void TT();
}

public interface ITE
{
    void TT();
}

public sealed class Point : IComparable<Point>, ITE
{
    private int m_x, m_y;
    public Point(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public int CompareTo(Point other)
    {
        return 1;
    }

    public override string ToString()
    {
        return base.ToString();
    }

    void IComparable<Point>.TT()
    {
        throw new NotImplementedException();
    }
     void ITE.TT()
    {

    }
}

public class Tupele<T>//索引器
{
    private T t;
    public Tupele(T item) { t = item; }
    private int a;
    
    public int this[int pos]
    {
        get { return a; }
        set {
            a = pos;
        }
    }

    public int this[string pos]
    {
        get { return a; }
        set
        {
            a = 1;
        }
    } 
 }

internal sealed class GenericType<T>
{
    private T m_Value;
    public GenericType(T value) { m_Value = value; }

    public TOutput Converter<TOutput>()
    {
        T t;
        TOutput result = (TOutput)Convert.ChangeType(m_Value, typeof(TOutput));
        return result;
    } 
}


public class AT
{
    public virtual void M<T, T1>() where T : class
    {

    }
}

public class BT : AT
{
    public override void M<T, T1>()  
    { 
    }
}



 public partial class UITest : MonoBehaviour {

    public void Test()
    {
        var a = new Dictionary<string,int>{ { "sdf",1},{ "sdfsdf",2} }; //集合初始化器
        var b = new UITest() { pre = null };

        var c = new { name = "sdf", age = 12 };//元祖类
                                               //编译器自动创建class名称
                                               //编译器1先判读字段类型2设置公共只读属性 3创建构造器 重写 object的方法
                                               //1如果有两个相同的匿名class 只会创建一个

        Tupele<int> d = new Tupele<int>(1); //索引器
        d[1] = 1;
        int ia = d[1];
    }

    public void TestEx()
    { 
        StringBuilder sb = new System.Text.StringBuilder();
        sb.IndexOf('c');
    }

    public void TestAction()
    {
        Action a = "sdf".ShowItem;
        a();
        UITest aa = new UITest() { pre = null, item = null };//集合初始化器
    }

    enum a
    {
    }
    public GameObject pre;
    public GameObject item;

    private static void Display(String s)
    {
        Debug.Log(s);
    }
    private static void Display<T>(T o)
    {
        Display(o.ToString());
    }

    // Use this for initialization
    void Start () {

        Display("123");
        Display(333);
        Display<String>("sdfeee");

        //object.ReferenceEquals(pre, item);//检测统一性
        //dynamic d = 123;
        //int x = (int)d;
        ////UIMgr.Instance.ShowWindow<TestPanel>(this.gameObject);
        //int a = checked(100 + 1);
        ////AssetBundle ab = AssetBundle.LoadFromFile(PathMod.DataPath + "/cube-bundle");  
        //GameObject bb = Instantiate(pre); 
        //bb.transform.parent = this.transform; 
        //LuaBehaviour tt = bb.AddComponent<LuaBehaviour>(); 
        ////bb.GetComponentInChildren<ToggleGroup>().SetAllTogglesOff();
        //Dropdown obj1 = bb.GetComponentInChildren<Dropdown>();  
        //Slider s = bb.GetComponentInChildren<Slider>();  
        ////obj1.onValueChanged.AddListener(CLick);


        ////Image btn = tt.GetComponentInChildren<Image>();
        ////btn.sprite = loadSprite("1");    
        //Button btn = this.GetComponentInChildren<Button>();
        //btn.onClick.AddListener(CLick);
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
