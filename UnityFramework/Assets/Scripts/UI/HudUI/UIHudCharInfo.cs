using UnityEngine;
using System.Collections;


namespace SPSGame.Unity
{

    public class UIHudCharInfo : UIObject
    {

        public UILabel charName;
        public UILabel charLevel;


        public void SetInfo( string name,int level )
        {
            charName.text = name;
            charLevel.text = string.Format("LV{0}", level);
        }

    }
}