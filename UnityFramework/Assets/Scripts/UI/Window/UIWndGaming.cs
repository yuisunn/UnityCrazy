using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using SPSGame;
namespace SPSGame.Unity
{

    public class UIWndGaming : UIWndBase
    {
        public GameObject backBtn = null;
        public UISprite imageSpr = null;
        public GameObject lineBtn = null;

        public GameObject listBtn = null;
        public GameObject listBtnObj = null;

        public GameObject backPackBtn = null;
        public GameObject skillWndBtn = null;


        public VoidDelegate OnOpenCharDetailHandler = null;
        public VoidDelegate OnOpenSelectLineHandler = null;

        public UISprite imageTopSpr = null;
        public UISprite imageBottomSpr = null;

        public GameObject options = null;
        public GameObject banghuiBtn = null;
        public GameObject jiayuanBtn = null;
        public GameObject maoxianBtn = null;
        public GameObject mijingBtn = null;

        public GameObject tuozhanBtn = null;

        public GameObject zhandou = null;
        public GameObject[] skillArray = null;



//        public GameObject controls = null;

//         public GameObject leftBtn = null;
//         public GameObject rightBtn = null;
        public PressMoveDelegate MoveHandler = null;
        public VoidDelegate LeaveBattleHandler = null;

        public GameObject jobBtn;
        public GameObject jobList;


        public Dictionary<int, ChosenSkillData> chonseDic = new Dictionary<int, ChosenSkillData>();//技能字典
        public Dictionary<int, float> chonseSkillDic = new Dictionary<int, float>();//选择的那个技能的字典

        private float[] maxTimeArr = new float[5];//最大时间
        private float[] leftTimeArr = new float[5]; //剩余时间

        private bool[] isOK = new bool[5] { false, false, false, false, false };//倒计时显示数组
        private bool[] IsActive = new bool[5] { true, true, true, true, true };//技能冷却一秒钟之前不能够点击技能按钮

        //public UILabel[] timeLbl;//显示时间的LBL
        public UISprite[] iconSpr;//技能图片
        public UISprite[] sliderSpr;//倒计时遮板



        protected override void Awake()
        {
            base.Awake();
            Logicer.LeanSkillTypeID(5);
            EventManager.Register<EventChosenSkill>(OnEventChonseSprite);
            EventManager.Register<EventChosenOneSkill>(OnEventChonseOneSprite);
            EventManager.Register<EventToDeathOrReborn>(OnEventToDeath);



        }

        protected override void Start()
        {

            ListenOnClick(backBtn, OnClickBackLogin);
            ListenOnClick(imageSpr.gameObject, OnClickImage);
            ListenOnClick(lineBtn, OnClickSelectLine);
            ListenOnClick(listBtn, OnClickListBtn);

            ListenOnClick(backPackBtn, OnClickBackPack);
            ListenOnClick(skillWndBtn, OnClickSkillWnd);

            ListenOnClick(banghuiBtn, OnClickBangHui);
            ListenOnClick(jiayuanBtn, OnClickJiaYuan);
            ListenOnClick(maoxianBtn, OnClickMaoXian);
            ListenOnClick(mijingBtn, OnClickMiJing);

            ListenOnClick(tuozhanBtn, OnClickTuozhan);


//             ListenOnPress(leftBtn, OnPressLeftRight);
//             ListenOnPress(rightBtn, OnPressLeftRight);

            ListenOnClick(jobBtn,OnClickJobBtn);




            foreach( GameObject go in skillArray )
            {
                ListenOnClick(go, OnClickSkill);
            }

            U3DMod.SetActive(imageTopSpr.gameObject, false);
            U3DMod.SetActive(imageBottomSpr.gameObject, false);

        }
     
        public void SetImageTopShow( bool isshow )
        {
            U3DMod.SetActive(imageTopSpr.gameObject, isshow);
        }

        public void SetImageBottomShow(bool isshow)
        {
            U3DMod.SetActive(imageBottomSpr.gameObject, isshow);
        }

    /// <summary>
    /// 接受技能信息
    /// </summary>
    /// <param name="e"></param>
        void OnEventChonseSprite(EventChosenSkill e)
        {
            if (e.chonseSkill != null)
            {
                chonseSkillDic.Clear();
                ResetSprite();
                foreach (var item in e.chonseSkill)
                {
                    SetSkillIcon(item.Value.IconPos, item.Value.IconName, item.Value.LeftTime, item.Value.CoolingTime);
                    //SetSkillIcon(item.Value.IconPos, item.Value.IconName, 5, 5);

                    chonseSkillDic.Add(item.Value.IconPos, (float)item.Value.LeftTime);
                    //chonseSkillDic.Add(item.Key, 5);

                }
            }
        }


        /// <summary>
        /// 接受释放的哪个技能
        /// </summary>
        /// <param name="e"></param>
        void OnEventChonseOneSprite(EventChosenOneSkill e)
        { 
            if(e.chonseoneSkill != null)
            {
                foreach (var item in e.chonseoneSkill)
                {
                   leftTimeArr[item.Value.IconPos] = (float)item.Value.LeftTime;
                   isOK[item.Value.IconPos] = true;
                }
            }
        }

        /// <summary>
        /// 死亡事件或者复活
        /// </summary>
        /// <param name="d"></param>
        void OnEventToDeath(EventToDeathOrReborn d)
        {
            if (d.stage.Equals("ToDeath"))
            {
                for (int i = 0; i < IsActive.Length; i++)
                {
                    IsActive[i] = false;
                    skillArray[i].GetComponent<UIButtonColor>().enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < IsActive.Length; i++)
                {
                    IsActive[i] = true;
                    skillArray[i].GetComponent<UIButtonColor>().enabled = true;
                }
            }
            
        }

        void Update()
        {
            if (isOK[0])
            {
                StartNewSkill(0);
                
            }
            if (isOK[1])
            {
                StartNewSkill(1);
               
            }
            if (isOK[2])
            {
                StartNewSkill(2);
         
            }
            if (isOK[3])
            {
                StartNewSkill(3);
    
            } 
            if (isOK[4])
            {
                StartNewSkill(4);
            }
        }

        /// <summary>
        /// 技能的数据 现实的处理
        /// </summary>
        /// <param name="index"></param>
        void StartNewSkill(int index)
        {
            IsActive[index] = false;
            skillArray[index].GetComponent<UIButtonColor>().enabled = false;
           // Transform leftTimeLbl = skillArray[index].transform.FindChild("leftTimeLbl");
            //UILabel sprLbl = U3DMod.GetComponent<UILabel>(leftTimeLbl.gameObject);
            //timeLbl[index].text = (leftTimeArr[index]).ToString("0.00");

            leftTimeArr[index] -= Time.deltaTime;

           // Transform slider = skillArray[index].transform.FindChild("Slider");
           // UISprite spr = U3DMod.GetComponent<UISprite>(slider.gameObject);
            
            sliderSpr[index].fillAmount = (float)(leftTimeArr[index] / maxTimeArr[index]);


            //fillAmount 判断冷却是否完成
            if (sliderSpr[index].fillAmount < 0.0001)
            {
                sliderSpr[index].fillAmount = 1;
                //timeLbl[index].text = null;
                isOK[index] = false;
                skillArray[index].GetComponent<UIButtonColor>().enabled = true;
                IsActive[index] = true;
            }
        }

        /// <summary>
        /// 得到技能的位置 技能图片的名字 最大冷却时间 剩余冷却时间
        /// </summary>
        /// <param name="index"></param>
        /// <param name="spritename"></param>
        /// <param name="leftTime"></param>
        /// <param name="maxTime"></param>
        void SetSkillIcon(int index, string spritename, double leftTime = 0, double maxTime = 0)
        {

            //Transform icon = skillArray[index].transform.FindChild("Icon");            
            if (iconSpr[index] != null)
            {
               // UISprite spr = U3DMod.GetComponent<UISprite>(icon.gameObject);
                iconSpr[index].spriteName = spritename;
            }
            leftTimeArr[index] = (float)leftTime;
            //Transform slider = skillArray[index].transform.FindChild("Slider");
            if (sliderSpr[index] != null)
            {
                //UISprite spr = U3DMod.GetComponent<UISprite>(slider.gameObject);
                sliderSpr[index].fillAmount = 1;
            }

            maxTimeArr[index] = (float)maxTime;
        }

        /// <summary>
        /// 图片的名字不为技能图标的名字
        /// </summary>
        void ResetSprite()
        {
            foreach (var item in skillArray)
            {
                 Transform icon = item.transform.FindChild("Icon");
                 UISprite spr = U3DMod.GetComponent<UISprite>(icon.gameObject);
                spr.spriteName = "hahah";
            }
        }

        public void Init( ESceneType type )
        {
            U3DMod.SetActive(options, type== ESceneType.MainCity);
            U3DMod.SetActive(lineBtn, type == ESceneType.MainCity);

            U3DMod.SetActive(tuozhanBtn, type == ESceneType.Tower);
            U3DMod.SetActive(zhandou, type != ESceneType.MainCity);

            //U3DMod.SetActive(controls, type == ESceneType.Tower);
        }

        bool pressing = false;
        bool isleft = false;
        void OnPressLeftRight(GameObject go, bool state)
        {
            pressing = state;
//            isleft = leftBtn == go;
            if(state)
                StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (pressing)
            {
                if ( MoveHandler != null)
                    MoveHandler(isleft, pressing);
                yield return 0;
            }
            if (MoveHandler != null)
                MoveHandler(isleft, false);
        }


        void OnClickBackLogin(GameObject obj)
        {
            //Logicer.ChangeScene(101);
           Logicer.ReturnToLogin();
        }
        void OnClickJobBtn(GameObject obj) 
        {
            if (U3DMod.isActive(jobList))
            {
                TweenPosition t = jobList.transform.GetComponentInChildren<TweenPosition>();
                t.PlayReverse();
                Invoke("HIdeJobBtn", 0.03f);
            }
            else
            {
                U3DMod.SetActive(jobList, true);
                TweenPosition t = jobList.transform.GetComponentInChildren<TweenPosition>();
                t.PlayForward();
            }
        }
        void HIdeJobBtn() 
        {
            U3DMod.SetActive(jobList, false);
        }
        void OnClickImage(GameObject obj)
        {
            if (OnOpenCharDetailHandler != null)
                OnOpenCharDetailHandler();
        }

        void OnClickSelectLine(GameObject obj)
        {
            if (OnOpenSelectLineHandler != null)
                OnOpenSelectLineHandler();
        }

        void OnClickBangHui(GameObject obj)
        {

        }

        void OnClickJiaYuan(GameObject obj)
        {

        }

        void OnClickMaoXian(GameObject obj)
        {
           // Logicer.ChangeScene(102);
            UIManager.Instance.ShowWindow<UIWndSelectLevel>();
        }

        void OnClickMiJing(GameObject obj)
        {
            Logicer.ChangeScene(1001);
        }

        void OnClickTuozhan( GameObject obj )
        {
            if (LeaveBattleHandler != null)
                LeaveBattleHandler();
        }

        void OnClickSkill( GameObject obj )
        {
            for( int i=0;i<skillArray.Length;++i )
            {
                if(skillArray[i] == obj)
                {
                    if (IsActive[i])
                    {
                        Logicer.UseSkill(i);
                    }
                }
            }
        }
            

        void OnClickBackPack( GameObject obj )
        {
            //创建背包页面
            UIWndBackPack backPack = UIManager.Instance.ShowWindow<UIWndBackPack>();
        }

        void OnClickSkillWnd( GameObject obj )
        {
           UIWndSkill skwnd =  UIManager.Instance.ShowWindow<UIWndSkill>();

        }

        void OnClickListBtn(GameObject obj) 
        {
            if ( U3DMod.isActive(listBtnObj))
            {
                TweenPosition[] tweens  =  listBtnObj.transform.GetComponentsInChildren<TweenPosition>();
                for (int i = 0; i < tweens.Length; i++)
                {
                    tweens[i].PlayReverse();
                }
                Invoke("HideBtn",0.11f);
                listBtn.transform.localScale = new Vector3(1, 1, 1);
            }
            else 
            {
                U3DMod.SetActive(listBtnObj, true);
                TweenPosition[] tweens1 = listBtnObj.transform.GetComponentsInChildren<TweenPosition>();
                for (int i = 0; i < tweens1.Length; i++)
                {
                    tweens1[i].PlayForward();
                }
                listBtn.transform.localScale = new Vector3(1, -1, 1);
            }
        }
        void HideBtn() 
        {
            //Hide();
            U3DMod.SetActive(listBtnObj,false);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.Remove<EventChosenSkill>(OnEventChonseSprite);
        }
    }
}