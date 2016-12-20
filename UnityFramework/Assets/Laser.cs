using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour {

    //游戏对象，这里是线段对象  
    private GameObject LineRenderGameObject;

    //线段渲染器  
    private LineRenderer lineRenderer;

    //设置线段的个数，标示一个曲线由几条线段组成  
    private int lineLength = 4;

    //分别记录4个点，通过这4个三维世界中的点去连接一条线段  
    private Vector3 v0 = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 v1 = new Vector3(0.0f, 1.0f, 0.0f);
    private Vector3 v2 = new Vector3(0.0f, 0.0f, 1.0f);
    private Vector3 v3 = new Vector3(1.0f, 0.0f, 0.0f); 
 
    void Update()
    {

        //在游戏更新中去设置点  
        //根据点将这个曲线链接起来  
        //第一个参数为 点的ID   
        //第二个 参数为点的3D坐标  
        //ID 一样的话就标明是一条线段  
        //所以盆友们须要注意一下！  

        lineRenderer.SetPosition(0, v0);
        lineRenderer.SetPosition(1, v1);
        lineRenderer.SetPosition(2, v2);
        lineRenderer.SetPosition(3, v3);
         
    } 
}
