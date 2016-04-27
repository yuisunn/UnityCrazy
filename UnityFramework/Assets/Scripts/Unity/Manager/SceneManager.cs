using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class SceneManager : Singleton<SceneManager>
    {
        public WWW _www = null;
        public AsyncOperation _asyn = null;
        public AssetBundle _sceneBundle = null;

        public float progress
        {
            get
            {
                if (_www != null)
                    return _www.progress;

                if (_asyn != null)
                    return _asyn.progress;

                return 1;
            }
        }


        /// <summary>
        /// 加载游戏场景
        /// </summary>
        /// <param name="scenename">场景名</param>
        /// <returns></returns>
        public void LoadGameScene(string scenename,UILoading uiload, VoidDelegate loadfinishdo = null)
        {
            if (_sceneBundle != null)
            {
                LeaveGameScene(() => { LoadGameScene(scenename,uiload, loadfinishdo); });
                return;
            }

            string path = PathMod.AssetPath(string.Format("Scene/{0}.unity3d", scenename));

            uiload.GetProgres = () => { return progress; };
            UnityMain.StartCoroutine(LoadSceneEnumerator(path, scenename, () => 
            { 
                if(loadfinishdo != null) 
                    loadfinishdo();
                //EventManager.Instance.Trigger<EventLoadScaneFinish>(new EventLoadScaneFinish(scenename));
            })); 

        }


        /// <summary>
        /// 离开游戏场景到空场景
        /// </summary>
        /// <param name="loadfinishdo">回调</param>
        /// <returns></returns>
        public void LeaveGameScene(VoidDelegate loadfinishdo)
        {
            UnityMain.StartCoroutine(LoadEmptyScene(loadfinishdo));
        }


        IEnumerator LoadSceneEnumerator(string path, string scenename, VoidDelegate loadfinishdo )
        {
            if (string.IsNullOrEmpty(scenename))
                yield break;

            _www = new WWW(path);
            yield return _www;

            _sceneBundle = _www.assetBundle;
            _www.Dispose();
            _www = null;

            if (_sceneBundle != null)
            {
                _asyn = Application.LoadLevelAsync(scenename);
                yield return _asyn;
                if (_asyn.isDone)
                {
                    yield return 1;                                    
                    try              
                    {
                        if (loadfinishdo != null)
                            loadfinishdo();
                    }
                    catch
                    {
                        DebugMod.LogError("Error occored in LoadSceneEnumerator callback delegate");
                    }


                    _asyn = null;
                }
            }
            else
            {
                DebugMod.LogError("can't load scene from: " + path);
            }
        }


        /// <summary>
        /// 跳转空场景
        /// 为了清理当前内存
        /// </summary>
        private IEnumerator LoadEmptyScene( VoidDelegate loadfinishdo )
        {

            yield return 1;

            //清空特效内存

            //清空对象内存

            Application.LoadLevel("empty");

            yield return 1;

            if(_sceneBundle != null)
            {
                _sceneBundle.Unload(true);
                _sceneBundle = null;
            }

            //清空部分XML节点缓存
            yield return 1;

            //即时回收，收回上次场景占用的内存
            AsyncOperation async = Resources.UnloadUnusedAssets();
            yield return async;

            //垃圾回收
            System.GC.Collect();

            yield return 1;

            if (loadfinishdo != null)
                loadfinishdo();
        }
    }
}