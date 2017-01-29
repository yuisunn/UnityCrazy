using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System;
using SLCGame.Unity;
using UnityEngine.UI;

namespace SLCGame.Unity
{
    /// <summary>
    /// lua基础类
    /// </summary>
    public class LuaBehaviour : MonoBehaviour
    {
        private string m_Data = null;
        private BundleCache m_Bundle = null; 
        private Dictionary<string, LuaFunction> m_Buttons = new Dictionary<string, LuaFunction>();

        private LuaFunction m_AwakeFunc = null;
        private LuaFunction m_UpdateFunc = null;
        private LuaFunction m_LateUpdateFunc = null;
        private LuaFunction m_FixedUpdateFunc = null;
        private LuaFunction m_LevelLoaded = null;

        public void OnInit(string text = null)
        {
            m_Data = text;
            m_Buttons = new Dictionary<string, LuaFunction>();
            InitAssetBundle();
            InitLua(); 
            DebugMod.Log("OnInit---->>>" + name + " text:>" + text);
        }

        public void InitAssetBundle()
        { 
            //m_Bundle = AssetBundleMgr.Instance.LoadUIAssetBundle(this.gameObject.name);
        }

        public void InitLua()
        {
            LuaScriptMgr.Instance.DoFile(PathMod.LuaUI + this.gameObject.name); 
            m_AwakeFunc = GetLuaFunction("Awake");
            m_UpdateFunc = GetLuaFunction("Update");
            m_LateUpdateFunc = GetLuaFunction("LateUpdate");
            m_FixedUpdateFunc = GetLuaFunction("FixedUpdate");
            m_LevelLoaded = GetLuaFunction("OnLevelLoaded");
        }

        protected void Awake()
        {
            if (m_AwakeFunc == null)
            {
                InitLua();
            }
            m_AwakeFunc.BeginPCall();
            m_AwakeFunc.PushObject(this.gameObject);
            m_AwakeFunc.PCall();
            m_AwakeFunc.EndPCall();
        }

        public void Start()
        {
            CallMethod("Start");
        }

        public void Update()
        {
            if (m_UpdateFunc != null)
            {
                m_UpdateFunc.BeginPCall();//需要压入参数调用pcall  call里调用了 beginpcall pcal endpcal
                m_UpdateFunc.Push(Time.deltaTime);
                m_UpdateFunc.Push(Time.unscaledDeltaTime);
                m_UpdateFunc.PCall();
                m_UpdateFunc.EndPCall();
            }
        }

        public void LateUpdate()
        {
            if (m_LateUpdateFunc != null)
            {
                m_FixedUpdateFunc.Call(); 
            }
        }

        public void FixedUpdate()
        {
            if (m_FixedUpdateFunc != null)
            {
                m_FixedUpdateFunc.BeginPCall();
                m_FixedUpdateFunc.Push(Time.fixedDeltaTime);
                m_FixedUpdateFunc.PCall();
                m_FixedUpdateFunc.EndPCall(); 
            } 
        }


        /// <summary>
        /// 获得一个资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected GameObject GetGameObject(string name)
        { 
            return AssetBundleMgr.Instance.LoadUIPerfab(name); 
        }

        protected LuaFunction GetLuaFunction(string funcName)
        {
            string module = name.Replace("(Clone)", "");
            return LuaScriptMgr.Instance.GetLuaFunction(module + "." + funcName);
        }

        protected object[] CallMethod(string func, params object[] args)
        {
            string module = name.Replace("(Clone)", "");
            return LuaScriptMgr.Instance.CallMethod(module, func, args);  
        }


        protected void OnClick()
        {
            CallMethod(name, "OnClick");
        }

        protected void OnClickEvent(GameObject go)
        {
            CallMethod(name, "OnClick", go);
        }
        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc)
        {
            if (go == null || luafunc == null) return;
            m_Buttons.Add(go.name, luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate () {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go)
        {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (m_Buttons.TryGetValue(go.name, out luafunc))
            {
                luafunc.Dispose();
                luafunc = null;
                m_Buttons.Remove(go.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick()
        {
            foreach (var de in m_Buttons)
            {
                if (de.Value != null)
                {
                    de.Value.Dispose();
                }
            }
            m_Buttons.Clear();
        }

        protected void OnDestroy()
        { 
            if (m_Bundle!=null)
            {
                m_Bundle.Release(); 
            } 
            if (LuaScriptMgr.Instance != null)
            {
                LuaScriptMgr.Instance.SafeRelease(ref m_AwakeFunc);
                LuaScriptMgr.Instance.SafeRelease(ref m_UpdateFunc);
                LuaScriptMgr.Instance.SafeRelease(ref m_LateUpdateFunc);
                LuaScriptMgr.Instance.SafeRelease(ref m_FixedUpdateFunc);
                LuaScriptMgr.Instance.SafeRelease(ref m_LevelLoaded);
                LuaScriptMgr.Instance.LuaGC();
            }
            
            DebugMod.Log("~" + name + "was destory!");
        } 
    }
}
