using System;

namespace SPSGame
{
    /// <summary>
    /// 游戏Action接口
    /// </summary>
    public abstract class ActionBase
    {
        public int ActionId { get; set; }
        public ActionParam ActParam { get; set; }
        public abstract bool ProcessAction();

    }

}
