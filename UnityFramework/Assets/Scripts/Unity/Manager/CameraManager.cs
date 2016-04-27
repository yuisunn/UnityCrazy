using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;
using System;
using System.Linq;

namespace SPSGame.Unity
{
    public class CameraManager : Singleton<CameraManager>
    {
        public Camera camera3D = null;

        private CCameraPathController mCamePath;

        Vector3 mCam3DPostion;
        bool mPathForward = false;

        private GSprite mLookTarget = null;

        private void path_AnimationFinishedEvent(bool isforward)
        {
            if (!mPathForward)
                U3DMod.SetActive(UIManager.Instance.hudRoot, true);
        }

        IEnumerator CameraBack()
        {
            while( camera3D.transform.rotation != Quaternion.Euler(13.2f, 2.2f, 1.5f) )
            {
                camera3D.transform.rotation = Quaternion.Slerp(camera3D.transform.rotation, Quaternion.Euler(13.2f, 2.2f, 1.5f), Time.deltaTime * 15f);
                yield return 0;
            }
        }

        public void Set3DCamera( Vector3 pos,Quaternion rot,float viewfield )
        {
            camera3D.transform.position = pos;
            camera3D.transform.rotation = rot;
            camera3D.fieldOfView = viewfield;

            mCam3DPostion = camera3D.transform.position;
        }

        public void InitSwiper(float bottom, float top, float left, float right)
        {
            SwiperControler swiper = U3DMod.GetComponent<SwiperControler>(camera3D.gameObject);
            swiper.swiperType = SwiperControler.ESwiperType.Vertical;
            swiper.Init(mCam3DPostion, top, bottom, left, right);
            swiper.GetPositionHandler = () => { return mCam3DPostion;};
            swiper.SetPositionHandler = (pos) => { mCam3DPostion = pos; };
        }

        public void RemoveSwiper()
        {
            SwiperControler swiper = U3DMod.GetComponent<SwiperControler>(camera3D.gameObject);
            if(swiper != null)
                swiper.DestroyThisComponent();
        }


        public void InitPath( Transform[] targets )
        {
            mCamePath = ResourceManager.New<CCameraPathController>("Prefab/CCameraPathController");
    
            GameObject hang = new GameObject("_cameratarget");
            U3DMod.AddChild(targets[3], hang);
            hang.transform.localPosition = new Vector3(0, 0.5f, 1.3f);
            mCamePath.path4.orientationTarget = hang.transform;
           
            mCamePath.path1.AnimationFinishedEvent += path_AnimationFinishedEvent;
            mCamePath.path2.AnimationFinishedEvent += path_AnimationFinishedEvent;
            mCamePath.path3.AnimationFinishedEvent += path_AnimationFinishedEvent;
            mCamePath.path4.AnimationFinishedEvent += path_AnimationFinishedEvent;
            mCamePath.path5.AnimationFinishedEvent += path_AnimationFinishedEvent;
            mCamePath.path6.AnimationFinishedEvent += path_AnimationFinishedEvent;
        }

        public void PlayPath(string rolename, bool isforward)
        {
            mPathForward = isforward;
            switch (rolename)
            {
                case "船长":
                    mCamePath.CameraNear(mCamePath.path1,isforward);
                    break;
                case "剑圣":
                    mCamePath.CameraNear(mCamePath.path2, isforward);
                    break;
                case "圣骑":
                    mCamePath.CameraNear(mCamePath.path3, isforward);
//                     if (isforward)
//                         mCamePath.path3.orientationMode = CameraPathAnimator.orientationModes.target;
//                     else
//                     {
//                         mCamePath.path3.orientationMode = CameraPathAnimator.orientationModes.none;
//                         UnityMain.StartCoroutine(CameraBack());
//                     }   
                    break;
                case "小黑":
                    mCamePath.CameraNear(mCamePath.path4, isforward);
                    if (isforward)
                        mCamePath.path4.orientationMode = CameraPathAnimator.orientationModes.target;
                    else
                    {
                        UnityMain.Instance.Timers.AddTimer(() => { 
                            mCamePath.path4.orientationMode = CameraPathAnimator.orientationModes.none;
                            UnityMain.StartCoroutine(CameraBack());
                        },.21f);         
                    }                       
                    break;
                case "冰女":
                    mCamePath.CameraNear(mCamePath.path5, isforward);
                    break;
                case "火女":
                    mCamePath.CameraNear(mCamePath.path6,isforward);
                    break;
            }
        }


        public void RemovePath()
        {
            mCamePath.path1.AnimationFinishedEvent -= path_AnimationFinishedEvent;
            mCamePath.path2.AnimationFinishedEvent -= path_AnimationFinishedEvent;
            mCamePath.path3.AnimationFinishedEvent -= path_AnimationFinishedEvent;
            mCamePath.path4.AnimationFinishedEvent -= path_AnimationFinishedEvent;
            mCamePath.path5.AnimationFinishedEvent -= path_AnimationFinishedEvent;
            mCamePath.path6.AnimationFinishedEvent -= path_AnimationFinishedEvent;

            U3DMod.Destroy(mCamePath);
            mCamePath = null;
        }

        public void SetTarget( GSprite tar )
        {
            mLookTarget = tar;
        }

        void LookAt( Vector3 pos )
        {
            Vector3 forw = camera3D.transform.forward;
            mCam3DPostion = pos - forw.normalized * 30;
        }


        public void LateUpdate()
        {
            if (mCamePath != null)
                return;

            if (mLookTarget!= null)
            {
                LookAt(mLookTarget.position);
            }
// 
//             if (mCam3DPostion != camera3D.transform.position)
//                 camera3D.transform.position = mCam3DPostion;
            if (Vector3.Magnitude(mCam3DPostion - camera3D.transform.position) > .1f)
            {
                camera3D.transform.position = Vector3.Lerp(camera3D.transform.position, mCam3DPostion, Time.deltaTime * 5);
            }
        }
    }

}