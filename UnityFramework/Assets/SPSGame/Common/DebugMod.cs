using System;
using System.Diagnostics;
using UnityEngine;

namespace SPSGame.Unity
{
    public class DebugMod
    {
        public DebugMod()
        { 
        }

        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message.ToString());
        }
         
        public static void LogError(object message)
        { 
            UnityEngine.Debug.Log(message.ToString());
        }
        public static void LogError(object category, object message)
        {
            UnityEngine.Debug.Log(message.ToString());
        }
        public static void LogException(Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
        public static void LogException(Exception e, string extMsg, bool showMsgBox)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
        public static void LogFormatStack(StackTrace stackTrace, string extMsg)
        {
            UnityEngine.Debug.Log(extMsg);
        }
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.Log(message.ToString());
        }
        public static void SetLogError(DebugMod.LogDelegate ld)
        {
            UnityEngine.Debug.LogError(ld.ToString());
        }
        public static void SetLogNormal(DebugMod.LogDelegate ld)
        {
            UnityEngine.Debug.Log(ld.ToString());
        }
        public static void SetLogWarn(DebugMod.LogDelegate ld)
        {
            UnityEngine.Debug.LogWarning(ld.ToString());
        }
        public static void ShowNotifyMsg(object message)
        {
            UnityEngine.Debug.Log(message.ToString());
        }
        public delegate void LogDelegate(string msg);
    }
}