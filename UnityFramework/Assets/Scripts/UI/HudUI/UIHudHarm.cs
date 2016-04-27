using UnityEngine;
using System.Collections;
using SPSGame.Unity;


public class UIHudHarm : UIObject
{
    public GameObject hudTextPrefab;//HUDText与社体
    public EHurtType style = EHurtType.Normal;//普通伤害

    public AnimationCurve scaleCurveNormal = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.25f, 1f) });
    public AnimationCurve scaleCurveCritacl = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 2.5f), new Keyframe(0.25f, 1f) });

    private HUDText mHudText;//HUDTexture


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="target">跟随对象</param>
    /// <returns></returns>
    public void Init( GameObject target,float extraheight )
    {
        GameObject mHudtextGo = U3DMod.Clone(hudTextPrefab);
        U3DMod.AddChild(gameObject, mHudtextGo);
        mHudText = mHudtextGo.GetComponent<HUDText>();

        UIFollowTarget followTarget = mHudText.GetComponent<UIFollowTarget>();
        followTarget.target = target.transform;
        followTarget.extraHeight = extraheight+0.5f;
        followTarget.gameCamera = CameraManager.Instance.camera3D;
        followTarget.uiCamera = UICamera.currentCamera;
    }


    /// <summary>
    /// 伤害数值
    /// </summary>
    /// <param name="style">伤害的类型</param>
    /// <param name="hurt"> 伤害值</param>
    public void HurtNumber(EHurtType style, int hurt)
    {
        switch (style)
        {
            case EHurtType.Normal:
                mHudText.scaleCurve = scaleCurveNormal;
                mHudText.Add("-" + hurt, Color.red, 0);
                break;
            case EHurtType.Crit:
                mHudText.scaleCurve = scaleCurveCritacl;
                Color c = new Color(255, 0, 255, 1);
                mHudText.Add("暴击  -" + hurt, c, 0);

                break;
            case EHurtType.Cure:
                mHudText.scaleCurve = scaleCurveNormal;
                mHudText.Add(" + " + hurt, Color.green, 0);
                break;
            case EHurtType.Miss:
                mHudText.scaleCurve = scaleCurveNormal;
                mHudText.Add("闪避", Color.yellow, 0);
                break;
        }
    }
}
