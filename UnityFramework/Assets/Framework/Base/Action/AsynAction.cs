using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame
{
    public class AsynAction<T>
    {
        public class AsynActionData
        {
            public object _userData;
            public Action<T, object> _action;
        };

        protected Queue<AsynActionData> m_QueueActionCallback = new Queue<AsynActionData>();
        protected event Action<T> m_FixActionCallback;

        public void RegFixAction(Action<T> action)
        {
            m_FixActionCallback += action;
        }

        public void UnRegFixAction(Action<T> action)
        {
            m_FixActionCallback -= action;
        }

        public void DoFixAction(T arg)
        {
            if (null != m_FixActionCallback)
                m_FixActionCallback(arg);
        }

        public void AddQueueAction(Action<T, object> action, object userData)
        {
            AsynActionData qAction = new AsynActionData()
            {
                _userData = userData,
                _action = action,
            };

            lock (m_QueueActionCallback)
                m_QueueActionCallback.Enqueue(qAction);
        }

        public void DoQueueAction(T arg)
        {
            AsynActionData qAction = null;
            lock (m_QueueActionCallback)
            {
                if (0 < m_QueueActionCallback.Count)
                    qAction = m_QueueActionCallback.Dequeue();
            }
            if (null != qAction)
            {
                qAction._action(arg, qAction._userData);
            }
        }
    }
}
