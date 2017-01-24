using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System;
using SLCGame.Unity;

namespace SLCGame.Unity
{
    /// <summary>
    /// lua基础类
    /// </summary>
    public class LuaBehaviour : MonoBehaviour
    {
        private string m_Data = null;
        private BundleCache m_Bundle = null;
        private List<LuaFunction> m_Buttons = new List<LuaFunction>();

        private LuaFunction m_AwakeFunc = null;
        private LuaFunction m_UpdateFunc = null;
        private LuaFunction m_LateUpdateFunc = null;
        private LuaFunction m_FixedUpdateFunc = null;
        private LuaFunction m_LevelLoaded = null;

        public void OnInit(string text = null)
        {
            m_Data = text;
            InitAssetBundle();
            InitLua();
            DebugMod.Log("OnInit---->>>" + name + " text:>" + text);
        }

        public void InitAssetBundle()
        { 
            m_Bundle = AssetBundleMgr.Instance.LoadUIAssetBundle(this.gameObject.name);
        }

        public void InitLua()
        {
            LuaScriptMgr.Instance.DoFile(PathMod.LuaUI + this.gameObject.name);
        }

        protected void Awake()
        {
            m_AwakeFunc = GetLuaFunction("Awake");
            m_UpdateFunc = GetLuaFunction("Update");
            m_LateUpdateFunc = GetLuaFunction("LateUpdate");
            m_FixedUpdateFunc = GetLuaFunction("FixedUpdate");
            m_LevelLoaded = GetLuaFunction("OnLevelLoaded");
            if (m_AwakeFunc != null)
            {
                m_AwakeFunc.Call(gameObject);
            }
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
