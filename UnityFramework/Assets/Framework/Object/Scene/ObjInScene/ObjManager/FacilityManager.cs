using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//场景内设施
namespace SPSGame
{
    public class FacilityManager
    {
        public Dictionary<string, FacilityObj> objDic = new Dictionary<string, FacilityObj>();
        public void OnUpdate(long tickNow)
        {
            foreach(var obj in objDic)
            {
                if (obj.Value.Enable)
                {
                    obj.Value.OnUpdate(tickNow);
                }
            }
        }
    }
}
