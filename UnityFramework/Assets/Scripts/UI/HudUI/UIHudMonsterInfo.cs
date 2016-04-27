using UnityEngine;
using System.Collections;

namespace SPSGame.Unity
{
    public class UIHudMonsterInfo : UIObject
    {
        public UILabel nameLab = null;
        //public UILabel levelLab = null;
        public UISlider healthSld = null;
        public GameObject blood = null;
        public void SetInfo(string name,int level,int maxhealth,int currenthealth)
        {
            nameLab.text = name;
            //levelLab.text = level.ToString();
            healthSld.value = (float)currenthealth / maxhealth;
        }

        public void ShowBlood(bool isshow)
        {
            U3DMod.SetActive(blood, isshow);
        }
    }
}