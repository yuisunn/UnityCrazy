using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SLCGame.Tools;
using SLCGame;

/// <summary>
/// 对unity 手动gc管理
/// </summary>
namespace SLCGame.Unity
{
    public class PoolMgr : Singleton<PoolMgr>
    {
        public void PoolInit()
        {

        }

        public GameObject NewPrefabInst(string path)
        {
            GameObject
            return 
        }
    }

        //    Dictionary<int, GObject> mObjectDic = new Dictionary<int, GObject>();
        //    public GSprite LoadSprite( int spriteid,int resid,ESpriteType type,Vector3 postion,Vector3 direction )
        //    {
        //        GSprite sprite = null;
        //        switch( type )
        //        {
        //            case ESpriteType.Player:
        //                sprite = new GPlayer();
        //                break;
        //            case ESpriteType.Monster:
        //                sprite = new GMonster();
        //                break;
        //            case ESpriteType.NPC:
        //                sprite = new GNPC();
        //                break;
        //            case ESpriteType.OtherPlayer:
        //                sprite = new GOtherPlayer();
        //                break;
        //            case ESpriteType.Ghost:
        //                sprite = new GGhost();
        //                break;

        //        }

        //        if( sprite == null )
        //        {
        //            DebugMod.LogError("fail to load sprite of id: " + spriteid + ",type:" + type);
        //            return null;
        //        }

        //        sprite.spriteType = type;
        //        sprite.direction = sprite.destDirection = direction;
        //        sprite.position = sprite.destPosition = postion;
        //        sprite.spriteID = spriteid;
        //        sprite.resID = resid;
        //        sprite.Init();
        //        mObjectDic[spriteid] = sprite;
        //        return sprite;
        //    }

        //    public GCreature LoadCreature( ECreatureType type,int creatureid )
        //    {
        //        GCreature creature = null;
        //        switch( type )
        //        {
        //            case ECreatureType.Bullet:
        //                creature = new GCreatureBullet();
        //                break;
        //            case ECreatureType.Cross:
        //                creature = new GCreatureCross();
        //                break;
        //            case ECreatureType.Hang:
        //                creature = new GCreatureHang();
        //                break;
        //            case ECreatureType.Point:
        //                creature = new GCreaturePoint();
        //                break;
        //        }
        //        mObjectDic[creatureid] = creature;
        //        return creature;
        //    }

        //    public GObject GetObject( int id )
        //    {
        //        if( mObjectDic.ContainsKey(id) )
        //        {
        //            return mObjectDic[id];
        //        }

        //        return null;
        //    }

        //    public GSprite GetSprite(int id)
        //    {
        //        if (mObjectDic.ContainsKey(id))
        //        {
        //            if (mObjectDic[id] is GSprite)
        //                return mObjectDic[id] as GSprite;
        //            else
        //                DebugMod.LogError(string.Format("Object of id {0} is now sprite", id));
        //        }

        //        return null;
        //    }

        //    public bool KillObject(int id)
        //    {
        //        if (mObjectDic.ContainsKey(id))
        //        {
        //            mObjectDic[id].Destroy();
        //            mObjectDic.Remove(id);
        //            return true;
        //        }
        //        return false;
        //    }

        //    public void ClearAllSprite()
        //    {
        //        foreach( int key in mObjectDic.Keys.ToArray() )
        //        {
        //            if(mObjectDic[key] is GSprite)
        //                KillObject(key);
        //        }
        //    }

        //    public void ClearAllCreature()
        //    {
        //        foreach (int key in mObjectDic.Keys.ToArray())
        //        {
        //            if (mObjectDic[key] is GCreature)
        //                KillObject(key);
        //        }
        //    }

        //    public void Clear()
        //    {
        //        foreach (int key in mObjectDic.Keys.ToArray())
        //        {  
        //            KillObject(key);
        //        }
        //    }

        //    public void ClearOtherSprite()
        //    {
        //        foreach (int key in mObjectDic.Keys.ToArray())
        //        {
        //            if( mObjectDic[key] is GSprite &&((GSprite)mObjectDic[key]).spriteType != ESpriteType.Player )
        //                KillObject(key);
        //        }
        //    }
        //}


    }

