using UnityEngine;
using System.Collections;

namespace SPSGame.Unity
{

    public class UIWndEquipmentPlay : UIWndBase
    {
        public GameObject backBtn;
        public GameObject StrengthenBtn;
        public GameObject StrengthenObj;
        public GameObject SpecialSkillBtn;
        public GameObject SpecialSkillObj;
        public GameObject GemBtn;
        public GameObject GemObj;
        public GameObject ImageIdentifyBtn;
        public GameObject ImageIdentifyObj;

        public GameObject StrengthBtn_01;
        public UISlider stengthSlider;
        public UILabel stenthAfterLab_01;
        public UILabel stenthAfterLab_02;
        public UILabel stenthAfterLab_03;

        public UILabel stenthBeforeLab_01;
        public UILabel stenthBeforeLab_02;
        public UILabel stenthBeforeLab_03;

        public UILabel consumeLab;
        public UILabel allLab;

        public UILabel stenthLevel;
        public UILabel testLevel;

        public UILabel titleLbl;

        public GameObject equipment1;
        public GameObject compound;//合成

        public GameObject btnCompoundNeed;//合成需求按纽
        public GameObject btnCompound;//合成按钮
        public GameObject btnAdvanceEquipment;//前期装备按纽

        public GameObject compoundNeedContent;//合成需求的内容
        public GameObject advanceEquipmentContent;//前期装备内容
        public GameObject noAdvance;//

        public GameObject pathway1;
        public GameObject pathway2;
        public GameObject pathway3;


        protected override void Awake()
        {

            base.Awake();
            ListenOnClick(backBtn, OnClickClose);
            ListenOnClick(StrengthenBtn, OnClickStrengthen);
            ListenOnClick(SpecialSkillBtn, OnClickSkill);
            ListenOnClick(GemBtn, OnClickGem);
            ListenOnClick(ImageIdentifyBtn, OnClickImageIdentify);
            ListenOnClick(StrengthBtn_01, OnClickStrenthEffect);

            ListenOnClick(equipment1, OnClickEquipment1);
            ListenOnClick(btnCompoundNeed, OnClickBtnCompoundNeed);
            ListenOnClick(btnCompound, OnClickBtnCompound);
            ListenOnClick(btnAdvanceEquipment, OnClickBtnAdvanceEquipment);

            ListenOnClick(pathway1, OnClickpathway1);
            ListenOnClick(pathway2, OnClickpathway2);
            ListenOnClick(pathway3, OnClickpathway3);


        }

        /// <summary>
        /// 第一个途径
        /// </summary>
        /// <param name="obj"></param>
        void OnClickpathway1(GameObject obj)
        { 
            
        }

        /// <summary>
        /// 第二个途径
        /// </summary>
        /// <param name="obj"></param>
        void OnClickpathway2(GameObject obj)
        {

        }

        /// <summary>
        /// 第三个途径
        /// </summary>
        /// <param name="obj"></param>
        void OnClickpathway3(GameObject obj)
        {

        }

        /// <summary>
        /// 点击合成按钮
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBtnCompound(GameObject obj)
        {

        }



        /// <summary>
        ///点击合成需求按钮 
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBtnCompoundNeed(GameObject obj)
        {
            //NotActivate();
            //U3DMod.SetActive(compound, true);
            U3DMod.SetActive(compoundNeedContent, true);
            U3DMod.SetActive(advanceEquipmentContent, false);
        }

        /// <summary>
        /// 点击进阶装备按纽
        /// </summary>
        /// <param name="obj"></param>
        void OnClickBtnAdvanceEquipment(GameObject obj)
        {
            U3DMod.SetActive(compoundNeedContent, false);
            U3DMod.SetActive(advanceEquipmentContent, true);
        }
        /// <summary>
        /// 强化
        /// </summary>
        /// <param name="obj"></param>
        void OnClickStrengthen(GameObject obj)
        {
            titleLbl.text = "强化";

            NotActivate();
            U3DMod.SetActive(StrengthenObj, true);
            BtnColor();
            BtnScale();
         
        }
        /// <summary>
        /// 特技
        /// </summary>
        /// <param name="obj"></param>
        void OnClickSkill(GameObject obj) 
        {
            titleLbl.text = "特技";

            NotActivate();
            U3DMod.SetActive(SpecialSkillObj, true);
            BtnColor();
            BtnScale();
        }
        /// <summary>
        /// 宝石
        /// </summary>
        /// <param name="obj"></param>
        void OnClickGem(GameObject obj)
        {
            titleLbl.text = "宝石";
            NotActivate();
            U3DMod.SetActive(GemObj, true);
            BtnColor();
            BtnScale();
        }
        /// <summary>
        /// 图鉴
        /// </summary>
        /// <param name="obj"></param>
        void OnClickImageIdentify(GameObject obj)
        {
            titleLbl.text = "图鉴";
            NotActivate();
            U3DMod.SetActive(ImageIdentifyObj, true);
            BtnColor();
            BtnScale();
        }

        /// <summary>
        /// 点击某一个装备 显示装备的合成页面
        /// </summary>
        /// <param name="obj"></param>
        void OnClickEquipment1(GameObject obj)
        {
            NotActivate();
            U3DMod.SetActive(compound, true);
            U3DMod.SetActive(advanceEquipmentContent, false);
            titleLbl.text = "合成";
        }
        /// <summary>
        /// 点击强化页面下的强化按钮
        /// </summary>
        /// <param name="obj"></param>
        void OnClickStrenthEffect(GameObject obj) 
        {
           float consume = float.Parse(consumeLab.text);
           float allprice = float.Parse(allLab.text);
           float befor_01 = float.Parse(stenthAfterLab_01.text);
           float befor_02 = float.Parse(stenthAfterLab_02.text);
           float befor_03 = float.Parse(stenthAfterLab_03.text);
           if (float.Parse(stenthLevel.text) > (float.Parse(testLevel.text)))
           {
               Debug.Log("超过玩家等级");
           }
           else 
           {
               if (consume <= allprice)
               {
                   float temp = allprice - consume;
                   SetPriceLab((consume * 1.2f).ToString(), temp.ToString());
                   stengthSlider.value += 0.3f;

                   if (stengthSlider.value >= 1)
                   {
                       Debug.Log("升级");
                       SetStrenthedProperty((befor_01 * 1.1).ToString(), (befor_02 * 1.1).ToString(), (befor_03 * 1.1).ToString());
                       stenthLevel.text = (float.Parse(stenthLevel.text) + 1f).ToString();
                       stengthSlider.value = stengthSlider.value - 1;
                   }
               }
               else
               {
                   consumeLab.color = Color.red;
                   Debug.Log("金币不足");
               }
           }
        }


        /// <summary>
        /// 设置装备强化后的属性
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
         public  void SetStrenthedProperty(string str1,string str2,string str3)
        {
            stenthAfterLab_01.text = str1;
            stenthAfterLab_02.text = str2;
            stenthAfterLab_03.text = str3;  
        }
        /// <summary>
        /// 设置装备强化之前的属性
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
         public void SetBeforeStrenthedProperty(string str1, string str2, string str3)
         {
             stenthBeforeLab_01.text = str1;
             stenthBeforeLab_02.text = str2;
             stenthBeforeLab_03.text = str3;
         }
        /// <summary>
        /// 设置强化装备消耗的金币数
        /// </summary>
        public void SetPriceLab(string consume ,string allPrice) 
         {
             consumeLab.text = consume;
             allLab.text = allPrice;
         }
        /// <summary>
        /// 界面全部为不激活状态
        /// </summary>
        void NotActivate() 
        {
            U3DMod.SetActive(StrengthenObj,false);
            U3DMod.SetActive(SpecialSkillObj, false);
            U3DMod.SetActive(GemObj, false);
            U3DMod.SetActive(ImageIdentifyObj, false);
            U3DMod.SetActive(compound, false);
        }

      
        /// <summary>
        /// 选中时的按钮的颜色变化
        /// </summary>
        void BtnColor() 
        {
            if (U3DMod.isActive(StrengthenObj))
            {
                StrengthenBtn.GetComponent<UISprite>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                SpecialSkillBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                GemBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ImageIdentifyBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);

            }
            if (U3DMod.isActive(SpecialSkillObj))
            {
                SpecialSkillBtn.GetComponent<UISprite>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                StrengthenBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                GemBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ImageIdentifyBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            }
            if (U3DMod.isActive(GemObj))
            {
                GemBtn.GetComponent<UISprite>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                SpecialSkillBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                StrengthenBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                ImageIdentifyBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            }
            if (U3DMod.isActive(ImageIdentifyObj))
            {
                ImageIdentifyBtn.GetComponent<UISprite>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                SpecialSkillBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                GemBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
                StrengthenBtn.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            }
        }
        /// <summary>
        /// 改变大小
        /// </summary>
        void BtnScale() 
        {
            if (U3DMod.isActive(StrengthenObj)) 
            {
                StrengthenBtn.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
                SpecialSkillBtn.transform.localScale = new Vector3(1,1,1);
                GemBtn.transform.localScale = new Vector3(1, 1, 1);
                ImageIdentifyBtn.transform.localScale = new Vector3(1, 1, 1);
            }
            if (U3DMod.isActive(SpecialSkillObj)) 
            {
                StrengthenBtn.transform.localScale = new Vector3(1f, 1f, 1f);
                SpecialSkillBtn.transform.localScale = new Vector3(1.1f, 1.1f,1.1f);
                GemBtn.transform.localScale = new Vector3(1, 1, 1);
                ImageIdentifyBtn.transform.localScale = new Vector3(1, 1, 1);
            }
            if (U3DMod.isActive(GemObj)) 
            {
                StrengthenBtn.transform.localScale = new Vector3(1f, 1f, 1f);
                SpecialSkillBtn.transform.localScale = new Vector3(1, 1, 1);
                GemBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                ImageIdentifyBtn.transform.localScale = new Vector3(1, 1, 1);
            }
            if (U3DMod.isActive(ImageIdentifyObj)) 
            {
                StrengthenBtn.transform.localScale = new Vector3(1f, 1f, 1f);
                SpecialSkillBtn.transform.localScale = new Vector3(1, 1, 1);
                GemBtn.transform.localScale = new Vector3(1, 1, 1);
                ImageIdentifyBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
        }
    }
}