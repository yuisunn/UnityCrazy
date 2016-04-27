using UnityEngine;
using System.Collections;
using SPSGame;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class GObject: GObjectBase, IObject
    {

        #region IObject属性

        public int resID = -1;

        protected GameObject mU3dObject = null;
        /// <summary>
        /// GameObject对象
        /// </summary>
        public GameObject u3dObject{
            get{
                return mU3dObject;
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public Transform u3dParent{
            get{
                return mU3dObject.transform.parent;
            }
        }

        
        protected Vector3 mDestPostion = Vector3.zero;
        /// <summary>
        /// 空间坐标
        /// </summary>
        public Vector3 destPosition{
            get{
                return mDestPostion;
            }
            set{
                mDestPostion = value;
            }
        }

        protected Vector3 mDestDirection = Vector3.zero;
        /// <summary>
        /// 空间坐标
        /// </summary>
        public Vector3 destDirection{
            get{
                return mDestDirection;
            }
            set{
                mDestDirection = value;
            }
        }

        protected Vector3 mPostion = Vector3.zero;
        /// <summary>
        /// 空间坐标
        /// </summary>
        public virtual Vector3 position{
            get{        
                return mPostion;
            }
            set{
                mPostion = value;
            }
        }

        protected Vector3 mDirection = Vector3.forward;
        /// <summary>
        /// 空间向量
        /// </summary>
        public Vector3 direction{
            get{
                return mDirection;
            }
            set{
                mDirection = value;
            }
        }

        protected float mMoveSpeed = 0f;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float moveSpeed{
            get{
                return mMoveSpeed;
            }
            set{
                mMoveSpeed = value;
            }
        }

        protected bool mIsInit = false;
        /// <summary>
        /// 是否在初始化
        /// </summary>
        public bool isInit{
            get{
                return mIsInit;
            }
        }

        protected bool mIsShow = true;
        /// <summary>
        /// 是否在显示
        /// </summary>
        public virtual bool isShow{
            get{
                return mIsShow;
            }
            set{
                mIsShow = value;
                if (u3dObject != null)
                    U3DMod.SetActive(u3dObject, mIsShow);
            }
        }

        #endregion IObject 属性


        #region IObject 方法
        public virtual void Init()
        {

            if (isInit)
                return;

            Load3DRes();

            RenderManager.Instance.Add(this);

            mIsInit = true;
        }

        protected virtual void Load3DRes()
        {
            if (resID == -1)
            {
                DebugMod.LogError("GObject has no res id");
                return;
            }

            string filename = null;
            string sourcename = null;

            filename = DataManager.Instance.GetModelRes(resID);
            sourcename = PathMod.GetPureName(filename);

            AssetBundleManager.Instance.LoadMonsterByLoader(filename, sourcename, (go) =>
            {
                mU3dObject = U3DMod.Clone(go as GameObject);
                OnLoadResComplete();
            });
        }

        protected virtual void OnLoadResComplete()
        {
            mU3dObject.transform.position = position;
            mU3dObject.transform.forward = direction;
        }

        public virtual void Show(bool isshow)
        {
            isShow = isshow;
        }


        public virtual void SetUp3DRes()
        {
            if (u3dObject != null)
            {
                if (u3dObject.transform.position != position)
                    u3dObject.transform.position = position;

                u3dObject.transform.forward = direction;
            }
        }

        public virtual void RenderFixedUpdate()
        {
            if (!isShow)
                return;

            SetUp3DRes();
        }

        /// <summary>
        /// 渲染逻辑
        /// </summary>
        /// <returns></returns>
        public virtual void RenderUpdate()
        {
            if (!isShow)
                return;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public virtual void Destroy()
        {
            U3DMod.Destroy(u3dObject);
            RenderManager.Instance.Remove(this);
        }
        #endregion IObject 方法
    }
}