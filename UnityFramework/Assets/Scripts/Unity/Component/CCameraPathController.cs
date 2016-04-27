using UnityEngine;
using System.Collections;
using SPSGame.Unity;



public class CCameraPathController : U3DObject
{

    public CameraPathAnimator path1;//ChuanZhang的CameraPathAnimator脚本
    public CameraPathAnimator path2;
    public CameraPathAnimator path3;
    public CameraPathAnimator path4;
    public CameraPathAnimator path5;
    public CameraPathAnimator path6;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < this.GetComponentsInChildren<CameraPathAnimator>().Length; i++)
        {
            this.GetComponentsInChildren<CameraPathAnimator>()[i].animationMode = CameraPathAnimator.animationModes.once;
            this.GetComponentsInChildren<CameraPathAnimator>()[i].playOnStart = false;
            this.GetComponentsInChildren<CameraPathAnimator>()[i].animationObject = CameraManager.Instance.camera3D.transform;            
       }

    }

    /// <summary>
    /// 拉进摄像机 使摄像机近距离照向所选择的英雄
    /// </summary>
    /// <param name="cpa">哪个英雄的摄像机路径动作</param>
    public void CameraNear( CameraPathAnimator cpa ,bool isforward )
    {
        cpa.animationMode = isforward ? CameraPathAnimator.animationModes.once : CameraPathAnimator.animationModes.reverse;
        cpa.Play();
    }
}
