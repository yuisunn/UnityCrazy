using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLCGame.Tools;
using System.IO;
using System;

namespace SLCGame.Unity
{
    /// <summary>
    /// 缺少断点重传和 重下 磁盘空间判断
    /// </summary>
    public class HotUpdateMgr : Singleton<HotUpdateMgr>
    {
        private List<string> m_DownloadFiles = new List<string>();

        void Initialize()
        {

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void CheckExtractResource()
        {
            bool isExists = Directory.Exists(PathMod.DataPath) &&
              Directory.Exists(PathMod.DataPath + AppConst.LuaFolder) && File.Exists(PathMod.DataPath + AppConst.FilesName);
            if (isExists || AppConst.DebugMode)
            {
                CoroutineMgr.Instance.StartCoroutine(OnUpdateResource());
                return;
            }
            //CoroutineMgr.Instance.StartCoroutine(OnExtractResource());    //启动释放协成 
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited()
        {
            GameMain.Instance.FileCkeckEnd();
        }
        /// <summary>
        /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
        /// </summary>
        IEnumerator OnUpdateResource()
        {
            m_DownloadFiles.Clear();
            m_CurDownloadNum = 0;
            if (!AppConst.UpdateMode)
            {
                OnResourceInited();
                yield break;
            }

            string dataPath = PathMod.DataPath;  //数据目录
            string url = AppConst.WebUrl;
            //string random = DateTime.Now.ToString("yyyymmddhhmmss");
            string listUrl = url + "files.txt";//?v=" + random;
            Debug.LogWarning("LoadUpdate---->>>" + listUrl);

            //下载文件列表
            WWW www = new WWW(listUrl); yield return www;
            if (www.error != null)
            {
                OnUpdateFailed(string.Empty);
                yield break;
            }
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            File.WriteAllBytes(dataPath + "files.txt", www.bytes);

            string filesText = www.text;

            string[] files = filesText.Split('\n');
            string ver = "";
            if (files.Length > 0)
                ver = files[0];

            string message = string.Empty;
            for (int i = 0; i < files.Length; i++)
            {
                if (string.IsNullOrEmpty(files[i]))
                    continue;
                string[] keyValue = files[i].Split('|');
                string f = keyValue[0];
                string localfile = (dataPath + f).Trim();
                string path = Path.GetDirectoryName(localfile);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileUrl = url + keyValue[0];// + "?v=" + random;
                bool canUpdate = !File.Exists(localfile);
                //如果文件存在 md5不一样就重新下载
                if (!canUpdate)
                {
                    string remoteMd5 = keyValue[1].Trim();
                    string localMd5 = Util.md5file(localfile);
                    canUpdate = !remoteMd5.Equals(localMd5);
                    if (canUpdate) File.Delete(localfile);
                }
                //本地缺少文件
                if (canUpdate)
                {
                    DebugMod.Log(fileUrl);
                    message = "downloading>>" + fileUrl;
                    //这里都是资源文件，用线程下载
                    while (m_CurDownloadNum > m_MaxDownloadNum)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    BeginDownload(fileUrl, localfile); 
                    //检查下载文件列表
                    while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }
                }
            }
            yield return new WaitForEndOfFrame();
            message = "更新完成!!";
            //facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            //GameMain.GlobalObject.SendMessage("OnShowMessage", message);
            //AssetManager.Instance.Initialize(OnResourceInited);
            OnResourceInited();
        }
        public int m_MaxDownloadNum = 3;
        public int m_CurDownloadNum;


        IEnumerator OnExtractResource()
        {
            string dataPath = PathMod.DataPath;  //数据目录
            string resPath = PathMod.AppContentPath(); //游戏包资源目录

            if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
            Directory.CreateDirectory(dataPath);

            string infile = resPath + "files.txt";
            string outfile = dataPath + "files.txt";
            if (File.Exists(outfile)) File.Delete(outfile);

            string message = "正在解包文件:>files.txt";
            //Debug.Log(message);
            ////facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            //MainBehaviour.GlobalObject.SendMessage("OnShowMessage", message);

            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    WWW www = new WWW(infile);
            //    yield return www;

            //    if (www.isDone)
            //    {
            //        File.WriteAllBytes(outfile, www.bytes);
            //    }
            //    yield return 0;
            //}
            //else File.Copy(infile, outfile, true);
            //yield return new WaitForEndOfFrame();

            ////释放所有文件到数据目录
            //string[] files = File.ReadAllLines(outfile);
            //foreach (var file in files)
            //{
            //    string[] fs = file.Split('|');
            //    infile = resPath + fs[0];  //
            //    outfile = dataPath + fs[0];

            //    message = "正在解包文件:>" + fs[0];
            //    Debug.Log("正在解包文件:>" + infile);
            //    //facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            //    GameMain.GlobalObject.SendMessage("OnShowMessage", message);
                
            //    string dir = Path.GetDirectoryName(outfile);
            //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            //    if (Application.platform == RuntimePlatform.Android)
            //    {
            //        WWW www = new WWW(infile);
            //        yield return www;

            //        if (www.isDone)
            //        {
            //            File.WriteAllBytes(outfile, www.bytes);
            //        }
            //        yield return 0;
            //    }
            //    else
            //    {
            //        if (File.Exists(outfile))
            //        {
            //            File.Delete(outfile);
            //        }
            //        File.Copy(infile, outfile, true);
            //    }
            //    yield return new WaitForEndOfFrame();
            //}
            //message = "解包完成!!!";
            ////facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            //GameMain.GlobalObject.SendMessage("OnShowMessage", message);

            yield return new WaitForSeconds(0.1f);
            message = string.Empty;

            //释放完成，开始启动更新资源
            GameMain.Instance.StartCoroutine(OnUpdateResource());
        }

        public void OnResourceInitEnd()
        {
            //GameMain.GlobalObject.SendMessage("OnHotUpdateFinished", true);
        }

        void OnUpdateFailed(string file)
        {
            string message = "更新失败!>" + file;
            //GameMain.GlobalObject.SendMessage("OnShowMessage", message);
            //GameMain.GlobalObject.SendMessage("OnHotUpdateFinished", false);
        }
        /// <summary>
        /// 重新下载
        /// </summary>
        void OnDownAgain()
        {

        }
        void OnDownloadFileFinish()
        {

        }
        /// <summary>
        /// 是否下载完成
        /// </summary>
        bool IsDownOK(string file)
        {
            return m_DownloadFiles.Contains(file);
        }

        void BeginDownload(string url, string file)
        {
            m_CurDownloadNum++;
            ThreadMgr.RunAsync(new TheardEvent(
                () => {
                    HttpDownFile down = new HttpDownFile(file,url);
                    down.DownloadFile();
                }
                , () => {
                    m_CurDownloadNum--; 
                    m_DownloadFiles.Add(file);
                }
                ));

        }
    }

}