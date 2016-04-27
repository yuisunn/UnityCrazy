using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPSGame.GameModule;
using SPSGame.Tools;
using SPSGame.CsShare;
using SPSGame.CsShare.Data;


namespace SPSGame
{
    public class CMD_CS_BeginSingSkill : NetAction
    {
        public override void SendParameterTcp(NetWriterTcp writer, ActionParam actionParam)
        {
            if (null == actionParam)
                return;

            long seq = (long)actionParam["target"];
            int skillid = (int)actionParam["SkillID"];
            short px = (short)actionParam["PlayerX"];
            short pz = (short)actionParam["PlayerZ"];
            short mx = (short)actionParam["MonsterX"];
            short mz = (short)actionParam["MonsterZ"];
            short isplayer = (short)actionParam["isplayer"];
            writer.writeLong(seq);
            writer.writeShort(isplayer);
            writer.writeInt32(skillid);
            writer.writeShort(px);
            writer.writeShort(pz);
            writer.writeShort(mx);
            writer.writeShort(mz);
        }

        public override ActionParam DecodePackage(NetPackage package)
        {
            if (null == ActParam || null == package)
                return null;
            if (null == package.DataReader)
                return null;

            SingSkillParam param = new SingSkillParam();
            package.DataReader.Msg2NetData<SingSkillParam>(param);

            PlayerAttack pa = Local.Instance.GetModule("PlayerAttack") as PlayerAttack;
            if (null != pa)
                pa.OnServerSingSkill(param.charid, param);
            return ActParam;
        }

        //public override bool ProcessAction()
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        DebugMod.LogException(ex);
        //        return false;
        //    }
        //}
    }
}
