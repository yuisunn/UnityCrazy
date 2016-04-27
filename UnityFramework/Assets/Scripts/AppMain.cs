using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame;
using SPSGame.Unity;
public class AppMain : MonoBehaviour 
{

    public GameObject uiRoot;
    public GameObject hudRoot;
    public Camera camera3D = null;

    void Awake()
    {
        LogicMain.Instance.Init();
        UnityMain.Instance.Init();

        UIManager.Instance.uiRoot = uiRoot;
        UIManager.Instance.hudRoot = hudRoot;
        CameraManager.Instance.camera3D = camera3D;
    }


    // Use this for initialization
    void Start()
    {
        LogicMain.Instance.OnAppStart();
    }

    void FixedUpdate()
    {
        UnityMain.Instance.FixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        LogicMain.Instance.OnAppUpdate();
        UnityMain.Instance.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void LateUpdate()
    {
        UnityMain.Instance.LateUpdate();
    }

    void OnApplicationQuit()
    {
        LogicMain.Instance.OnApplicationQuit();
    }

}
