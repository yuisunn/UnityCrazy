using UnityEngine;
using System.Collections;


namespace SPSGame.Unity
{

    public class UIHudNPCInfo : UIObject
    {

        public UILabel charName;



        public void SetInfo(string name)
        {
            charName.text = name;
        }

    }
}