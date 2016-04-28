using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

namespace SPSGame.Unity
{
    public class Timers
    {
        public class Timer
        {
            public int timerID;

            public float wait;
            public float interval;
            public int repeatTimes;
            public System.Action action;

            public float timerCounter = 0;
            public int numCounter = 0;
            public bool active = true;

            public bool Tick()
            {
                if (!active) return true;

                timerCounter += Time.deltaTime;

                if( timerCounter >= wait + interval*numCounter )
                {
                    action();
                    numCounter++;
                }

                if (numCounter >= repeatTimes||(numCounter>=1&&interval<=0))
                    return false;

                return true;
            }

        }

        Dictionary<int, Timer> mTimerDic = new Dictionary<int, Timer>();

        int mIndex = 0;


        /// <summary>
        /// 加入计时器
        /// </summary>
        /// <param name="wait">第一次的等待时间</param>
        /// <param name="interval">执行间隔</param>
        /// <param name="ac">执行函数</param>
        /// <returns>返回计时器编号，小于零时为创建失败</returns>
        public int AddTimer(System.Action action, float wait, float interval = 0, int repeattimes = 1)
        {
            Timer tc = new Timer();
            tc.timerID = mIndex++;
            tc.wait = wait;
            tc.interval = interval;
            tc.action = action;
            tc.repeatTimes = repeattimes;
            mTimerDic[mIndex] = tc;
            return 0;
        }


        /// <summary>
        /// 停止计时器
        /// </summary>
        /// <param name="timerID">计时器编号</param>
        /// <returns>成功与否</returns>
        public bool StopTimer( int timerID )
        {
            if( mTimerDic.ContainsKey(timerID) )
            {
                mTimerDic[timerID].active = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 恢复计时器
        /// </summary>
        /// <param name="timerID">计时器编号</param>
        /// <returns>成功与否</returns>
        public bool ResumeTimer(int timerID)
        {
            if (mTimerDic.ContainsKey(timerID))
            {
                mTimerDic[timerID].active = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <param name="timerID">计时器编号</param>
        /// <returns>成功与否</returns>
        public bool ResetTimer(int timerID)
        {
            if (mTimerDic.ContainsKey(timerID))
            {
                Timer tc = mTimerDic[timerID];
                tc.numCounter = 0;
                tc.timerCounter = 0;
                tc.active = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除计时器
        /// </summary>
        /// <param name="timerID">计时器编号</param>
        /// <returns>成功与否</returns>
        public bool DeleteTimer(int timerID)
        {
            if (mTimerDic.ContainsKey(timerID))
            {
                mTimerDic.Remove(timerID);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 清楚所有计时器
        /// </summary>
        public void Clear()
        {
            mTimerDic.Clear();
        }


        public void Update()
        {
            int[] keys = mTimerDic.Keys.ToArray();
            for( int i=0;i<keys.Length;++i )
            {
                if (!mTimerDic[keys[i]].Tick())
                {
                    mTimerDic.Remove(keys[i]);
                }
            }
        }
    }
}