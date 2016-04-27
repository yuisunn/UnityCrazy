using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace SPSGame.Unity
{
    [RequireComponent(typeof(UIPanel))]
    public class UILoading:UIObject
    {

        public UITexture loadingBack = null;
        public UILabel progresLabel = null;
        public UISlider progresSlider = null;
        public FloatDelegate GetProgres;


        public float progresPersent
        {
            get
            {
                return progresSlider.value;
            }
            set
            {
                progresSlider.value = value;
            }

        }



        protected override void Update()
        {
            base.Update();

            if(GetProgres != null)
                progresPersent = GetProgres();
            
        }
    }
}
