using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;
using System.Linq;

namespace SPSGame.Unity
{
    public class StageBase 
    {
        public EndStageDelegate OnEndStage = null;

//         protected override void Awake()
//         {
//             base.Awake();
//             DontDestroyOnLoad(gameObject);
//         }
// 
//         protected override void Start()
//         {
//             base.Start();
//             StartStage();
//         }

        public virtual void StartStage()
        {

        }


        public virtual void EndStage()
        {

        }

    }

    
}
