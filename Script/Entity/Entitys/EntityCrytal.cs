using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCrytal : EntityStaticActor {

    public override void onStart()
    {
        base.onStart();
        this.CacheObj.layer = 12;
    }


    public override void onEnter(BaseEntity entity)
    {
        onChangeColor();
        EntityMgr.Instance.removeEntity(entity);
        Message msgdie = new Message(MsgCmd.Die_Monster_Entity, this);
        msgdie.Send();  
        onDamage(20);
        sendHpMsg();
    }

    public override void onCreate(EntityInfo data)
    {
        base.onCreate(data);
        //通知UI刷新
        sendHpMsg();
    }

    private void sendHpMsg()
    {
        Message msg = new Message(MsgCmd.On_Crystal_HP_Change, this);
        msg["id"] = this.UID;
        msg["hp"] = this.HP;
        msg["orgHP"] = this.OrgHP;
        msg.Send();
    }

    public override void onDamage(float damage)
    {
        base.onDamage(damage);
        this.HP -= damage;
        if (this.HP <= 0)
        {
            Message msgdie = new Message(MsgCmd.Die_Crystal_Entity, this);
            msgdie.Send();
            EntityMgr.Instance.removeEntity(this);
        }
    }

}
