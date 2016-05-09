using UnityEngine;
using System.Collections;
using System.Reflection; 

namespace SLCGame.Unity
{
    public class U3DMod
    {
        public static T New<T>( string name = null ) where T : Component
        {
            string goname = name == null ? typeof(T).ToString() : name;
            GameObject go = new GameObject(goname);
            T t = go.AddComponent<T>();
            return t;
        }

        public static T Find<T>() where T : Component
        {
            T t = GameObject.FindObjectOfType<T>();
            return t;
        }

        public static GameObject Find(string name)
        {
            GameObject go = GameObject.Find(name);
            return go;
        }

        public static GameObject FindChild(Transform transform, string childname,bool lowercompare = true)
        {
            if (null == transform)
			{
                DebugMod.LogError("transform is null in FindChild");
                return null;
			}

            int totalChildCount = transform.childCount;
			for (int i = 0; i < totalChildCount; i++)
			{
                string name = transform.GetChild(i).gameObject.name;
                if ((lowercompare?name.ToLowerInvariant():name) == childname.ToLowerInvariant())
				{
                    return transform.GetChild(i).gameObject;
				}
			}
			
			for (int i = 0; i < totalChildCount; i++)
			{
                GameObject go = FindChild(transform.GetChild(i), childname);
				if (null != go)
				{
					return go;
				}
			}
			
			return null;
        }

        public static GameObject Clone(GameObject sample)
        {
            return Clone<GameObject>(sample);
        }

        public static T Clone<T>(Object sample) where T:Object
        {
            if (sample == null)
            {
                DebugMod.LogError("sample is null in:" + MethodBase.GetCurrentMethod().Name);
                return null;
            }
            return GameObject.Instantiate(sample) as T;
        }

        public static T GetComponent<T>(GameObject go) where T:Component
        {
            if(go == null)
            {
                DebugMod.LogError("gameobject is null");
            }

            T t = go.GetComponent<T>();
            if(t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }

        public static void AddChild(Object parent, Object child, bool usechildoriginaltransform = false)
        {
            if (child == null)
            {
                DebugMod.LogError("child is null in:" + MethodBase.GetCurrentMethod().Name);
                return;
            }               

            Transform prt = null;
            Transform cld = null;

            if(parent != null)
            {
                if (parent is GameObject)
                {
                    prt = ((GameObject)parent).transform;
                }
                else if (parent is Transform)
                {
                    prt = (Transform)parent;
                }
                else
                {
                    DebugMod.LogError("parent type is unknown in:" + MethodBase.GetCurrentMethod().Name);
                }
            }


            if (child is GameObject)
            {
                cld = ((GameObject)child).transform;
            }
            else if (child is Transform)
            {
                cld = (Transform)child;
            }
            else
            {
                DebugMod.LogError("child type is unknown in:" + MethodBase.GetCurrentMethod().Name);
            }

            if (prt == null || cld == null)
                return;

            Vector3 localpos = cld.localPosition;
            Quaternion localrot = cld.localRotation;
            Vector3 localscl = cld.localScale;

            cld.parent = prt;
            cld.localPosition = usechildoriginaltransform ? localpos : Vector3.zero;
            cld.localRotation = usechildoriginaltransform ? localrot : Quaternion.identity;
            cld.localScale = usechildoriginaltransform ? localscl : Vector3.one;
        }

        public static T AddChild<T>(GameObject parent) where T : UnityEngine.Object
        {
            T t = new GameObject(GetTypeName<T>()) as T;
            AddChild(parent, t as GameObject);
            return t;
        }

        public static string GetTypeName<T>()
        {
            string s = typeof(T).ToString();
            if (s.StartsWith("UI")) s = s.Substring(2);
            else if (s.StartsWith("UnityEngine.")) s = s.Substring(12);
            return s;
        }


        public static bool isActive(GameObject go)
        {
            return go.activeSelf;
        }


        public static void SetActive(GameObject go, bool state)
        {       
            if( isActive(go) != state )
                go.SetActive(state);
        }


        public static void Destroy(UnityEngine.Object obj,float time)
        {
            if (obj)
            {
                if (obj is Transform)
                {
                    Transform t = (obj as Transform);
                    GameObject go = t.gameObject;

                    if (Application.isPlaying)
                    {
                        UnityEngine.Object.Destroy(go, time);
                    }
                    else UnityEngine.Object.DestroyImmediate(go);
                }
                else if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;

                    if (Application.isPlaying)
                    {
                        UnityEngine.Object.Destroy(go, time);
                    }
                    else UnityEngine.Object.DestroyImmediate(go);
                }
                else if (Application.isPlaying) UnityEngine.Object.Destroy(obj,time);
                else UnityEngine.Object.DestroyImmediate(obj);
            }
        }

        public static void Destroy(UnityEngine.Object obj)
        {
            if (obj)
            {
                if (obj is Transform)
                {
                    Transform t = (obj as Transform);
                    GameObject go = t.gameObject;

                    if (Application.isPlaying)
                    {
                        t.parent = null;
                            UnityEngine.Object.Destroy(go);
                    }
                    else UnityEngine.Object.DestroyImmediate(go);
                }
                else if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;
                    Transform t = go.transform;

                    if (Application.isPlaying)
                    {
                        t.parent = null;
                        UnityEngine.Object.Destroy(go);
                    }
                    else UnityEngine.Object.DestroyImmediate(go);
                }
                else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
                else UnityEngine.Object.DestroyImmediate(obj);
            }
        }

        public static void DestroyImmediate(UnityEngine.Object obj)
        {
            if (obj != null)
            {
                if (Application.isEditor) UnityEngine.Object.DestroyImmediate(obj);
                else UnityEngine.Object.Destroy(obj);
            }
        }

    }
}