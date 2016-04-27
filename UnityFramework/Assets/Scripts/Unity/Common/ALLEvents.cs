using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SPSGame.Unity
{
    public class EventLoadScene:EventArgs
    {
        public int sceneID;
        public int heroResID;
        public EventLoadScene( int sceneid,int resid )
        {
            sceneID = sceneid;
            heroResID = resid;
        }
    }


    public class EventLoadCreatureBullet:EventArgs
    {
        public int creatureID;
        public int resID;
        public int spriteID;
        public int targetSpriteID;
        public float moveSpeed;
        public float lifeMax;

        public EventLoadCreatureBullet(int creatureid, int resid, int spriteid, int targetspriteid, float speed, float life)
        {
            creatureID = creatureid;
            resID = resid;
            spriteID = spriteid;
            targetSpriteID = targetspriteid;
            moveSpeed = speed;
            lifeMax = life;
        }
    }

    public class EventKillCreature:EventArgs
    {
        public int creatureID;
        public EventKillCreature( int creatureid )
        {
            creatureID = creatureid;
        }
    }

    public class EventLoadCreatureCross : EventArgs
    {
        public int creatureID;
        public int resID;
        public int spriteID;
        public Vector3 direction;
        public float speed;
        public float lifeMax;
        public bool hitOnce;
        public EventLoadCreatureCross(int creatureid, int resid,int spriteid,Vector3 direct, float _speed, float _life,bool hitonce)
        {
            creatureID = creatureid;
            resID = resid;
            spriteID = spriteid;
            direction = direct;
            speed = _speed;
            lifeMax = _life;
            hitOnce = hitonce;
        }
    }

    public class EventLoadCreatureHang : EventArgs
    {
        public int creatureID;
        public int resID;
        public int spriteID;
        public float life;

        public EventLoadCreatureHang(int creatureid, int resid,int spriteid, float _life)
        {
            creatureID = creatureid;
            resID = resid;
            spriteID = spriteid;
            life = _life;
        }
    }
   
    public class EventLoadCreaturePoint : EventArgs
    {
        public int creatureID;
        public int resID;
        public Vector3 pos;
        public float life;

        public EventLoadCreaturePoint(int creatureid, int resid,Vector3 _pos, float _life)
        {
            creatureID = creatureid;
            resID = resid;
            pos = _pos;
            life = _life;
        }
    }

    /// <summary>
    /// 背包信息的事件
    /// </summary>
    public class EventPackInfo : EventArgs
    {
        public short type;
        public List<Dictionary<string, string>> info;
        public EventPackInfo(short _type, List<Dictionary<string, string>> _info)
        {
            type = _type;
            info = _info;
        }
    }

    public class EventItemInfo : EventArgs 
    {
        public Dictionary<string, string> info;
        public EventItemInfo(Dictionary<string,string> _info) 
        {
            info = _info;
        }
    }

	/// <summary>
    /// 人物基础属性
    /// </summary>
    public class EventRoleBaseProperty : EventArgs
    {
        public int forceLab;
        public int speedLab;
        public int witLab;
        public int forceUpLab;
        public int speedUpLab;
        public int witUpLab;

        public EventRoleBaseProperty (int _forceLab, int _speedLab, int _witLab, int _forceUpLab, int _speedUpLab, int _witUpLab)
        {
            forceLab = _forceLab;
            speedLab = _speedLab;
            witLab = _witLab;
            forceUpLab = _forceUpLab;
            speedUpLab = _speedUpLab;
            witUpLab = _witUpLab;
        }
    }

    /// <summary>
    /// 人物详细属性
    /// </summary>
    public class EventRoleDetailProperty : EventArgs
    {
        public int maxHpLab;
        public int phyATKLab;
        public int magicATKLab;
        public int physicProtectedLab;
        public int magicProtectLab;
        public int physicCritLab;
        public int critHitLab;

        public EventRoleDetailProperty(int _maxHpLab, int _phyATKLab, int _magicATKLab, int _physicProtectedLab, int _magicProtectLab, int _physicCritLab, int _critHitLab)
        {
            maxHpLab = _maxHpLab;
            phyATKLab = _phyATKLab;
            magicATKLab = _magicProtectLab;
            physicProtectedLab = _physicProtectedLab;
            magicProtectLab = _magicProtectLab;
            physicCritLab = _physicCritLab;
            critHitLab = _critHitLab;
        }
    }

    /// <summary>
    /// 用于展示角色的装备
    /// </summary>
    public class EventShowPLayerEquipInfo : EventArgs
    {
        public List<Dictionary<string, string>> info;

        public EventShowPLayerEquipInfo(List<Dictionary<string, string>> _info)
        {
            info = _info;
        }
    }

    /// <summary>
    /// 用于展示可替换的装备
    /// </summary>
    public class EventShowChooseEquipInfo : EventArgs
    {
        public List<Dictionary<string, string>> info;

        public EventShowChooseEquipInfo (List<Dictionary<string, string>> _info)
        {
            info = _info;
        }
    }
	
    public class EventLoadSprite:EventArgs
    {
        public int spriteid;
        public int resid;
        public int type;
        public Vector3 postion;
        public Vector3 direction;
        public bool isShow;
        public EventLoadSprite( int sprid,int res,int _type,Vector3 pos,Vector3 dir,bool isshow = true )
        {
            spriteid = sprid;
            resid = res;
            type = _type;
            postion = pos;
            direction = dir;
            isShow = isshow;
        }
    }

    public class EventMoveSprite:EventArgs
    {
        public int spriteID;       
        public Vector3 position;
        public Vector3 direction;
        public float speed;

        public EventMoveSprite( int id,Vector3 pos,Vector3 dir,float _speed )
        {
            spriteID = id;
            position = pos;
            direction = dir;
            speed = _speed;
        }
    }

    public class EventLocatSprite:EventArgs
    {
        public int spriteID;       
        public Vector3 position;
        public Vector3 direction;

        public EventLocatSprite(int id, Vector3 pos, Vector3 dir)
        {
            spriteID = id;
            position = pos;
            direction = dir;
        }
    }

    public class EventShowSprite:EventArgs
    {
        public int spriteID;
        public bool isShow;
        public EventShowSprite( int id,bool show )
        {
            spriteID = id;
            isShow = show;
        }
    }

    public class EventShowHud:EventArgs
    {
        public int spriteID;
        public bool isShow;
        public EventShowHud(int id, bool show)
        {
            spriteID = id;
            isShow = show;
        }
    }

    public class EventPlayerControlAble:EventArgs
    {
        public bool controlAble = false;
        public EventPlayerControlAble(bool _controlable)
        {
            controlAble = _controlable;
        }
    }

    public class EventPlayerControlSpeed : EventArgs
    {
        public float controlSpeed = -1;
        public EventPlayerControlSpeed(float _controlspeed)
        {
            controlSpeed = _controlspeed;
        }
    }

    public class EventSendLineState:EventArgs
    {
        public int[] linestates;

        public EventSendLineState(int[] _states)
        {
            linestates = _states;
        }
    }

    public class EventActSprite:EventArgs
    {
        public int spriteID;
        public int actionID;
        public float speed;
        public EventActSprite( int id,int act,float _speed )
        {
            spriteID = id;
            actionID = act;
            speed = _speed;
        }
    }

    public class EventKillSprite:EventArgs
    {
        public int spriteID;
        public EventKillSprite(int id)
        {
            spriteID = id;
        }
    }

    public class EventHurtSprite : EventArgs
    {
        public int spriteID;
        public EHurtType hurtType;
        public int hurtNumber;
        public EventHurtSprite(int id,EHurtType type,int hurtnumber)
        {
            spriteID = id;
            hurtType = type;
            hurtNumber = hurtnumber;
        }
    }

    public class EventSetSpriteInfo : EventArgs
    {
        public int spriteID;
        public string spriteName;
        public int spriteLevel;
        public int maxHealth;
        public int currentHealth;
        public EventSetSpriteInfo(int id, string name,int level,int maxhealth,int currenthealth )
        {
            spriteID = id;
            spriteName = name;
            spriteLevel = level;
            maxHealth = maxhealth;
            currentHealth = currenthealth;
        }
    }


    public class EventButtonEvent:EventArgs
    {
        public ButtonEventType buttonEvent;
        public EventButtonEvent( ButtonEventType eventtype )
        {
            buttonEvent = eventtype;
        }
    
    }

    public class EventWndDestroy : EventArgs
    {

    }

    public class EventWndSelected : EventArgs
    {

    }

    public class EventEndStage : EventArgs
    {

    }



    public class EventSendChar:EventArgs
    {
        public long charID;
        public string firstName;
        public string lastName;
        public int charClass;
        public int charGrage;
        public int vipLevel;
        public int charLevel;
        public int mapID;

        public EventSendChar(long _charid, string _firstname, string _lastname, int _charclass,int _chargrade, int _viplevel, int _charlevel, int _mapid)
        {
            charID = _charid;
            firstName = _firstname;
            lastName = _lastname;
            charClass = _charclass;
            charGrage = _chargrade;
            vipLevel = _viplevel;
            charLevel = _charlevel;
            mapID = _mapid;
        }
    }

    public class EventDeleteChar : EventArgs
    {
        public long charID;
        public int charClass;

        public EventDeleteChar(long _charid, int _charclass)
        {
            charID = _charid;
            charClass = _charclass;
        }
    }

    //public class Event

    public class EventLoadSceneStart:EventArgs
    {
        public string sceneName = "";
        public EventLoadSceneStart(string scenename)
        {
            sceneName = scenename;
        }
    }

    public class EventLoadScaneFinish:EventArgs
    {
        public string sceneName = "";
        public EventLoadScaneFinish( string scenename )
        {
            sceneName = scenename;
        }
    }


    public class EventGameStageEnter:EventArgs
    {
        public EGameStage gameStage;
        public EventGameStageEnter( EGameStage stage)
        {
            gameStage = stage;
        }
    }

    public class EventGameStageLeave:EventArgs
    {
        public EGameStage gameStage;
        public EventGameStageLeave(EGameStage stage)
        {
            gameStage = stage;
        }
    }

    public class EventActionFinish:EventArgs
    {
        public LogicActionDefine action;
        public EventActionFinish(LogicActionDefine e)
        {
            action = e;
        }
    }

    /// <summary>
    /// 技能信息
    /// </summary>
    public class EventLeanSkill : EventArgs
    {
        public List<LocalDataToC> listSkill;
        public Dictionary<int ,int> dicSkill;

        public EventLeanSkill(List<LocalDataToC> listdic,Dictionary<int ,int> diclean )
        {
            listSkill = listdic;
            dicSkill = diclean;
        }
    }


    /// <summary>
    /// 技能学习
    /// </summary>
    public class EventStudySkill : EventArgs
    {
        public int studySkill;
        public int idSkill;
        public EventStudySkill(int dic, int id)
        {
            studySkill = dic;
            idSkill = id;
        }
    }

    /// <summary>
    /// 技能升级
    /// </summary>
    public class EventUpSkill : EventArgs
    {
        public LocalDataToC dicUpSkill;
        public int errStu;
        public EventUpSkill(LocalDataToC dic, int error)
        {
            dicUpSkill = dic;
            errStu = error;
        }
    }

    public class EventChosenSkill : EventArgs
    {
        public Dictionary<int, ChosenSkillData> chonseSkill;
        public EventChosenSkill(Dictionary<int, ChosenSkillData> chonse)
        {
            chonseSkill = chonse;
        }
    }

    public class EventChosenOneSkill : EventArgs
    {
        public Dictionary<int, ChosenSkillData> chonseoneSkill;
        public EventChosenOneSkill(Dictionary<int, ChosenSkillData> chonse)
        {
            chonseoneSkill = chonse;
        }
    }

    public class EventToDeathOrReborn : EventArgs
    {
        public string stage;
        public EventToDeathOrReborn(string sta)
        {
            stage = sta;
        }
    }


}