using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class MyItemManager
    {
        private static MyItemManager _Instance;
        public static MyItemManager Instance
        {
            get
            {
                if (null == _Instance)
                    _Instance = new MyItemManager();
                return _Instance;
            }
        }

    }
}
