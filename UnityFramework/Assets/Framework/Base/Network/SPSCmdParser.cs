using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.Tools;

namespace SPSGame
{
    public class SPSCmdParser
    {
        protected Dictionary<CsShare.SPSCmd, AsynAction<ActionParam>> m_CmdRcverDic = new Dictionary<CsShare.SPSCmd, AsynAction<ActionParam>>();

        protected Dictionary<NetEventType, AsynAction<SPSNetEventArgs>> m_NetEventRcverDic = new Dictionary<NetEventType, AsynAction<SPSNetEventArgs>>();

        protected Queue<SPSNetEventArgs> m_QueueNetEvent = new Queue<SPSNetEventArgs>();
        public bool RegisterCmdRcver(CsShare.SPSCmd ActionID, Action<ActionParam> m)
        {
            try
            {
                if (m_CmdRcverDic.ContainsKey(ActionID))
                {
                    AsynAction<ActionParam> asynAction = m_CmdRcverDic[ActionID];
                    asynAction.RegFixAction(m);
                }
                else
                {
                    AsynAction<ActionParam> asynAction = new AsynAction<ActionParam>();
                    asynAction.RegFixAction(m);
                    m_CmdRcverDic[ActionID] = asynAction;
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        public void AddServerCmdNextStep(CsShare.SPSCmd ActionID, Action<ActionParam, object> m, object userData)
        {
            try
            {
                if (m_CmdRcverDic.ContainsKey(ActionID))
                {
                    AsynAction<ActionParam> asynAction = m_CmdRcverDic[ActionID];
                    asynAction.AddQueueAction(m, userData);
                }
                else
                {
                    AsynAction<ActionParam> asynAction = new AsynAction<ActionParam>();
                    asynAction.AddQueueAction(m, userData);
                    m_CmdRcverDic[ActionID] = asynAction;
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public bool RegisterNetEventRcver(NetEventType EventType, Action<SPSNetEventArgs> m)
        {
            try
            {
                if (m_NetEventRcverDic.ContainsKey(EventType))
                {
                    AsynAction<SPSNetEventArgs> asynAction = m_NetEventRcverDic[EventType];
                    asynAction.RegFixAction(m);
                }
                else
                {
                    AsynAction<SPSNetEventArgs> asynAction = new AsynAction<SPSNetEventArgs>();
                    asynAction.RegFixAction(m);
                    m_NetEventRcverDic[EventType] = asynAction;
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }

        public void AddNetEventNextStep(NetEventType EventType, Action<SPSNetEventArgs, object> m, object userData)
        {
            try
            {
                if (m_NetEventRcverDic.ContainsKey(EventType))
                {
                    AsynAction<SPSNetEventArgs> asynAction = m_NetEventRcverDic[EventType];
                    asynAction.AddQueueAction(m, userData);
                }
                else
                {
                    AsynAction<SPSNetEventArgs> asynAction = new AsynAction<SPSNetEventArgs>();
                    asynAction.AddQueueAction(m, userData);
                    m_NetEventRcverDic[EventType] = asynAction;
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
            }
        }

        public int ProcessServerCmd(CsShare.SPSCmd cmdType, ActionParam param)
        {
            try
            {
                if (m_CmdRcverDic.ContainsKey(cmdType))
                {
                    AsynAction<ActionParam> asynAction = m_CmdRcverDic[cmdType];
                    if (null != asynAction)
                    {
                        asynAction.DoFixAction(param);
                        asynAction.DoQueueAction(param);
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return 0;
            }
        }

        public int DoAllNetEvent()
        {
            try
            {
                SPSNetEventArgs args = null;
                do
                {
                    args = null;

                    lock (m_QueueNetEvent)
                    {
                        if (0 < m_QueueNetEvent.Count)
                            args = m_QueueNetEvent.Dequeue();
                    }

                    if (null == args)
                        break;

                    if (m_NetEventRcverDic.ContainsKey(args.eventType))
                    {
                        AsynAction<SPSNetEventArgs> asynAction = m_NetEventRcverDic[args.eventType];
                        if (null != asynAction)
                        {
                            asynAction.DoFixAction(args);
                            asynAction.DoQueueAction(args);
                        }
                    }
                } while (args != null);

                return 0;
            }
            catch (Exception ex)
            {
                DebugMod.LogException(ex);
                return 0;
            }
        }

        public bool ProcessNetEvent(SPSNetEventArgs args)
        {
            lock(m_QueueNetEvent)
            {
                m_QueueNetEvent.Enqueue(args);
            }
            return true;
        }
    }
}
