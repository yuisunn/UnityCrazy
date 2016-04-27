using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;
namespace SPSGame.Unity
{

    public class UIWndCreateRole : UIWndBase
    {
        public UISprite charFirstNameIcon = null;
        public UILabel charLastNameLabel = null;
        public UILabel roleNameLabel = null;

        public GameObject backBoard = null;

        public GameObject createCharBtn = null;
        public GameObject randomLastNameBtn = null;

        public GameObject deleteBtn = null;
        public GameObject selectBtn = null;

        public GameObject showBtn = null;
        public GameObject backBtn = null;

        public GameObject firstNameBtn = null;

        public GameObject createBoard = null;
        public GameObject selectBoard = null;

        public GameObject silderExport = null;
        public GameObject silderExplode = null;
        public GameObject silderLive = null;
        public GameObject silderControll = null;
        public GameObject silderHelp = null;

        public GameObject ExportLab = null;
        public GameObject ExplodeLab = null;
        public GameObject LiveLab = null;
        public GameObject ControllLab = null;
        public GameObject HelpLab = null;

        public GameObject anyKeyBackBtn = null;

        public UISprite[] stars = null;

        public UILabel masterName1 = null;
        public UILabel masterName2 = null;
        public UILabel masterName3 = null;

        public UISprite masterSprName1 = null;
        public UISprite masterSprName2 = null;
        public UISprite masterSprName3 = null;


        public UILabel roleName = null;
        public VoidDelegate OnCreateCharHandler = null;
        public VoidDelegate OnDeleteCharHandler = null;
        public VoidDelegate OnSelectRoleHandler = null;
        public VoidDelegate OnBackFrameHandler = null;
        public VoidDelegate OnShowActionHandler = null;

        public AudioClip[] chuanzhangsounds;
        public AudioClip[] xiaoheisounds;
        public AudioClip[] huonvsounds;
        public AudioClip[] shengqisounds;
        public AudioClip[] jianshengsounds;
        public AudioClip[] bingnvsounds;

        public VoidDelegate OnHideHandler = null;

        private AudioSource souce;
        RandomName mRdname;

        protected override void Awake()
        {
            base.Awake();

            ListenOnClick(createCharBtn, OnClickCreateChar);
            ListenOnClick(randomLastNameBtn, OnClickRandomLastName);
            ListenOnClick(backBtn, OnClickBack);
            ListenOnClick(backBoard, OnClickBack);
            ListenOnClick(deleteBtn, OnClickDeleteChar);
            ListenOnClick(selectBtn, OnClickSelectRole);
            ListenOnClick(firstNameBtn, OnClickFirstName);
            ListenOnClick(showBtn, OnClickShow);
            ListenOnClick(anyKeyBackBtn, OnClickBack);
            InCharExist(false);

            souce = this.GetComponent<AudioSource>();
            TextAsset tex = ResourceManager.Load<TextAsset>("Config/Name");
            mRdname = new RandomName(tex.text, false);

        }

        protected override void Start()
        {
            base.Start();
        }
        /// <summary>
        /// 重写OnEnable（）当激活物体时播放动画
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            TweenPosition[] tweps = this.transform.GetComponentsInChildren<TweenPosition>();
            foreach (TweenPosition twe in tweps)
            {
                twe.enabled = true;
                twe.PlayForward();
            }
            EventDelegate.Remove( tweps[0].onFinished ,Hide);
        }

        public void InCharExist( bool isexist )
        {
            U3DMod.SetActive(createBoard, !isexist);
            U3DMod.SetActive(selectBoard, isexist);
        }

        public void RandomName()
        {

            if(charLastNameLabel != null)  
                charLastNameLabel.text = mRdname.GenerateRandomName();
        }

        void OnClickCreateChar(GameObject obj)
        {
            if (OnCreateCharHandler != null)
                OnCreateCharHandler();
        }

        void OnClickRandomLastName(GameObject obj)
        {
            RandomName();
            RandomIcon();
        }

        void OnClickSelectRole(GameObject obj)
        {
            if (OnSelectRoleHandler != null)
                OnSelectRoleHandler();
        }

        void OnClickDeleteChar(GameObject obj)
        {
            if (OnDeleteCharHandler != null)
                OnDeleteCharHandler();
        }

        void OnClickBack(GameObject obj)
        {
            if (OnHideHandler != null)
                OnHideHandler();
           //当鼠标点击返回时 反向播放动画
            TweenPosition[] tweps = this.transform.GetComponentsInChildren<TweenPosition>();
            foreach (TweenPosition twe in tweps)
            {
                twe.enabled = true;
                twe.PlayReverse();
            }
            tweps[0].SetOnFinished(Hide);
        }


        void OnClickFirstName(GameObject obj)
        {
            RandomIcon();
        }
        /// <summary>
        ///随机图标
        /// </summary>
        public void RandomIcon() 
        {
            charFirstNameIcon.spriteName = "tubiao" + UnityEngine.Random.Range(1, 16);
        }

        void OnClickShow(GameObject obj)
        {
            if (OnShowActionHandler != null)
                OnShowActionHandler();
        }

        /// <summary>
        /// 属性设置
        /// </summary>
        /// <param name="str"></param>
        /// <param name="number"></param>
        public void SetProperty(int export, int explode, int live, int control, int help) 
        {
            silderExport.GetComponent<UISlider>().value = export / 10f;
            ExportLab.GetComponent<UILabel>().text = export.ToString();

            silderExplode.GetComponent<UISlider>().value = explode / 10f;
            ExplodeLab.GetComponent<UILabel>().text = explode.ToString();

            silderLive.GetComponent<UISlider>().value = live / 10f;
            LiveLab.GetComponent<UILabel>().text = live.ToString();

            silderControll.GetComponent<UISlider>().value = control / 10f;
            ControllLab.GetComponent<UILabel>().text = control.ToString();

            silderHelp.GetComponent<UISlider>().value = help / 10f;
            HelpLab.GetComponent<UILabel>().text = help.ToString();
        }
        /// <summary>
        /// 设置星级
        /// </summary>
        /// <param name="number"></param>
        public void SetStarLeve(int number) 
        {
            if (number <= stars.Length && number>0)
            {
                for (int i = 0; i < number; i++)
                {
                    stars[i].spriteName = "xingxingliang";
                   
                }
                for (int j = number - 1; j < stars.Length; j++)
                {
                    stars[j].spriteName = "xingxingan";
                }
                
            }
            else 
            {
                DebugMod.Log("输入数字越界");
            }
        }
        /// <summary>
        /// 设置船长名字
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <param name="name3"></param>
        public void SetMasterName(string name1,string name2,string name3) 
        {
            masterName1.text = name1;
            masterName2.text = name2;
            masterName3.text = name3;
        }
        /// <summary>
        /// 设置姓名显示
        /// </summary>
        /// <param name="str"></param>
        public void SetRoleNameShow(string str) 
        {
            roleName.text = str;
        }
        /// <summary>
        /// 设置玩家头像图标
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
        public void SetRoleScriteName(string str1,string str2,string str3) 
        {
            masterSprName1.spriteName = str1;
            masterSprName2.spriteName = str2;
            masterSprName3.spriteName = str3;
        }
        /// <summary>
        /// 设置音效播放
        /// </summary>
        /// <param name="roleid"></param>

        public void PlaySound(int roleid)
        {
            int index = (int)UnityEngine.Random.Range(0,3);
            if (1 == roleid)
            {
                //船长
                souce.clip = chuanzhangsounds[index];
                souce.Play();
            }
            else if(2 == roleid)
            {
              
                //圣骑
                souce.clip = shengqisounds[index];
                souce.Play();
            }
            else if(3 == roleid)
            {
                //剑圣
                souce.clip = jianshengsounds[index];
                souce.Play();
            }
           else if(4 == roleid)
            {
               //小黑
                souce.clip = xiaoheisounds[index];
                souce.Play();
            }
           else if (5 == roleid) 
            {
                //火女
                souce.clip = huonvsounds[index];
                souce.Play();

            }
            else if(6 == roleid)
            {
                //冰女
                souce.clip = bingnvsounds[index];
                souce.Play();
            }
        }
    }

}