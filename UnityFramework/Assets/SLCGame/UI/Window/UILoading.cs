using System; 
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SLCGame.Unity
{ 
    public class UILoading : UIObject
    {

        public Texture loadingBack = null;
        public Text progresLabel = null;
        public Slider progresSlider = null;
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
         
        protected void Update()
        { 

            if (GetProgres != null)
                progresPersent = GetProgres();

        }
    }
}
