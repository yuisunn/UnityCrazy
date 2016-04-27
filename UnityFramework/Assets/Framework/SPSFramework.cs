using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SPSGame.CsShare;
using SPSGame.GameModule;
using SPSGame.Tools;

namespace SPSGame
{
    public class TcpServerSetting
    {
        public string serverName;
        public string serverIp;
        public int port;
        public int rcvTimeout = 5000;
        public int sendTimeOut = 5000;
        public int connTimeout = 5000;
        public int maxPackSize = (int)TCPClientEnum.MAX_PACKET_SIZE;
    }

    public class HttpServerSetting
    {
        public string serverName;
        public string url;
        public int port;
        public int overtime = 30;
    }

    public class SPSFramework
    {
        private static Threader _frmworkThreader = new Threader();
        private static bool _appExit = false;
        private ServerManager _svrMgr = new ServerManager();
        private StageManager _stageMgr = new StageManager();
        private GameModuleManager _moduleMgr = new GameModuleManager();
        private SceneManager _sceneMgr = new SceneManager();
        private Queue<LogicAction> m_queueLogicAction = new Queue<LogicAction>();

        public void OnApplicationQuit()
        {
            SPSFramework._appExit = true;
            _svrMgr.OnApplicationQuit();
        }

        public static void LT(Action action)
        {
            _frmworkThreader.QueueOnMainThread(action);
        }

        private void InitAssembly()
        {
            //Type thisType = Type.GetType("SPSGame.SPSFramework");
            LogicActionFactory.Instance.ActionAssembly = NetActionFactory.Instance.ActionAssembly
                = System.Reflection.Assembly.GetCallingAssembly();
        }

        protected virtual void InitMore() { }
        public void Init()
        {
            try
            {
                Local.Instance.SvrMgr = _svrMgr;
                Local.Instance.ModuleMgr = _moduleMgr;
                Local.Instance.SceneMgr = _sceneMgr;
                Local.Instance.StageMgr = _stageMgr;

                InitStages();
                InitAssembly();
                InitMore();

                //创建逻辑线程
                DebugMod.Log("Create Logic Thread.");
                Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
                t.Start(this);
                Thread.Sleep(0);
                DebugMod.Log("Create Logic Thread OK.");
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

		public virtual void OnDataReady ()
        {
            getAllDataFromUnity();
            Local.Instance.SceneMgr.InitAllGameScene();
            InitServers();
            InitGameModules();
        }
		
        public virtual void OnAppStart()
        {
            _stageMgr.BeginStage("startup");
        }

        public virtual void OnAppUpdate()
        {
        }

        public void OnThreadUpdate()
        {
            try
            {
                DequueLogicAction();
                _frmworkThreader.Update();
                _stageMgr.OnUpdate();
                _svrMgr.OnUpdate();
                _sceneMgr.OnUpdate();
                _moduleMgr.OnUpdate();
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public static void ThreadProc(object param)
        {
            try
            {
                DebugMod.Log("Logic Thread Start.");

                SPSFramework frame = param as SPSFramework;
                long prevTick = DateTime.Now.Ticks;
                if (null != param)
                {
                    while (!_appExit)
                    {
                        long nowTick = DateTime.Now.Ticks;
                        long elapseTick = nowTick - prevTick;
                        if (elapseTick > 333333)        //33ms
                        {
                            prevTick = nowTick;
                            frame.OnThreadUpdate();
                        }
                    }
                }

                DebugMod.Log("Logic Thread Exit.");
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public void RegStage(SPSStageBase stage)
        {
            _stageMgr.RegisterStage(stage.StageName, stage);
        }

        public void RegGameModule(string modulename, GameModuleBase module)
        {
            _moduleMgr.RegisterModule(modulename, module);
        }

        public void RegTcpServer(TcpServerSetting setting, SPSTcpServer server)
        {
            server.ServerName = setting.serverName;
            server.Create(setting.serverIp, setting.port, setting.rcvTimeout, setting.sendTimeOut, setting.connTimeout, setting.maxPackSize);
            _svrMgr.RegisterServer(setting.serverName, server);
        }
        public void RegHttpServer(HttpServerSetting setting, SPSHttpServer server)
        {
            server.ServerName = setting.serverName;
            server.Create(setting.url, setting.port);
            _svrMgr.RegisterServer(setting.serverName, server);
        }

        public virtual void InitStages()
        {
            RegStage(new StageStartup());
            RegStage(new StageAppUpdate());
            RegStage(new StageLogin());
            RegStage(new StageSelectLine());
            RegStage(new StageRoleManage());
            RegStage(new StageGame());
        }

        public virtual void InitServers()
        {
            string serverConfigInfo = Local.Instance.CallUnityReadFileFunc("serverconfig");

            CSVMod csvMod = new CSVMod(serverConfigInfo);

			if (!csvMod.LoadCsvStr())
            {
                DebugMod.Log("服务器信息配置文件读取错误");
            }
			
            BuyServer buyserver = new BuyServer();
            HttpServerSetting buysvrsetting = new HttpServerSetting()
            {
                serverName = csvMod.GetData(0, "ServerName"),
                url = csvMod.GetData(0, "Url/ServerIP"),
                port = Int32.Parse(csvMod.GetData(0, "Port")),
            };
            RegHttpServer(buysvrsetting, buyserver);

            LineInfoServer lineinfo = new LineInfoServer();
            HttpServerSetting linesvrsetting = new HttpServerSetting()
            {
                serverName = csvMod.GetData(1, "ServerName"),
                url = csvMod.GetData(1, "Url/ServerIP"),
                port = Int32.Parse(csvMod.GetData(1, "Port")),
            };
            RegHttpServer(linesvrsetting, lineinfo);

            LoginServer loginserver = new LoginServer();
            TcpServerSetting loginsetting = new TcpServerSetting()
            {
                serverName = csvMod.GetData(2, "ServerName"),
                serverIp = csvMod.GetData(2, "Url/ServerIP"),
                port = Int32.Parse(csvMod.GetData(2, "Port")),
            };
            RegTcpServer(loginsetting, loginserver);


            GameServer gameserver = new GameServer();
            TcpServerSetting gamesvrsetting = new TcpServerSetting()
            {
                serverName = csvMod.GetData(3, "ServerName"),
                serverIp = csvMod.GetData(3, "Url/ServerIP"),
                //serverIp = "127.0.0.1",
                port = Int32.Parse(csvMod.GetData(3, "Port"))
            };
            RegTcpServer(gamesvrsetting, gameserver);
        }

        //public void RegisterCmd(string serverName, CsShare.SPSCmd ActionID, Action<ActionParam> m)
        //{
        //    _svrMgr.RegisterCmdRcver(serverName, ActionID, m);
        //}

        //public void RegisterNetEvent(string serverName, NetEventType netEventType, Action<SPSNetEventArgs> m)
        //{
        //    _svrMgr.RegisterNetEventRcver(serverName, netEventType, m);
        //}

        //初始化各个游戏模块
        public virtual void InitGameModules()
        {
            MyItemModule item = new MyItemModule();
            _moduleMgr.RegisterModule("item", item);
            OtherPlayerMove othermove = new OtherPlayerMove();
            _moduleMgr.RegisterModule("othermove", othermove);
            MonsterMove monmove = new MonsterMove();
            _moduleMgr.RegisterModule("monmove", monmove);
            MonsterAttack monatt = new MonsterAttack();
            _moduleMgr.RegisterModule("monatt", monatt);
            MyPlayerFight mpfight = new MyPlayerFight();
            _moduleMgr.RegisterModule("mpfight", mpfight);
            PlayerAttack pa = new PlayerAttack();
            _moduleMgr.RegisterModule("PlayerAttack", pa);
            MyPlayerSkill mpskill = new MyPlayerSkill();
            _moduleMgr.RegisterModule("mpskill", mpskill);
            PlayerFactory.Instance.Init(500);
            MonsterFactory.Instance.Init(500);
            HurtObjFactory.Instance.Init(500);
            SfxObjFactory.Instance.Init(500);
        }

        public bool CallLogicAction(LogicActionDefine actionType, ActionParam param)
        {
            try
            {
                DebugMod.Log("actiontype: "+actionType+"param: "+ param.ToString());
                LogicAction action = (LogicAction)LogicActionFactory.Instance.CreateAction(actionType);
                if (null == action)
                    return false;
                action.ActionId = (int)actionType;
                action.ActParam = param;

                lock (m_queueLogicAction)
                {
                    m_queueLogicAction.Enqueue(action);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        private void DequueLogicAction()
        {
            try
            {
                LogicAction action = null;
                do
                {
                    action = null;
                    lock (m_queueLogicAction)
                    {
                        if (0 < m_queueLogicAction.Count)
                            action = m_queueLogicAction.Dequeue();
                    }
                    if (null != action)
                        action.ProcessAction();
                } while (action != null);
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public void InitUnityActionCallBack(Action<UnityActionDefine, ActionParam> action)
        {
            if (null != action)
                Local.Instance.ActionCallbackUnity = action;
        }

		public void InitUnityReadFileCallBack (Func<string, string> func)
        {
            if (null != func)
            {
                Local.Instance.funcReadFileCallBackUnity = func;
            }
        }

        private void getAllDataFromUnity()
        {
            string[] fileConts = new string[20];

            try
            {
                fileConts[0] = Local.Instance.CallUnityReadFileFunc("SkillAll");
                fileConts[1] = Local.Instance.CallUnityReadFileFunc("SkillLevelUp");
                fileConts[2] = Local.Instance.CallUnityReadFileFunc("RolesInitArg");
                fileConts[3] = Local.Instance.CallUnityReadFileFunc("SkillEffect");
                fileConts[4] = Local.Instance.CallUnityReadFileFunc("SkillState");
                fileConts[5] = Local.Instance.CallUnityReadFileFunc("Monster");
                fileConts[6] = Local.Instance.CallUnityReadFileFunc("MonsterDynAttr");
                fileConts[7] = Local.Instance.CallUnityReadFileFunc("MonsterStaticAttr");
                fileConts[8] = Local.Instance.CallUnityReadFileFunc("PlayerAttr");
                ConfigManager.Instance.Init(fileConts);

                string DBdata = Local.Instance.CallUnityReadFileFunc("MyPlayerSkillDB");
                MyPlayerSkillDBManager.Instance.Init(DBdata);
            }
            catch
            {
                DebugMod.LogError("Read All Files Failed!");
            }
        }

    }
     
}
