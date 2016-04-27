using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame.Tools
{
    public class Assert
    {
        public static void Check(bool express)
        {
            if (express)
                return;

            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            System.Diagnostics.StackFrame sf = st.GetFrame(0);
            string msg = "Assert Excpetion, file:" + sf.GetFileName() + "method:" + sf.GetMethod().Name + "line:" + sf.GetFileLineNumber();
            throw new Exception(msg);
        }

        /// <summary>
        /// 取得当前源码的哪一行
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }

        /// <summary>
        /// 取当前源码的源文件名
        /// </summary>
        /// <returns></returns>
        public static string GetCurSourceFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);

            return st.GetFrame(0).GetFileName();

        }

        public static string GetMethodName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetMethod().Name;
        }
    }
}
