using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SLCGame.Tools.Unity;

namespace SLCGame.Unity
{
    public delegate void VoidDelegate();
    public delegate bool BoolDelegate();
    public delegate int IntDelegate();
    public delegate float FloatDelegate();
    public delegate Vector3 CalculatePositionDelegate(Vector3 postion);

    public delegate void PressMoveDelegate(bool isleft, bool state);

    public delegate void DataViewDelegateByID(int roldid);

    public delegate void MessageBoxResultDelegate(MsgResult result);


    // 窗口销毁回调
    public delegate void WndDestroyDelegate(UIWndBase sender, EventWndDestroy args);

    // 窗口选中回调
    public delegate void WndSelectedDelegate(UIWndBase sender, EventWndSelected args);


    //public delegate void EndStageDelegate(StageBase sender, EventEndStage args);


 

    public class U3dCache
    {
        public List<U3DObject> spwanList; //池外元素
        public List<U3DObject> despawnList;//池内元素

        public U3dCache( U3DObject uobj )
        {
            spwanList = new List<U3DObject>();
            despawnList = new List<U3DObject>();
            despawnList.Add(uobj);
        }

        public U3DObject SpawnOne()
        {
            U3DObject uobj = null;
            if (despawnList.Count > 0)
            {
                spwanList.Add(despawnList[0]);
                uobj = despawnList[0];
                despawnList.Remove(uobj);
                uobj.DespawnHandler = Despawn;
                uobj.OnDestroyHandler = OnDestroy;
            }
            else if (spwanList.Count > 0)
            {
                uobj = U3DMod.Clone<U3DObject>(spwanList[0]);
                spwanList.Add(uobj);
                uobj.DespawnHandler = Despawn;
                uobj.OnDestroyHandler = OnDestroy;
            }
            return uobj;
        }

        public void Despawn(U3DObject uobj)
        {
            if (spwanList.Contains(uobj))
            {
                U3DMod.AddChild(null, uobj.gameObject,true);
                uobj.Hide();
                spwanList.Remove(uobj);
                despawnList.Add(uobj);
            }
        }

        public void OnDestroy(U3DObject uobj)
        {
            if (spwanList.Contains(uobj))
            {
                spwanList.Remove(uobj);
            }

            if (despawnList.Contains(uobj))
            {
                despawnList.Remove(uobj);
            }
        }

        public void Clear()
        {
            foreach( U3DObject uobj in spwanList )
            {
                uobj.Destroy();
            }
            spwanList.Clear();

            foreach (U3DObject uobj in despawnList)
            {
                uobj.Destroy();
            }
            despawnList.Clear();
        }
    }
}