using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class RenderManager:Singleton<RenderManager>
    {
        List<IObject> mObjectList = new List<IObject>();
        int mIndex = 0;

        public void Add(IObject obj)
        {
            mObjectList.Add(obj);
        }

        public void Remove(IObject obj)
        {
            mObjectList.Remove(obj);
        }

        public void FixedUpdate()
        {
            for (mIndex = 0; mIndex < mObjectList.Count; ++mIndex)
            {
                mObjectList[mIndex].RenderFixedUpdate();
            }        
        }

        // Update is called once per frame
        public void Update()
        {
            for (mIndex = 0; mIndex < mObjectList.Count; ++mIndex) 
            {
                mObjectList[mIndex].RenderUpdate();
            }
        }
    }

}