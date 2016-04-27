using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class NpcManager
    {
        public Dictionary<string, NpcObj> objDic = new Dictionary<string, NpcObj>();
        public void OnUpdate(long tickNow)
        {
            foreach (var obj in objDic)
            {
                if (obj.Value.Enable)
                {
                    obj.Value.OnUpdate(tickNow);
                }
            }
        }
    }
}
