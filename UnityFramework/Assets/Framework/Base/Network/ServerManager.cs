using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{

    public class ServerManager
    {
        protected Dictionary<string, SPSServerBase> m_ServerDic = new Dictionary<string, SPSServerBase>();
     

        public void OnStart()
        {

        }

        public SPSServerBase GetServer(string serverName)
        {
            string lowerName = serverName.ToLower();
            if (m_ServerDic.ContainsKey(lowerName))
            {
                return m_ServerDic[lowerName];
            }
            else
                return null;
        }

        public void RegisterServer(string serverName, SPSServerBase server)
        {
            string lowerName = serverName.ToLower();
            if (!m_ServerDic.ContainsKey(lowerName))
            {
                server.OnInit();
                m_ServerDic.Add(lowerName, server);
            }
        }

        public bool RegisterCmdRcver(string serverName, CsShare.SPSCmd ActionID, Action<ActionParam> m)
        {
            SPSServerBase server = GetServer(serverName);
            if (null == server)
                return false;
            return server.RegisterCmdRcver(ActionID, m);
        }

        public bool AddServerCmdNextStep(string serverName, CsShare.SPSCmd ActionID, Action<ActionParam, object> m, object userData)
        {
            SPSServerBase server = GetServer(serverName);
            if (null == server)
                return false;
            server.AddServerCmdNextStep(ActionID, m, userData);
            return true;
        }

        public bool RegisterNetEventRcver(string serverName, NetEventType type, Action<SPSNetEventArgs> m)
        {
            SPSServerBase server = GetServer(serverName);
            if (null == server)
                return false;
            return server.RegisterNetEventRcver(type, m);
        }

        public bool AddNetEventNextStep(string serverName, NetEventType type, Action<SPSNetEventArgs, object> m, object userData)
        {
            SPSServerBase server = GetServer(serverName);
            if (null == server)
                return false;
            server.AddNetEventNextStep(type, m, userData);
            return true;
        }

        public void OnUpdate()
        {
            foreach(var server in m_ServerDic)
            {
                server.Value.OnUpdate();
            }
        }

        public void OnApplicationQuit()
        {
            foreach(var server in m_ServerDic)
            {
                server.Value.OnApplicationQuit();
            }
        }
    }
}
