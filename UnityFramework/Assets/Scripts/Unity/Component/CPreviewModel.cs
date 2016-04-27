using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class CPreviewModel : U3DComponent
    {
        public VoidDelegate OnClickHandler;

        private CPreviewAnimator mAnim = null;

        private  Color modleOutlineColor;
        private  Color modleMainColor;

        private float _time;

        List<CEffect> mEffects = new List<CEffect>();

        Dictionary<string, Transform> mEffectHangDic = new Dictionary<string, Transform>();

        protected override void Start()
        {
            base.Start();

            EasyTouch.On_SimpleTap += OnModelClick;

            //EasyTouch.On_Drag += OnModelDrag;
            mAnim = U3DMod.GetComponent <CPreviewAnimator>(gameObject);
            //获取模型初始的颜色值
            modleOutlineColor = GetModleOutlineColor();
            modleMainColor = GetModleMainColor();
            if (modleMainColor != Color.black)
                SetAppear(true, .5f);
        }

        public void InitAllEffect(string rolename)
        {
            switch (rolename)
            {
                case "船长":
                    LoadEffect("cz_dl_zhx_.unity3d");
                    LoadEffect("cz_dl_zhx_bip001 prop2.unity3d");
                    LoadEffect("cz_dl_qs_bip001.unity3d");
                    LoadEffect("cz_dl_qs_bip001 l hand.unity3d");
                    LoadEffect("cz_dl_qs_bip001 r hand.unity3d");
                    break;
                case "火女":
                    LoadEffect("hv_dl_qs_.unity3d");
                    LoadEffect("hv_dl_zhx_.unity3d");
                    LoadEffect("hv_dl_zhx_bip001 l hand.unity3d");
                    LoadEffect("hv_dl_zhx_bip001 r hand.unity3d");
                    break;
                case "剑圣":
                    LoadEffect("js_dl_qs_.unity3d");
                    LoadEffect("js_dl_qs_bip001 l hand.unity3d");
                    LoadEffect("js_dl_qs_bip001 r hand.unity3d");
                    break;
                case "冰女":
                    LoadEffect("bv_dl_qs and zhx_bip001 footsteps.unity3d");
                    break;
                case "小黑":
                    LoadEffect("xh_dl_qs_.unity3d");
                    LoadEffect("xh_dl_qs_bip001 l hand.unity3d");
                    LoadEffect("xh_dl_qs_bip001 r hand.unity3d");
                    LoadEffect("xh_dl_zhx_.unity3d");
                    break;
                case "圣骑":
                    LoadEffect("qnqs_dl_qishen_.unity3d");
                    LoadEffect("qnqs_dl_zhanxunhuan_.unity3d");
                    break;
            }
        }

        void OnModelClick(Gesture ges)
        {
            if (ges.pickObject == gameObject)
            if(OnClickHandler != null)
            {
                OnClickHandler();
            }
        }

        void OnModelDragStart(Gesture ges)
        {
            if (ges.pickObject == gameObject)
            {

            }
        }

        void OnModelDrag(Gesture ges)
        {
            //暂时不用
            /*
            if (ges.pickObject == gameObject)
            {
                float distance = Vector2.Distance(ges.position, ges.startPosition);
                float angle = distance * 0.18f / Mathf.PI;
                string str = (ges.swipe).ToString();
                if (str == "Right")
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y-angle,0));
                }
                if (str == "Left")
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + angle, 0));
                }
            }
            */
        }

        protected override void OnDestroy()
        {
            EasyTouch.On_SimpleTap -= OnModelClick;
            //EasyTouch.On_Drag -= OnModelDrag;
        }
        /// <summary>
        /// 设置模型的淡入淡出效果
        /// </summary>
        /// <param name="isappear"></param>
        public void SetAppear( bool isappear,float times)
        {
            if (!isappear)
            {
                //淡出
                StartCoroutine(ControllerColorTimeOut(isappear, times));
            }
            else
            {
                //淡入
                StartCoroutine(ControllerColorTimeIn(isappear, times));
            }
        }
        /// <summary>
        /// 淡出效果 timer: 自定义的时间以秒为单位
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// 
        IEnumerator ControllerColorTimeOut(bool isappear, float timer)
        {
           
            float temptime = timer / Time.deltaTime;
            Renderer[] mtrs = GetComponentsInChildren<Renderer>();
            float mainTemp = modleMainColor.a / temptime;
            float outLineTemp = modleOutlineColor.a / temptime;
            float mainColor = modleMainColor.a - mainTemp;
            float outLineClor = modleOutlineColor.a - outLineTemp;
            while (true)
            {
                    mainColor -= mainTemp;
                    outLineClor -= outLineTemp;
                    foreach (Renderer mtr in mtrs)
                    {
                        //获取shader 的颜色值并设置  

                        mtr.material.SetColor("_Color", new Color(modleMainColor.r, modleMainColor.g, modleMainColor.b, mainColor));
                        mtr.material.SetColor("_OutlineColor", new Color(modleOutlineColor.r, modleOutlineColor.g, modleOutlineColor.b, outLineClor));
                    }
                if (mainColor <= 0 && outLineClor <= 0)
                {
                    break;
                }
                yield return 0;
            }
        }
        /// <summary>
        /// 淡入效果
        /// </summary>
        /// <param name="isappear"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        IEnumerator ControllerColorTimeIn(bool isappear, float timer)
        {
            float temptime = timer / Time.deltaTime;
            Renderer[] mtrs = GetComponentsInChildren<Renderer>();
            float mainTemp = modleMainColor.a / temptime;
            float outLineTemp = modleOutlineColor.a / temptime;
            float mainColor = 0 + mainTemp;
            float outLineClor = 0 + outLineTemp;
            while (true)
            {
                mainColor += mainTemp;
                outLineClor += outLineTemp;
                if (outLineClor >= modleOutlineColor.a)
                {
                    outLineClor = modleOutlineColor.a;
                }
                foreach (Renderer mtr in mtrs)
                {
                    //获取shader 的颜色值并设置  

                    mtr.material.SetColor("_Color", new Color(modleMainColor.r, modleMainColor.g, modleMainColor.b, mainColor));
                    mtr.material.SetColor("_OutlineColor", new Color(modleOutlineColor.r, modleOutlineColor.g, modleOutlineColor.b, outLineClor));


                }
                if (mainColor >= 1 && outLineClor >= modleOutlineColor.a)
                {
                    break;
                }
                yield return 0;
            }
        }
       
      
        /// <summary>
        /// 获得模型之前的颜色值
        /// </summary>
        /// <returns></returns>
        private Color GetModleOutlineColor() 
        {
            Renderer[] mtrs = GetComponentsInChildren<Renderer>();
            Color color = Color.black;
            foreach (Renderer mtr in mtrs)
            {
                if (mtr.material.HasProperty("_OutlineColor"))
                    color = mtr.material.GetColor("_OutlineColor");
            }
            return color;
        }
        /// <summary>
        /// 获得模型主颜色值
        /// </summary>
        /// <returns></returns>
        private Color GetModleMainColor()
        {
            Renderer[] mtrs = GetComponentsInChildren<Renderer>();
            Color color = Color.black;
            foreach (Renderer mtr in mtrs)
            {
                if (mtr.material.HasProperty("_Color"))
                    color = mtr.material.GetColor("_Color");
            }
            return color;
        }

        public void PlayShow(string rolename, bool isstand )
        {
            mAnim.Stand(isstand);

            CameraManager.Instance.PlayPath(rolename, isstand);
            if (!isstand)
            {
                while (mEffects.Count > 0)
                {
                    mEffects[0].Despawn();
                    mEffects.RemoveAt(0);
                }
            }

            switch (rolename)
            {
                case "船长":
                    if (isstand)
                    {
                        ShowEffect("cz_dl_zhx_.unity3d");
                        ShowEffect("cz_dl_zhx_bip001 prop2.unity3d");
                        ShowEffect("cz_dl_qs_bip001.unity3d",2);
                        ShowEffect("cz_dl_qs_bip001 l hand.unity3d",2);
                        ShowEffect("cz_dl_qs_bip001 r hand.unity3d",2);
                    }
                    break;
                case "火女":
                    if (isstand)
                    {
                        ShowEffect("hv_dl_qs_.unity3d",2);
                        ShowEffect("hv_dl_zhx_.unity3d");
                        ShowEffect("hv_dl_zhx_bip001 l hand.unity3d");
                        ShowEffect("hv_dl_zhx_bip001 r hand.unity3d");
                    }
                    break;
                case "剑圣":
                    if (isstand)
                    {
                        ShowEffect("js_dl_qs_.unity3d", 2);
                        ShowEffect("js_dl_qs_bip001 l hand.unity3d", 2);
                        ShowEffect("js_dl_qs_bip001 r hand.unity3d",2);
                    }
                    break;
                case "冰女":
                    if (isstand)
                    {
                        ShowEffect("bv_dl_qs and zhx_bip001 footsteps.unity3d");
                    }
                    break;
                case "小黑":
                    if(isstand)
                    {
                        ShowEffect("xh_dl_qs_.unity3d",2);
                        ShowEffect("xh_dl_qs_bip001 l hand.unity3d",2);
                        ShowEffect("xh_dl_qs_bip001 r hand.unity3d",2);
                        ShowEffect("xh_dl_zhx_.unity3d");
                    }
                    break;
                case "圣骑":
                    if (isstand)
                    {
                        ShowEffect("qnqs_dl_qishen_.unity3d",2);
                        ShowEffect("qnqs_dl_zhanxunhuan_.unity3d");
                    }
                    break;
            }
        }

        public void PlayAction()
        {
            mAnim.RandomAct();
        }

        void LoadEffect(string resname)
        {
            EffectManager.Instance.LoadEffect(resname, (uobj) =>
            {
                string sourceName = PathMod.GetPureName(resname);
                int index = sourceName.LastIndexOf("_");
                string bone = sourceName.Substring(index + 1);
                if (!string.IsNullOrEmpty(bone))
                {
                    GameObject hang = U3DMod.FindChild(transform, bone);
                    if (hang != null)
                    {
                        mEffectHangDic[resname] = hang.transform;         
                    }
                }
                else
                {
                    mEffectHangDic[resname] = transform; 
                }
                uobj.Despawn();
            });
        }

        void ShowEffect( string resname,float showtime = 0)
        {                           
            EffectManager.Instance.LoadEffect(resname, (uobj) =>
            {
                if( mEffectHangDic.ContainsKey(resname) )
                {
                    U3DMod.AddChild(mEffectHangDic[resname], uobj.gameObject, true);

                    if (showtime != 0f)
                    {
                        UnityMain.Instance.Timers.AddTimer(uobj.Despawn, showtime);
                    }
                    else
                    {
                        mEffects.Add((CEffect)uobj);
                    }  
                }
                else
                {
                    Debug.Log("no hang");
                }
                uobj.Show();
            });
        }
    }
}