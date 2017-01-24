using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLCGame.Tools;
using LuaInterface;
using System;
using SLCGame.Tools.Unity;

namespace SLCGame.Unity
{
    public class LuaScriptMgr : UnitySingleton<LuaScriptMgr>
    {
        public LuaState m_Lua;
        private LuaLoader m_Loader;
        public HashSet<string> m_FileList = null;
        Dictionary<string, LuaFunction> m_FunDic = null;
          

        public void Init()
        {
            m_FileList = new HashSet<string>();
            m_FunDic = new Dictionary<string, LuaFunction>(); 
            m_Lua = new LuaState();
            m_Loader = new LuaLoader(); 
            this.OpenLibs();
            m_Lua.LuaSetTop(0);

            LuaBinder.Bind(m_Lua);
            LuaCoroutine.Register(m_Lua, this); 
        }

        public void InitStart()
        {
            InitLuaPath();
            InitLuaBundle();
            m_Lua.Start();  //启动LUAVM
        }
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs()
        {
            m_Lua.OpenLibs(LuaDLL.luaopen_pb);
            //m_Lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            //m_Lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            m_Lua.OpenLibs(LuaDLL.luaopen_lpeg);
            m_Lua.OpenLibs(LuaDLL.luaopen_bit);
            m_Lua.OpenLibs(LuaDLL.luaopen_socket_core);

            //this.OpenCJson();
        } 
        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
            if (AppConst.DebugMode)
            {
                string rootPath = PathMod.AppContentPath();
                m_Lua.AddSearchPath(rootPath + "Lua");
                m_Lua.AddSearchPath(rootPath + "View");
                m_Lua.AddSearchPath(rootPath + "ToLua/Lua");
            }
            else
            {
                m_Lua.AddSearchPath(PathMod.DataPath + "lua");
            }
        }
        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle()
        {
            if (m_Loader.beZip)
            {
                m_Loader.AddBundle("lua/lua.unity3d");
                m_Loader.AddBundle("lua/lua_math.unity3d");
                m_Loader.AddBundle("lua/lua_system.unity3d");
                m_Loader.AddBundle("lua/lua_system_reflection.unity3d");
                m_Loader.AddBundle("lua/lua_unityengine.unity3d");
                m_Loader.AddBundle("lua/lua_common.unity3d");
                m_Loader.AddBundle("lua/lua_logic.unity3d");
                m_Loader.AddBundle("lua/lua_view.unity3d");
                m_Loader.AddBundle("lua/lua_controller.unity3d");
                m_Loader.AddBundle("lua/lua_misc.unity3d");

                m_Loader.AddBundle("lua/lua_protobuf.unity3d");
                m_Loader.AddBundle("lua/lua_3rd_cjson.unity3d");
                m_Loader.AddBundle("lua/lua_3rd_luabitop.unity3d");
                m_Loader.AddBundle("lua/lua_3rd_pbc.unity3d");
                m_Loader.AddBundle("lua/lua_3rd_pblua.unity3d");
                m_Loader.AddBundle("lua/lua_3rd_sproto.unity3d");
            }
        }

        public void LuaGC()
        {
            m_Lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        void SendGMmsg(params string[] param)
        {
            Debugger.Log("SendGMmsg");
            string str = "";
            int i = 0;
            foreach (string p in param)
            {
                if (i > 0)
                {
                    str = str + " " + p;
                    Debugger.Log(p);
                }
                i++;
            }
            CallLuaFunction("GMMsg", str);
        }
         
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="module"></param>
        /// <param name="func"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object[] CallMethod(string module, string func, params object[] args)
        {
            string funcName = module + "." + func;
            funcName = funcName.Replace("(Clone)", "");
            return CallLuaFunction(funcName, args);
        }
          
        public void SafeRelease(ref LuaFunction func)
        {
            if (func != null)
            {
                func.Dispose();
                func = null;
            }
        }

        void SafeUnRef(ref int reference)
        {
            if (reference > 0)
            {
                //LuaDLL.lua_unref(lua.L, reference);
                reference = -1;
            }
        }

        public void Destroy()
        {
            if (m_FunDic == null) return;
            foreach (KeyValuePair<string, LuaFunction> kv in m_FunDic)
            {
                kv.Value.Dispose();
            } 
            Debugger.Log("Lua module destroy");
        }

        public object[] DoString(string str)
        {
            return m_Lua.DoString(str);
        }

        public void DoFile(string fileName)
        {
            if (!m_FileList.Contains(fileName))
            {
                m_FileList.Add(fileName);
                m_Lua.DoFile(fileName);
            } 
        }

        public LuaFunction GetLuaFunction(string name)
        {
            LuaFunction func = null;
            if (m_FunDic.TryGetValue(name, out func))
            { 
                func.Call();
            }
            else
            {
                func = m_Lua.GetFunction(name); 
            }
            if (func != null)
            {
                func.Call();
            }
            return func;
        }

        /// <summary>
        /// 不缓存LuaFunction
        /// </summary> 
        public object[] CallLuaFunction(string name, params object[] args)
        {
            LuaFunction func = null;

            if (m_FunDic.TryGetValue(name, out func))
            { 
                return func.Call(args);
            }
            else
            {
                func = m_Lua.GetFunction(name);
                m_FunDic.Add(name, func);  
            }
            if (func != null)
            {
                return func.Call(args);
            }
            return null;
        } 
    }
}
