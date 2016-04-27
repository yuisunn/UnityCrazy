using UnityEngine;
using System.Collections;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class UIWndRoleFrame : UIWndBase
    {

        public GameObject backBtn;

        public UISprite[] Images;
        public UILabel[] Infos;

        public DataViewDelegateByID ViewDataHandler = null;

        public GameObject[] Selicts;
        protected override void Awake()
        {
            base.Awake();
            ListenOnClick(backBtn, OnClickBack);
            foreach( UISprite spr in Images )
            {
                ListenOnClick(spr.gameObject, OnClickImage);
                ListenOnPress(spr.gameObject,OnPress);
            }
        }

        void OnClickBack( GameObject obj )
        {
            Logicer.ReturnToLogin();
        }

        void OnClickImage(GameObject obj)
        {
            for (int index = 0; index < Images.Length; ++index)
            {
                if (obj == Images[index].gameObject)
                {
                    ViewDataHandler(index + 1);
                }
            }
        }
        /// <summary>
        /// 监听按下事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isOnpress"></param>
        void OnPress(GameObject obj,bool isOnpress) 
        {
            for (int index = 0; index < Images.Length; ++index)
            {
                if (obj == Images[index].gameObject && isOnpress)
                {
                    U3DMod.SetActive(Selicts[index], true);
                }
                if (obj == Images[index].gameObject && !isOnpress)
                {
                    U3DMod.SetActive(Selicts[index], false);
                }
            }
        }

        /// <summary>
        /// 设置角色显示信息
        /// </summary>
        /// <param name="roleid">角色id：1~6</param>
        /// <param name="name">角色名字</param>
        /// <param name="level">角色等级（为-1时不显示）</param>
        /// <param name="grade">角色阶段（1,2,3阶段）</param>
        /// <returns></returns>
        public void SetInfo( int roleid,string name,int level,int grade )
        {
            int index = roleid - 1;
            Infos[index].text = name + (level==-1?"":("\nLV" + level));
            Infos[index].color = level == -1 ? Color.white : Color.yellow;

            string spriteheadname = "";

            switch (index)
            {
                case 0:
                    spriteheadname = "chuanzhang";
                    break;
                case 1:
                    spriteheadname = "qishi";
                    break;
                case 2:
                    spriteheadname = "jiansheng";
                    break;
                case 3:
                    spriteheadname = "xiaohei";
                    break;
                case 4:
                    spriteheadname = "huonv";
                    break;
                case 5:
                    spriteheadname = "bingnv";
                    break;
                default:
                    DebugMod.Log("can't find roleid: " + roleid);
                    break;
            }

            Images[index].spriteName = spriteheadname + grade;        
            Images[index].MakePixelPerfect();
        }

    }
}