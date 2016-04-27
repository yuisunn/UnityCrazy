using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class Coroutiner
    {
        private const float FRAMETIME = 0.02F;

        public enum ECoroutineLevel
        {
            High = 0,
            Normal = 1,
            Low = 3,
        }

        public class Coroutine
        {
            public IEnumerator coroutine;

            public int crLevel;
            public int crCounter = 0;

            public bool Tick( bool havetime )
            {
                if (coroutine.Current is WWW)
                {
                    try
                    {
                        if (((WWW)coroutine.Current).isDone)
                        {
                            return coroutine.MoveNext();
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        DebugMod.LogError(coroutine.ToString()+":" + e.Message);
                    }
                }

                if (coroutine.Current is AsyncOperation)
                {
                    try
                    {
                        if (((AsyncOperation)coroutine.Current).isDone)
                        {
                            return coroutine.MoveNext();
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        DebugMod.LogError("Coroutiner error: " + e.Message);
                    }
                }
// 
//                 if (coroutine.Current is WaitForSeconds)
//                 {
//                     if (((WaitForSeconds)coroutine.Current))
//                     {
//                         return coroutine.MoveNext();
//                     }
//                     else
//                     {
//                         return true;
//                     }
//                 }

                bool b = true;
                if (havetime || crCounter++ >= crLevel)
                {
                    try
                    {
                        if (coroutine != null)
                            b = coroutine.MoveNext();
                    }
                    catch (Exception e)
                    {
                        DebugMod.LogError("Coroutiner error: " + e.Message);
                        //object bb = coroutine.Current;
                        b = false;
                    }
                    crCounter = 0;
                }
                return b;
            }
        }


        int mIndex = 0;
        Dictionary<int, Coroutine> mCoroutineDic = new Dictionary<int, Coroutine>();
        public int AddCoroutine(IEnumerator coroutine, ECoroutineLevel level = ECoroutineLevel.High)
        {
            Coroutine cr = new Coroutine();
            cr.coroutine = coroutine;
            cr.crLevel = (int)level;
            mCoroutineDic[mIndex++] = cr;
            return mIndex;
        }

        public void Update()
        {
            float starttime = Time.time;
            int[] keys = mCoroutineDic.Keys.ToArray();
            
            for( int i = 0;i<keys.Length;++i )
            {
                if (mCoroutineDic.ContainsKey(keys[i]))
                    if (!mCoroutineDic[keys[i]].Tick((Time.time - starttime) < FRAMETIME))
                        mCoroutineDic.Remove(keys[i]);
            }
        }
    }

}