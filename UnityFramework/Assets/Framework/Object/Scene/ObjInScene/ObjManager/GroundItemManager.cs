using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class GroundItemManager
    {
        public Dictionary<string, GroundItemObj> objDic = new Dictionary<string, GroundItemObj>();
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
