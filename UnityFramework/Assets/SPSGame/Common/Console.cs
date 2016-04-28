/// <summary>
/// OnGUIWindow.cs
/// ------------------------
/// 脚脚本功能：显示信息
/// 挂在对象：Camera
/// 编辑记录：
/// 2015.07.17 Creat by qiao
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame; 
using SPSGame.Unity;
namespace SPSGame.Unity
{
    public class Console : MonoBehaviour
    {
        public float width = 360;//窗口的宽
        public float height = 640;//窗口的高
        //定义标准屏幕分辨率
        public float m_fScreenWidth = 720;
        public float m_fScreenHeight = 1280;

        public GUIStyle style;

        private Vector2 scrollPosition = new Vector2(0, 0);
        private Vector2 scrollPosition2 = new Vector2(0, 0);

        private Rect windowRect;//OnGUI 拖动的回调 窗口的位置大小
        private Rect windowSecondRect;//OnGUI 拖动的回调 第二个为调试窗口的位置大小
        private Rect windowThridRect;//OnGUI 拖动的回调 第三个为调试窗口的位置大小


        //定义缩放系数
        private float m_fScaleWidth;
        private float m_fScaleHeight;
        private float _updateInterval = 1f;//设定更新帧率的时间间隔为1秒
        private float _accum = .0f;//累积时间
        private float _timeLeft;

        private int _frames = 0;//在_updateInterval时间内运行了多少帧
        private string fpsFormat;
        private string contantString = "请在这里输入";
        private string longString;
        private string str;

        private bool controlSelect = true;//总开关,显示那个窗口 
        private bool isActive = false;//是否出现窗口  
        private bool isBottom = true;//是否直接到底部
        private bool isAll = true;//显示全部
        private bool isNormall = false;//显示普通信息
        private bool isWarning = false;//显示警告信息
        private bool isCw = false;//显示错误信息
        private bool isFirst = true;//是否刚开始显示的内容
        private bool isThridWind = false;


        List<string> list = new List<string>();
        void Awake()
        {
            //DebugMod.SetLogNormal(AddLogNormal);
            //DebugMod.SetLogWarn(AddLogWarning);
            //DebugMod.SetLogError(AddLogError);

            //计算缩放系数
            m_fScaleWidth = (float)Screen.width / m_fScreenWidth;
            m_fScaleHeight = (float)Screen.height / m_fScreenHeight;
        }

        void Start()
        {
            windowRect = new Rect(0f, 0f, width, height);
            windowSecondRect = new Rect(0f, 0f, width, height);
            windowThridRect = new Rect(0f, 0f, 140, 60);

            _timeLeft = _updateInterval;
        }
        void Update()
        {
            _timeLeft -= Time.deltaTime;
            //Time.timeScale可以控制Update 和LateUpdate 的执行速度,  
            //Time.deltaTime是以秒计算，完成最后一帧的时间  
            //相除即可得到相应的一帧所用的时间  
            _accum += Time.timeScale / Time.deltaTime;
            ++_frames;//帧数  
            if (_timeLeft <= 0)
            {
                float fps = _accum / _frames;
                fpsFormat = System.String.Format("{0:F0} FPS", fps);//保留两位小数  
                _timeLeft = _updateInterval;
                _accum = 0.0f;
                _frames = 0;
            }

        }

        const float m_KBSize = 1024.0f * 1024.0f;
        string GetUsedMemory()
        {
            string memory = string.Empty;
#if UNITY_EDITOR
//        float totalMemory = (float)(Profiler.GetTotalAllocatedMemory() / m_KBSize);
//       float totalReservedMemory = (float)(Profiler.GetTotalReservedMemory() / m_KBSize);
//        memory = totalReservedMemory.ToString();
        memory = "-";
//         float totalUnusedReservedMemory = (float)(Profiler.GetTotalUnusedReservedMemory() / m_KBSize);
//         float monoHeapSize = (float)(Profiler.GetMonoHeapSize() / m_KBSize);
//         float monoUsedSize = (float)(Profiler.GetMonoUsedSize() / m_KBSize);

//        Debug.Log( string.Format("chhh:TotalAllocatedMemory：{0}MB， TotalReservedMemory：{1}MB，TotalUnusedReservedMemory:{2}MB,  MonoHeapSize:{3}MB, MonoUsedSize:{4}MB", totalMemory, totalReservedMemory, totalUnusedReservedMemory, monoHeapSize, monoUsedSize));
//#elif
//        return string.Empty;
//#endif
#elif UNITY_ANDROID 
//         using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//         {
//             using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//             {
//                 string result = obj_Activity.Call<string>("getMemoryInfo");
//                 if (result != "")
//                 {
//                     string[] datas = result.Split('-');
//                     memory = datas[2];
//                 }
// 
//             }
//         }
#endif
            return memory;
        }

        void OnGUI()
        {
            //windowSecondRect.position = windowRect.position;

            //都按照比例缩放
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(m_fScaleWidth, m_fScaleHeight, 1));
            //显示哪一个窗口
            if (controlSelect)
            {
                if (isActive)
                {
                    windowRect = GUI.Window(1, windowRect, WindowMethod, string.Format("{0}/{1:F2}MB", fpsFormat, GetUsedMemory()));
                    windowSecondRect.position = windowRect.position;
                }
                else
                {
                    if (isThridWind)
                    {
                        isActive = true;
                        windowRect.position = windowThridRect.position;
                    }
                    else
                    {
                        windowThridRect = GUI.Window(3, windowThridRect, WindowThridMethod, "", style);
                    }
                }
            }
            else
            {
                windowSecondRect = GUI.Window(2, windowSecondRect, WindowSecondMethod, fpsFormat);
                windowRect.position = windowSecondRect.position;
            }
        }
        void WindowThridMethod(int id)
        {
            GUI.Label(new Rect(5, 25, 80, 50), fpsFormat);
            //GUILayout.Label(fpsFormat);
            /*if (controlSelect)
            {
                if (isActive)
                {
                    windowRect = GUI.Window(1, windowRect, WindowMethod, string.Format("{0}/{1:F2}MB", fpsFormat, GetUsedMemory()));
                    windowSecondRect.position = windowRect.position;
                }
                else
                {
                    if (GUI.Button(new Rect(60, 20, 60, 40),fpsFormat))
                    {
                        isActive = true;
                    }
                }
            }
            else
            {
                windowSecondRect = GUI.Window(2, windowSecondRect, WindowSecondMethod, fpsFormat);
                windowRect.position = windowSecondRect.position;
            }*/
            if (GUI.Button(new Rect(80, 20, 60, 50), "调试"))
            {
                isThridWind = true;
            }
            //拖拽窗口
            GUI.DragWindow();
        }


        /// <summary>
        /// 窗口一的方法
        /// </summary>
        /// <param name="id"></param>
        void WindowMethod(int id)
        {
            if (isActive)
            {
                if (GUI.Button(new Rect(width - 40, 0, 40, 20), "X"))
                {
                    isActive = false;
                    isThridWind = false;
                }
                if (GUI.Button(new Rect(0, height - 80, width / 5, 40), "全部"))
                {
                    isBottom = true;
                    isAll = true;
                    isNormall = false;
                    isWarning = false;
                    isCw = false;
                }
                if (GUI.Button(new Rect(width / 5, height - 80, width / 5, 40), "普通"))
                {
                    isBottom = true;
                    isAll = false;
                    isNormall = true;
                    isWarning = false;
                    isCw = false;
                }
                if (GUI.Button(new Rect(width * 2 / 5, height - 80, width / 5, 40), "警告"))
                {
                    isBottom = true;
                    isAll = false;
                    isNormall = false;
                    isWarning = true;
                    isCw = false;
                }
                if (GUI.Button(new Rect(width * 3 / 5, height - 80, width / 5, 40), "错误"))
                {
                    isBottom = true;
                    isAll = false;
                    isNormall = false;
                    isWarning = false;
                    isCw = true;
                }
                if (GUI.Button(new Rect(width * 4 / 5, height - 80, width / 5, 40), "清除"))
                {
                    isBottom = true;
                    isAll = false;
                    isNormall = false;
                    isWarning = false;
                    isCw = false;
                    list = new List<string>();
                }
                if (isAll)
                    GetAppearIntAll();
                if (isNormall)
                    GetAppearNormall();
                if (isWarning)
                    GetAppearWarning();
                if (isCw)
                    GetAppearCW();
                if (GUI.Button(new Rect(0, height - 40, width / 2, 40), "LOG"))
                {
                }
                if (GUI.Button(new Rect(width / 2, height - 40, width / 2, 40), "调试"))
                {
                    controlSelect = false;
                }
                //文本的输出直接到底部
                if (isBottom)
                {
                    scrollPosition.y = float.MaxValue;
                    isBottom = false;
                }
                //拖拽窗口
                GUI.DragWindow();
            }
        }


        /// <summary>
        /// 窗口二的方法
        /// </summary>
        /// <param name="id"></param>
        void WindowSecondMethod(int id)
        {
            if (Input.GetMouseButtonDown(0) && isFirst)
            {
                contantString = "";
                isFirst = false;
            }
            contantString = GUI.TextArea(new Rect(0, height - 80, width * 3 / 4, 40), contantString, 200);
            if (GUI.Button(new Rect(width - 40, 0, 40, 20), "X"))
            {
                windowSecondRect.position = Vector2.zero;
                controlSelect = true;
                isActive = false;
                isThridWind = false;
            }
            if (GUI.Button(new Rect(0, height - 40, width / 2, 40), "LOG"))
            {
                controlSelect = true;
            }
            if (GUI.Button(new Rect(width / 2, height - 40, width / 2, 40), "调试"))
            {
                //print("调试");
            }
            if (GUI.Button(new Rect(width * 3 / 4, (height - 80), width / 4, 40), "发送"))
            {
                longString += contantString + "\n";
//                Logicer.GM(contantString);

                contantString = "";
                isBottom = true;
            }
            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, GUILayout.Width(width - 10), GUILayout.Height(height - 100));
            GUI.skin.label.fontSize = 20;
            GUI.color = Color.red;
            GUILayout.Label(longString);
            GUILayout.EndScrollView();
            if (isBottom)
            {
                scrollPosition2.y = float.MaxValue;
                isBottom = false;
            }
            GUI.DragWindow();
        }


        /// <summary>
        /// 输出全部
        /// </summary>
        void GetAppearIntAll()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width - 10), GUILayout.Height(height - 100));
            foreach (string item in list)
            {
                if (item.StartsWith("W"))
                {
                    str = item.Substring(1);
                    GUI.color = Color.yellow;
                    str = "警告 ：" + str;
                    GUILayout.Label(str);
                }
                else if (item.StartsWith("N"))
                {
                    str = item.Substring(1);
                    str = "普通 ：" + str;
                    GUILayout.Label(str);
                }
                else if (item.StartsWith("E"))
                {
                    str = item.Substring(1);
                    GUI.color = Color.red;
                    str = "错误 ：" + str;
                    GUILayout.Label(str);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 输出普通信息
        /// </summary>
        void GetAppearNormall()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width - 10), GUILayout.Height(height - 100));
            foreach (string item in list)
            {
                if (item.StartsWith("N"))
                {
                    str = item.Substring(1);
                    str = "普通 ：" + str;
                    GUILayout.Label(str);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 输出警告
        /// </summary>
        void GetAppearWarning()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width - 10), GUILayout.Height(height - 100));
            foreach (string item in list)
            {
                if (item.StartsWith("W"))
                {
                    str = item.Substring(1);
                    GUI.color = Color.yellow;
                    str = "警告 ：" + str;
                    GUILayout.Label(str);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 输出错误
        /// </summary>
        void GetAppearCW()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width - 10), GUILayout.Height(height - 100));
            foreach (string item in list)
            {
                if (item.StartsWith("E"))
                {
                    str = item.Substring(1);
                    GUI.color = Color.red;
                    str = "错误 ：" + str;
                    GUILayout.Label(str);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 一个外部调用接口，添加普通的信息
        /// </summary>
        /// <param name="information"></param>
        public void AddLogNormal(string information)
        {
            list.Add("N" + information);
        }


        /// <summary>
        /// 添加警告信息
        /// </summary>
        /// <param name="information"></param>
        public void AddLogWarning(string information)
        {
            list.Add("W" + information);
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="information"></param>
        public void AddLogError(string information)
        {
            list.Add("E" + information);
        }
    }
}
