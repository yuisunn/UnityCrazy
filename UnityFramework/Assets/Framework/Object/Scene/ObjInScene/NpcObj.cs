using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class NpcObj : SceneObject
    {
        public NpcObj(int spriteid)
            :base(spriteid)
        {

        }
        protected NpcData m_objData = new NpcData();
    }
}
