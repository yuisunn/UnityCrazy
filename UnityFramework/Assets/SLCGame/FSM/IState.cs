using UnityEngine;
using System.Collections;

namespace SLCGame
{
    public interface IState
    {
        void OnEnter(string prevState);
        void OnExit(string nextState);
        void OnUpdate();
    }
}