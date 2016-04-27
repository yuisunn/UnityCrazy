using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

namespace SPSGame
{
    //创建角色
    public class CMD_Create_Character : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam || null == writer )
                return;

            writer.writeString(actionParam.Get<string>("FirstName"));
            writer.writeString(actionParam.Get<string>("LastName"));
            writer.writeShort(actionParam.Get<short>("CharClass"));
            //string stringOld = actionParam.Get<string>("LastName");

            //Encoding gbk = Encoding.GetEncoding("gb2312");

            //byte[] bytesOld = gbk.GetBytes(stringOld);
            //byte[] bytesNew = Encoding.Convert(gbk, Encoding.UTF8, bytesOld);
            //string stringNew = Encoding.UTF8.GetString(bytesNew);
            ////byte[] bytesNew = Encoding.Convert(UTF8Encoding)
            //writer.writeString(stringNew);


        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            NetDataPlayerBase newplayer = new NetDataPlayerBase();
            if (0 >= package.DataReader.Msg2NetData<NetDataPlayerBase>(newplayer))
                return null;

            ActParam["newplayer"] = newplayer;

            return ActParam;
        }

        public override bool ProcessAction()
        {
            try
            {
                //Local.Instance.CallActionFinish(LogicActionDefine.CreateChar, 0);

                NetDataPlayerBase newplayer = ActParam["newplayer"] as NetDataPlayerBase;
                if (null == newplayer)
                    return false;

                DebugMod.Log("收到新创建的角色，id=" + newplayer.CharID);

                MyPlayer myplayer = Local.Instance.GetMyPlayer;
                if (null != myplayer)
                {
                    myplayer.OnCreatePlayer(newplayer);

                    DebugMod.Log("收到新创建的角色数据，id=" + newplayer.CharID);
                }
                return true;
            }
            catch(Exception ex)
            {
                DebugMod.LogException(ex);
                return false;
            }
        }
    }
}
