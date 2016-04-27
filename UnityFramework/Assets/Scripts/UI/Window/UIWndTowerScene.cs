using UnityEngine;
using System.Collections;


namespace SPSGame.Unity
{
    public class UIWndTowerScene : UIWndBase
    {
        public UITexture imageTexture = null;
        public GameObject jiantouSpr = null;

        public GameObject backBtn = null;
        public VoidDelegate OnPressUpImageHandler = null;

        GameObject mRTModel = null;
        Camera mRTCamera = null;
        int mHeroResID = -1;
        Vector3 imageOriginalPos;
        Vector3 jiantouOriginalPos;
        protected override void Awake()
        {
            base.Awake();

            ListenOnClick(backBtn, OnClickBackLogin);
            ListenOnPress(imageTexture.gameObject, OnPressImageStart);
        }

        protected override void Start()
        {
            base.Start();
            imageOriginalPos = imageTexture.transform.localPosition;
            //jiantouOriginalPos = jiantouSpr.transform.localPosition;
            CreateModel();
        }

        public void Init( int heroresid )
        {
            mHeroResID = heroresid;
        }

        public void Resume()
        {
            imageTexture.transform.localPosition = imageOriginalPos;
        }

        void CreateModel()
        {
            string filename = DataManager.Instance.GetModelRes(mHeroResID);
            string sourcename = PathMod.GetPureName(filename);

            AssetBundleManager.Instance.LoadMonsterByLoader(filename, sourcename, (go) =>
            {
                mRTModel = U3DMod.Clone(go as GameObject);
                mRTModel.transform.position = new Vector3(100, 0, 0);
                mRTModel.transform.rotation = Quaternion.Euler(0, -180, 0);

                GameObject obj = new GameObject("_RT Camera");
                mRTCamera = obj.AddComponent<Camera>();
                obj.transform.position = new Vector3(100, 1.45f, -5);
                mRTCamera.cullingMask = 1;
                mRTCamera.orthographic = true;
                mRTCamera.orthographicSize = 1.5f;

                RenderTexture rd = new RenderTexture(400, 600, 1);
                mRTCamera.targetTexture = rd;
                imageTexture.mainTexture = rd;
            });
        }


        void OnPressImageStart(GameObject go, bool state)
        {                  
            if(state)  
            UnityMain.StartCoroutine(ShowDragHero(null));
        }


        IEnumerator ShowDragHero(VoidDelegate cb)
        {
            Vector3 pos = Vector3.zero;
            bool iswork = true;
            U3DMod.SetActive(jiantouSpr, false);

            while (iswork)
            {
                if (Input.GetMouseButton(0))
                {
                    pos = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
                    imageTexture.transform.position = pos;
//                     if (dragmodel != null)
//                         dragmodel.transform.position = pos;
                }

                

                if (Input.GetMouseButtonUp(0))
                {
                    if (OnPressUpImageHandler!= null)
                        OnPressUpImageHandler();

                    imageTexture.transform.localPosition = imageOriginalPos;
                    iswork = false;
                    U3DMod.SetActive(jiantouSpr, true);
                }

                yield return 0;
            }
        }

        void OnClickBackLogin(GameObject obj)
        {
            Logicer.ChangeScene(101);
            //Logicer.ReturnToLogin();
        }

        public override void Destroy()
        {
            if (mRTCamera != null)
                U3DMod.Destroy(mRTCamera.gameObject);
            if (mRTModel != null)
                U3DMod.Destroy(mRTModel);

            base.Destroy();
        }

        protected override void Update()
        {
            base.Update();

            //if (jiantouSpr != null)
               // jiantouSpr.transform.localPosition = jiantouOriginalPos + new Vector3(0, 10* Mathf.Sin(10*Time.time) , 0);
        }

    }
}