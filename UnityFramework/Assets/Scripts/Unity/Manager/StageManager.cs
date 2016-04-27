using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class StageManager : Singleton<StageManager>
    {
        private EGameStage mCurrentStage = EGameStage.Default;
        private StageBase mStage = null;

        public void ChangeGameStage( EGameStage nextstage )
        {
            if (mCurrentStage != EGameStage.Default)
            {
                LeaveStage(mCurrentStage);
            }

            if (mCurrentStage != nextstage)
            {
                mCurrentStage = nextstage;
                EnterStage(mCurrentStage);
            }      

        }


        void EnterStage( EGameStage stage )
        {       
            switch (stage)
            {
                case EGameStage.StartUp:
                    DataManager.Instance.Init();
                    break;
                case EGameStage.Update:
                    break;
                case EGameStage.Login:
                    UIManager.Instance.ShowWindow<UILogin>();
                    SceneManager.Instance.LeaveGameScene(null);
                    break;
                case EGameStage.SelectRole:
                    mStage = new RoleStage();
                    //mStage = U3DMod.New<RoleStage>();
                    mStage.StartStage();
                    break;
                case EGameStage.Gaming:
                    //mStage = U3DMod.New<GameStage>();
                    mStage = new GameStage();
                    mStage.StartStage();
                    break;
                case EGameStage.Default:
                    return;
            }

            EventManager.Trigger<EventGameStageEnter>(new EventGameStageEnter(mCurrentStage));
        }


        void LeaveStage( EGameStage stage )
        {
            if(mStage != null)
                mStage.EndStage();
            mStage = null;

            switch (stage)
            {
                case EGameStage.StartUp:
                    break;
                case EGameStage.Update:
                    break;
                case EGameStage.Login:
                    UIManager.Instance.DestroyWindow<UILogin>();
                    break;
                case EGameStage.SelectRole:
                    break;
                case EGameStage.Gaming:
                    break;
                case EGameStage.Default:
                    break;
            }

            EventManager.Trigger<EventGameStageLeave>(new EventGameStageLeave(mCurrentStage));          
        }
    }
}