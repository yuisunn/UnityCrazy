using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{

    public class UILogin : UIWndBase
    {

        public GameObject loginbutton;
        public UILabel namelabel;
        public UILabel pwdlabel;
        public UILabel iplabel;
        public UIInput ipinput;
        public UIInput nameinput;
        public UIInput pwinput;

        public UIToggle localtoggle = null;

        public GameObject back;
        // Use this for initialization

        
        protected override void Awake()  
        {
            base.Awake();
            ListenOnClick(loginbutton, OnLogin);
            localtoggle.onChange.Add(new EventDelegate(this,"OnChange"));
            //得到设置名字的key
            nameinput.value = PlayerPrefs.HasKey("_NAME") ? PlayerPrefs.GetString("_NAME") : "test1";
            ipinput.value = PlayerPrefs.HasKey("_IP") ? PlayerPrefs.GetString("_IP") : "192.168.10.251";
            pwinput.value = PlayerPrefs.HasKey("_PW") ? PlayerPrefs.GetString("_PW") : "11111";
        }

        protected override void Start()
        {
            int local = PlayerPrefs.GetInt("_LOCAL", 0);
            localtoggle.Set(local ==1);

            OnChange();          
            //UnityMain.Instance.Timers.AddTimer(() => { Logicer.LoginServer(namelabel.text, pwdlabel.text); }, 0.1f);
        }

        void OnChange()
        {
            PlayerPrefs.SetInt("_LOCAL", localtoggle.value ? 1 : 0);
            Logicer.isLocal = localtoggle.value;
        }

        void OnLogin(GameObject go)
        {

            UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
            if (state == UnityEngine.NetworkReachability.NotReachable)
            {
                DebugMod.LogError("net wrong");
            }

            Logicer.LoginServer(namelabel.text, pwdlabel.text,iplabel.text);
            PlayerPrefs.SetString("_NAME", namelabel.text);//保存输入的名字
            PlayerPrefs.SetString("_IP", iplabel.text);//保存输入的名字
            PlayerPrefs.SetString("_PW", pwdlabel.text);//保存输入的名字

        }
        
    }
}