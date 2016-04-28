using UnityEngine;
using System.Collections;

namespace SPSGame.Unity
{
    public class UIObject : U3DObject
    {

        protected void ListenOnSubmit(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onSubmit = callback;
        }

        protected void ListenOnClick(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onClick = callback;
        }

        protected void ListenOnPress(GameObject obj, UIEventListener.BoolDelegate callback)
        {
            UIEventListener.Get(obj).onPress = callback;
        }

        protected void ListenOnSelect(GameObject obj, UIEventListener.BoolDelegate callback)
        {
            UIEventListener.Get(obj).onSelect = callback;
        }

        protected void ListenOnDragStart(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onDragStart = callback;
        }

        protected void ListenOnDragOver(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onDragOver = callback;
        }

        protected void ListenOnDrag(GameObject obj, UIEventListener.VectorDelegate callback)
        {
            UIEventListener.Get(obj).onDrag = callback;
        }

        protected void ListenOnDragEnd(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onDragEnd = callback;
        }

        protected void ListenOnDragOut(GameObject obj, UIEventListener.VoidDelegate callback)
        {
            UIEventListener.Get(obj).onDragOut = callback;
        }

        protected void ListenOnDrop(GameObject obj, UIEventListener.ObjectDelegate callback)
        {
            UIEventListener.Get(obj).onDrop = callback;
        }
    }
}