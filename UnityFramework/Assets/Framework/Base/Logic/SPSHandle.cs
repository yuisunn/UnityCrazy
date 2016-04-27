using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SPSGame
{
    /// <summary>
    /// 下一步事件参数
    /// </summary>
    public class NextStageEventArgs : EventArgs
    {
        public int StageType { get; set; }
        public int ID;
        public object Tag;
    }

    //下一步事件通知函数
    public delegate void NextStageEventHandler(object sender, NextStageEventArgs args);


}