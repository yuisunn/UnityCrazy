using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Unity;
using System.Linq;
using SPSGame;
public class UIWndSkill : UIWndBase
{
    public GameObject btnFirstCareer = null;   //Skill1按纽
    public GameObject btnSecondCareer = null;   //Skill2按纽 
    public GameObject btnThridCareer = null;   //Skill3按纽
    public GameObject btnEquipmentSkill = null;   //Skill3按纽

    public GameObject firstCareerPanel = null;//Skill1的技能面板
    public GameObject secondCareerPanel = null;//Skill2的技能面板
    public GameObject thridCareerPanel = null;//Skill3的技能面板
    public GameObject equipmentSkillPanel = null;//Skill3的技能面板

    public GameObject BGScrollview = null;//滑动背景面板

    public GameObject btnUpGrade = null;//升级按钮
    public GameObject btnStudy = null;//升级按钮
    public GameObject btnBack = null;//退出按钮

    

    public UILabel skillNameLbl = null;//技能一的名字
    public UILabel studyLbl = null;//学习 OR 等级  
    public UILabel effect1Lbl = null;//等级效果
    public UILabel effect2Lbl = null;
    public UILabel codeTimeLbl = null;//冷却时间
    public UILabel consumLbl = null;//消耗
    public UILabel describeLbl = null;//技能的描述

    public UILabel upGradeNormLbl = null;//升级标准
    public UILabel upGradeEffect1Lbl = null;//升级后的效果
    public UILabel upGradeEffect2Lbl = null;
    public UILabel upGradeconsumLbl = null;//要升级，所需要的消耗

    public UILabel firstCareerName = null;//一阶职业名称
    public UILabel secondCareerName = null;
    public UILabel thridCareerName = null;

    public GameObject[] skillIcon = null;//技能按纽

    public GameObject[] shortSkillIcon = null;//快捷技能按纽

    public UILabel[] skillIconNameLbl = null;//技能名称

    public GameObject[] skillMask = null;//快捷技能遮挡名称



    public List<LocalDataToC> mskillDic = new List<LocalDataToC>();

    public bool _misLearned = false;
    public int mid = -1;
    //private int _mskillLevel = 1;

    public Dictionary<int,bool> conditionDic = new Dictionary<int,bool>() ;
    public Dictionary<int, int> shortSkillDic = new Dictionary<int,int>();



    public delegate void SubmitSkillDelegate( Dictionary<int,string> dic );
    public SubmitSkillDelegate SubmitSkillHandler = null;

   // public MessageBoxResultDelegate callback = null;

    protected override void Awake()
    {

        base.Awake();

        Logicer.LeanSkillTypeID(1);

        EventManager.Register<EventLeanSkill>(OnEventSkillSprite);
        EventManager.Register<EventStudySkill>(OnEventStudySkillSprite);
        EventManager.Register<EventUpSkill>(OnEventUpSkillSprite);

        U3DMod.SetActive(firstCareerPanel, true);
        U3DMod.SetActive(secondCareerPanel, false);
        U3DMod.SetActive(thridCareerPanel, false);
        U3DMod.SetActive(equipmentSkillPanel, false);

        ListenOnClick(btnFirstCareer, OnClickFirstCareer);
        ListenOnClick(btnSecondCareer, OnClickSecondCareer);
        ListenOnClick(btnThridCareer, OnClickThridCareer);
        ListenOnClick(btnEquipmentSkill, OnClickEquipmentSkill);

        ListenOnClick(skillIcon[0], OnClickSkillIcon);
        ListenOnClick(skillIcon[1], OnClickSkillIcon);
        ListenOnClick(skillIcon[2], OnClickSkillIcon);
        ListenOnClick(skillIcon[3], OnClickSkillIcon);
        ListenOnClick(skillIcon[4], OnClickSkillIcon);

        ListenOnPress(skillIcon[0], DragSkillIcon);
        ListenOnPress(skillIcon[1], DragSkillIcon);
        ListenOnPress(skillIcon[2], DragSkillIcon);
        ListenOnPress(skillIcon[3], DragSkillIcon);
        ListenOnPress(skillIcon[4], DragSkillIcon);
        ListenOnDragEnd(skillIcon[0],DragEndSkillIcon);

        ListenOnClick(btnUpGrade, OnClickBtnUpGrade);
        ListenOnClick(btnStudy, OnClickBtnStudy);
        ListenOnClick(btnBack, OnBtnHide);

        //callback = callbackHandler;
    }

    void DragEndSkillIcon(GameObject obj)
    { 
        
    }




    public int pos1 = 1;
    void DragSkillIcon(GameObject o,bool s = true)
    {
        if (o == skillIcon[0])
        {
            pos1 = 0;
            ShowCareerDes(charst, pos1);
        }


        if (o == skillIcon[1])
        {
            pos1 = 1;
            ShowCareerDes(charst, pos1);
        }

        if (o == skillIcon[2])
        {
            pos1 = 2;
            ShowCareerDes(charst, pos1);
        }
        if (o == skillIcon[3])
        {
            pos1 = 3;
            ShowCareerDes(charst, pos1);
        }

        if (o == skillIcon[4])
        {
            pos1 = 4;
            ShowCareerDes(charst, pos1);
        }

    }

    /// <summary>
    /// 接受技能信息
    /// </summary>
    /// <param name="e"></param>
    void OnEventSkillSprite(EventLeanSkill e)
    {
        charst = 1;
        mskillDic = e.listSkill;
        if (e.dicSkill.Count != 0)
        {
            foreach (var i in e.dicSkill)
            {
                shortSkillDic.Add(i.Key, i.Value);
            }
            //shortSkillDic = e.dicSkill;
        }
        string[] careerName = mskillDic[0].CharStageName.Split(':');
        Career(careerName[0], careerName[1], careerName[2]);
        ShowSkillDes(1);
        ShowCareerDes(1,0);
        SpriteHeigh();
        if (shortSkillDic != null)
        {
            foreach (var item in shortSkillDic)
            {
                if (item.Key != 0)
                {
                    foreach (var item1 in mskillDic)
                    {
                        if (item1.SkillID == item.Value)
                        {
                            shortSkillIcon[item.Key - 1].GetComponent<UISprite>().spriteName = item1.SkillIconID;
                        }
                    }
                }
            }
            //if (!shortSkillDic.Keys.Contains(0))
            //{
            //    shortSkillDic.Add(0, GetCareerAndPos(1, 0).SkillID);
            //}
        }


    }

    void SpriteHeigh()
    {
        for (int i = 0; i < mskillDic.Count; i++)
        {
            if (1 == mskillDic[i].CharStage && mskillDic[i].IsLearned == true)
            {
                skillIcon[i].GetComponent<UISprite>().color = Color.white;
                AppaerBtn(true);
                skillIcon[i].GetComponent<DragSkillSprite>().enabled = true;
                skillIcon[i].GetComponent<UIButtonColor>().pressed = Color.gray;
                skillMask[i].SetActive(false);
               
            }
            else
            {
                skillIcon[i].GetComponent<UISprite>().color = Color.white;
                skillIcon[i].GetComponent<DragSkillSprite>().enabled = false;
                skillIcon[i].GetComponent<UIButtonColor>().pressed = Color.gray;

                skillMask[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 技能面板
    /// </summary>
    /// <param name="charStage">角色阶位</param>
    void ShowSkillDes(int charStage)
    {
        for (int i = 0; i < skillIcon.Length; i++)
        {
            foreach (var item1 in mskillDic)
            {
                if (0 == item1.SkillIconPos || (charStage == item1.CharStage && item1.SkillIconPos == i))
                {
                    skillIcon[i].GetComponent<UISprite>().color = Color.gray;
                    skillIcon[i].GetComponent<UISprite>().spriteName = item1.SkillIconID;
                    skillIconNameLbl[i].text = item1.SkillName;
                }
            }
        }
    }

    LocalDataToC itemLoc = null;
    /// <summary>
    /// 点击面板的描述
    /// </summary>
    /// <param name="careerStage">点击的是哪个职业面板</param>
    void ShowCareerDes(int careerStage , int IconPos)
    {
        if ((itemLoc = GetCareerAndPos(careerStage, IconPos)) != null)
        {
            AppearShowPro(itemLoc);
            AppearShowUp(itemLoc);
        }
        else
        {
            Debug.LogError("No Such obj");
        }
    }

    public LocalDataToC GetCareerAndPos(int stage ,int pos)
    {
        foreach (var item in mskillDic)
        {
            if (stage == item.CharStage && pos == item.SkillIconPos)
            {
                return item;
            }
        }
        return null;
    }


    /// <summary>
    /// 学习技能
    /// </summary>
    /// <param name="s"></param>
    void OnEventStudySkillSprite(EventStudySkill s)
    {
        if(0 == s.studySkill)
        {
            if (s.idSkill == itemLoc.SkillID)
            {
                itemLoc.IsLearned = true;
                ColorHeigh(skillIcon[pos], true);
                this.studyLbl.text = itemLoc.SkillLevel.ToString();
                AppaerBtn(itemLoc.IsLearned);
                skillIcon[pos].gameObject.GetComponent<DragSkillSprite>().enabled = true;
                if(0 == pos)
                {
                    shortSkillDic.Add(0, GetCareerAndPos(1, 0).SkillID);
                }
                skillMask[pos].SetActive(false);
            }

        }
        else if (2 == s.studySkill)
        {
            UIManager.MsgBox("提示信息", "金币不满足", MsgStyle.Yes, null);//目前只有金币 以后要改正
        }
        else if(1 == s.studySkill)
        {
            UIManager.MsgBox("提示信息", "学习条件不满足", MsgStyle.Yes, null);
        }

    }

    /// <summary>
    /// 升级技能信息
    /// </summary>
    /// <param name="e"></param>

    void OnEventUpSkillSprite(EventUpSkill e)
    {
        //List<mSkillDynamicArgWithIcon> mskill = mskillDic.Values.First();

        //for (int i = 0; i < mskill.Count; i++)
        //{
        //    if (e.dicUpSkill.SkillID == mskill[i].SkillID)
        //    {
        //        mskill[i] = e.dicUpSkill;
        //        AppearShowPro(i);
        //        AppearShowUp(i);
        //        break;
        //    }
        //}
        if (itemLoc.SkillID == e.dicUpSkill.SkillID)
        {
            if (0 == e.errStu)
            {
                UpDataDate(e.dicUpSkill);
                AppearShowPro(itemLoc);
                AppearShowUp(itemLoc);

            }
            else if (2 == e.errStu)
            {
                UIManager.MsgBox("提示信息", "金币不满足", MsgStyle.Yes, null);//目前只有金币 以后要改正
            }
            else if (1 == e.errStu)
            {
                UIManager.MsgBox("提示信息", "升级条件不满足", MsgStyle.Yes, null);
            }
        }

    }


    void UpDataDate(LocalDataToC data)
    {

        itemLoc.PhyHurt = data.PhyHurt;
        itemLoc.IncAttacSpead = data.IncAttacSpead;
        itemLoc.LevelUpCond = (short)data.LevelUpCond;
        itemLoc.ConsumeMoney = data.ConsumeMoney;
        itemLoc.LevelUpEffect = "升级效果";
        itemLoc.LevelUpMsg1 = data.LevelUpMsg1;
        itemLoc.LevelUpMsg2 = data.LevelUpMsg2;
        itemLoc.LevelUpMsg3 = data.LevelUpMsg3;
        itemLoc.SkillLevel = data.SkillLevel;
    }

    /// <summary>
    /// 显示属性
    /// </summary>
    /// <param name="i"></param>
    void AppearShowPro(LocalDataToC localdata)
    {
        string _skillname = localdata.SkillName;
        string _phyhurt = ("0" == localdata.PhyHurt) ? "物理攻击： 0" : localdata.PhyHurt;
        string _incattacspead = ("0" == localdata.IncAttacSpead) ? "攻击速度： 0" : localdata.IncAttacSpead;
        string _skilldescrib = localdata.SkillDescrib;
        float _consumeenergy = localdata.ConsumeEnergy;
        double _coolingtime = localdata.CoolingTime;
        _misLearned = localdata.IsLearned;
        int _skilllevel = localdata.SkillLevel;
        ShowProperty(_skillname, _skilllevel, _phyhurt, _incattacspead, _coolingtime, _consumeenergy, _skilldescrib, _misLearned);
        mid = (int)localdata.SkillID;
    }

    /// <summary>
    /// 升级属性显示
    /// </summary>
    /// <param name="i"></param>
    void AppearShowUp(LocalDataToC localdata)
    {
        int _levelUpCond = localdata.LevelUpCond;
        string _levelUpMsg1 = localdata.LevelUpMsg1;
        string _levelUpMsg2 = localdata.LevelUpMsg2;
        //string _levelUpMsg3 = mskill[i].LevelUpMsg3;
        int _consumeMoney = localdata.ConsumeMoney;
        ShowUpGradeValue(_levelUpCond, _levelUpMsg1, _levelUpMsg2, _consumeMoney);
    }


    int charst = 0;
    /// <summary>
    /// 点击firstCareer按纽时的处理
    /// </summary>
    /// <param name="o"></param>
    void OnClickFirstCareer(GameObject obj)
    {
        charst = 1;
        CareerPanelShow();
        U3DMod.SetActive(firstCareerPanel, true);
        ShowCareerDes(1,0);
        CareerColor();
        ColorHeigh(obj, false);
    }

    /// <summary>
    /// 点击Skill2按纽时的处理
    /// </summary>
    /// <param name="o">普通图标显示</param>
    void OnClickSecondCareer(GameObject o)
    {
        charst = 2;

        CareerPanelShow();
        U3DMod.SetActive(secondCareerPanel, true);
        CareerColor();
        ColorHeigh(o, false);
    }

    /// <summary>
    /// 点击Skill3按纽时的处理
    /// </summary>
    /// <param name="o"></param>
    void OnClickThridCareer(GameObject o)
    {
        charst = 3;

        CareerPanelShow();
        U3DMod.SetActive(thridCareerPanel, true);
        CareerColor();
        ColorHeigh(o, false);
    }

    /// <summary>
    ///点击Equipment按纽时的处理
    /// </summary>
    /// <param name="o"></param>
    void OnClickEquipmentSkill(GameObject o)
    {
        CareerPanelShow();
        U3DMod.SetActive(equipmentSkillPanel, true);
        CareerColor();
        ColorHeigh(o, false);

    }


    /// <summary>
    /// 技能面板的显示
    /// </summary>
    void CareerPanelShow()
    {
        U3DMod.SetActive(firstCareerPanel, false);
        U3DMod.SetActive(secondCareerPanel, false);
        U3DMod.SetActive(thridCareerPanel, false);
        U3DMod.SetActive(equipmentSkillPanel, false);
    }

    public int pos = 1;
    /// <summary>
    /// 点击技能图标时的处理
    /// </summary>
    /// <param name="o"></param>
    void OnClickSkillIcon(GameObject o)
    {
        //根据点击的那个按钮 执行那个操作
        if (o == skillIcon[0])
        {
            pos = 0;
            ShowCareerDes(charst, pos);
            AppaerBtn(_misLearned);
        }
        if (o == skillIcon[1])
        {
            pos = 1;
            ShowCareerDes(charst, pos);
            //AppearShowPro(1);
            AppaerBtn(_misLearned);
        }
        if (o == skillIcon[2])
        {
            pos = 2;

            ShowCareerDes(charst, pos);
           
            AppaerBtn(_misLearned);
        }
        if (o == skillIcon[3])
        {
            pos = 3;

            ShowCareerDes(charst, pos);
            
            AppaerBtn(_misLearned);
        }
        if (o == skillIcon[4])
        {
            pos = 4;
            ShowCareerDes(charst, pos);
            AppaerBtn(_misLearned);
        }

    }

    /// <summary>
    /// 升级按钮
    /// </summary>
    /// <param name="o"></param>
    void OnClickBtnUpGrade(GameObject o)
    {
        Logicer.StudyAndUpSkillTypeID(3, itemLoc.SkillID, itemLoc.charLevel, itemLoc.CharStage, itemLoc.ConsumeMoney);
    }

    /// <summary>
    /// 学习按纽
    /// </summary>
    /// <param name="o"></param>
    void OnClickBtnStudy(GameObject o)
    {
        Logicer.StudyAndUpSkillTypeID(2, itemLoc.SkillID, itemLoc.charLevel, itemLoc.CharStage, itemLoc.ConsumeMoney , itemLoc.CoolingTime, itemLoc.SkillIconID);
    }

    void AppaerBtn(bool hide)
    {
        if (hide)
        {
            U3DMod.SetActive(btnStudy, false);
            U3DMod.SetActive(btnUpGrade, true);
        }
        else
        {
            U3DMod.SetActive(btnStudy, true);
            U3DMod.SetActive(btnUpGrade, false);
        }
    }


    LocalDataToC GetSkillData(int skillid)
    {
        for( int i=0;i<mskillDic.Count;++i )
        {
            if (mskillDic[i].SkillID == skillid)
                return mskillDic[i];
        }
        return null;
    }

    /// <summary>
    /// 退出按钮
    /// </summary>
    /// <param name="o"></param>
    void OnBtnHide(GameObject o)
    {
        //UIManager.MsgBox("信息提示", "是否保存技能", MsgStyle.YesAndNo, callback);

        Logicer.ShortSkillDic(4, shortSkillDic);
        Logicer.LeanSkillTypeID(5);
        //if (SubmitSkillHandler != null)
        //{
        //    Dictionary<int, string> skillicondic = new Dictionary<int, string>();
        //    foreach (int key in shortSkillDic.Keys)
        //    {
        //        skillicondic[key] = GetSkillData(shortSkillDic[key]).SkillIconID;
        //    }
        //    SubmitSkillHandler(skillicondic);
        //}

        ShowSkillDes(1);
        ShowCareerDes(1, 0);
        SpriteHeigh();
        Hide();
        
    }

    //private void callbackHandler(MsgResult result)
    //{
    //    if (result == MsgResult.Yes)
    //    {
           
    //    }
    //    else
    //    {

            
    //    }
    //}






    /// <summary>
    /// 点击技能图标时 显示的技能的属性
    /// </summary>
    /// <param name="skillName"></param>
    /// <param name="study">等级/未学习</param>
    /// <param name="effect1">效果1</param>
    /// <param name="effect2">效果2</param>
    /// <param name="codeTime">冷却时间</param>
    /// <param name="consum">消耗能量</param>
    /// <param name="describe">描述</param>
    /// <param name="isStudy">是否已经学习</param>
    public void ShowProperty(string skillName, int study, string effect1, string effect2, double codeTime, float consum, string describe, bool isStudy)
    {
        this.skillNameLbl.text = skillName;
        //是否达到了条件
        this.studyLbl.text = (isStudy) ? study.ToString() : "未学习";

        this.effect1Lbl.text = effect1;
        this.effect2Lbl.text = effect2;
        this.codeTimeLbl.text = "冷却时间：" + codeTime.ToString();
        this.consumLbl.text ="消耗能量：" + consum.ToString();
        this.describeLbl.text = describe;
    }

    /// <summary>
    /// 点击技能图标时显示的升级条件等
    /// </summary>
    /// <param name="UpGradeNorm">达到标准的等级</param>
    /// <param name="upGradeEffect1">升级以后的效果1</param>
    /// <param name="upGradeEffect2">升级以后的效果2</param>
    /// <param name="upGradeConsum">实际所需的消耗</param>
    public void ShowUpGradeValue(float UpGradeNorm, string upGradeEffect1, string upGradeEffect2, int upGradeConsum)
    {

        this.upGradeNormLbl.text = "角色达到" + UpGradeNorm.ToString() + "级";
        this.upGradeEffect1Lbl.text = upGradeEffect1;
        this.upGradeEffect2Lbl.text = upGradeEffect2;
        this.upGradeconsumLbl.text = "消耗" + "金币" + " ：" + upGradeConsum; ;
    }

    /// <summary>
    /// 不同阶级的职业名称
    /// </summary>
    /// <param name="firstNameLbl">一阶职业的名称</param>
    /// <param name="secondNameLbl">二阶职业的名称</param>
    /// <param name="thridNameLbl">三阶职业的名称</param>
    public void Career(string firstNameLbl,string secondNameLbl,string thridNameLbl)
    {
        firstCareerName.text = firstNameLbl;
        secondCareerName.text = secondNameLbl;
        thridCareerName.text = thridNameLbl;
    }

    /// <summary>
    /// 颜色的显示
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isHeigh">判断是白亮色还是灰色</param>
    void ColorHeigh(GameObject go , bool isHeigh)
    {
        go.GetComponent<UISprite>().color = isHeigh ? Color.white : Color.gray;
    }


    void CareerColor()
    {
       
        ColorHeigh(btnFirstCareer,true);
        ColorHeigh(btnSecondCareer, true);
        ColorHeigh(btnThridCareer, true);
        ColorHeigh(btnEquipmentSkill, true);
    }


    /// <summary>
    /// 技能名字的显示
    /// </summary>
    /// <param name="list"></param>
    public void SkillName(List<mSkillDynamicArgWithIcon> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string _skillName = list[i].SkillIconID;
            string _skillNameLbl = list[i].SkillName;
            skillIcon[i].GetComponent<UISprite>().spriteName = _skillName;
            skillIconNameLbl[i].text = _skillNameLbl;
        }
    }

    protected override void OnDestroy()
    {
        shortSkillDic.Clear();
        EventManager.Remove<EventLeanSkill>(OnEventSkillSprite);
        EventManager.Remove<EventStudySkill>(OnEventStudySkillSprite);
        EventManager.Remove<EventUpSkill>(OnEventUpSkillSprite);
    }
}
