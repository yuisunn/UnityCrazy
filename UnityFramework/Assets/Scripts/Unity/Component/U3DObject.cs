using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class U3DObject : MonoBehaviour
    {

        public UnityAction<U3DObject> DespawnHandler;
        public UnityAction<U3DObject> DestroyHandler;
        public UnityAction<U3DObject> OnDestroyHandler;

        protected virtual void Awake()
        {

        }


        protected virtual void OnEnable()
        {

        }


        protected virtual void Start()
        {

        }

        public virtual bool isShow()
        {
            if (gameObject != null)
                return U3DMod.isActive(gameObject);
            else
                return false;
        }

        public virtual void Show( bool isshow )
        {
            if (null != gameObject)
                U3DMod.SetActive(gameObject, isshow);
        }

        public virtual void Show()
        {
            Show(true);
        }

        public virtual void Hide()
        {
            Show(false);
        }

        public virtual void Despawn()
        {
            if (DespawnHandler != null)
                DespawnHandler(this);
            else
                Destroy();
        }

        public virtual void Destroy()
        {
            if (DestroyHandler != null)
                DestroyHandler(this);
            if (gameObject != null)
                U3DMod.Destroy(gameObject);
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnDestroy()
        {
            if (OnDestroyHandler != null)
                OnDestroyHandler(this);
        }
    }
}