using UnityEngine;
using System.Collections;
using SPSGame;

namespace SPSGame.Unity
{

    public interface IObject
    {     
        GameObject u3dObject { get; }

        Transform u3dParent { get;}

        Vector3 position { get; set; }

        Vector3 direction { get; set; }

        float moveSpeed { get; set; }

        bool isInit { get; }

        bool isShow { get; }

        void Init();

        void Show( bool isshow );

        void RenderFixedUpdate();

        void RenderUpdate();

        void Destroy();
    }
}