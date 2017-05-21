using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityMonster : EntityDynamicActor
{
    public override void onStart()
    {
        base.onStart();
        //this.CacheObj.layer = 10;
        this.CacheObj.tag = "Monster";
    }

    public override void onUpdate()
    {
        if (fsm != null) { fsm.onTick(); }
    }

    public override void onCreate(EntityInfo data)
    {
        base.onCreate(data);
        fsm = new MonsterFSM(this);
        fsm.onChangeState(StateType.idle);
    }

    public override void onDamage(float damage)
    {
        this.HP -= damage;
        this.BillBoard.setFloatByType(PartType.bloodPart, (this.HP / this.OrgHP) < 0 ? 0 : this.HP / this.OrgHP);
        if (this.HP <= 0)
        {
            onChangeState(StateType.die);
        }
        else
        {
            onChangeState(StateType.beHit);
        }
    }
    public void onChangeColor(string colorName, Color color)
    {
        this.GetComponentInChildren<MeshRenderer>().material.SetColor(colorName, color);
    }
}

