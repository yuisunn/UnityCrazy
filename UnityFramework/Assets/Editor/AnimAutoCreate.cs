using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

public class AnimAutoCreate : Editor
{
    //[MenuItem("Test/Test")]
    //static void DoCreateAnimationAssets()
    //{
    //    //创建animationController文件，保存在Assets路径下
    //    AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/animation.controller");
    //    //得到它的Layer， 默认layer为base 你可以去拓展
    //    AnimatorControllerLayer layer = animatorController.GetLayer(0);
    //    //把动画文件保存在我们创建的AnimationController中
    //    AddStateTransition("Assets/Resources/airenlieren@Idle.FBX", layer);
    //    AddStateTransition("Assets/Resources/attack@attack.FBX", layer);
    //    AddStateTransition("Assets/Resources/aersasi@Run.FBX", layer);
    //}

    //private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    //{
    //    UnityEditorInternal.StateMachine sm = layer.stateMachine;
    //    //根据动画文件读取它的AnimationClip对象
    //    AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
    //    //取出动画名子 添加到state里面
    //    State state = sm.AddState(newClip.name);
    //    state.SetAnimationClip(newClip, layer);
    //    //把state添加在layer里面
    //    Transition trans = sm.AddAnyStateTransition(state);
    //    //把默认的时间条件删除
    //    trans.RemoveCondition(0);
    //}
}