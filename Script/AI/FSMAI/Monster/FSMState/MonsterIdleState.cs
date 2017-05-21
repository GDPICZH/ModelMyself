using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : FSMState
{
    private float serchTime = 0.2f;
    private float minDis;

    public MonsterIdleState(BaseEntity agent) : base(agent)
    {
    }


    public override void setStateInfo()
    {
        this.SType = StateType.idle;
    }

    public override void onEnter()
    {
        base.onEnter();
        EntityDynamicActor dyAgent = this.agent as EntityDynamicActor;
        if (dyAgent != null)
        {
            dyAgent.anim.CrossFade("idle" ,.1f);
        }
        minDis = dyAgent.SonType == EntitySonType.first ? 0.3f : 2f;
    }

    public override void onUpdate()
    {
        serchTime -= Time.deltaTime;
        if (serchTime < 0)
        {
            serchEnemy(0);
            serchTime = .2f;
        }
    }

    public override void onExit()
    {
        base.onExit();
        serchTime = 0.2f;
    }

    //查找敌人
    private void serchEnemy(float dis)
    {
        EntityDynamicActor dyAgent = this.agent as EntityDynamicActor;
        if (dyAgent != null)
        {
            if (dyAgent.Target == null)
            {
                EntityType type = dyAgent.SonType == EntitySonType.first ? EntityType.staticActor : EntityType.player;
                List<BaseEntity> lst = EntityMgr.Instance.getEntityByType(type);
                if (lst != null && lst.Count > 0)
                {
                    dyAgent.Target = lst[0];                
                }
            }
            if (dyAgent.Target != null)
            {
                bool isReach = Vector3.Distance(dyAgent.CacheTrans.position, dyAgent.Target.CacheTrans.position) < minDis;
                if (!isReach)
                {
                    dyAgent.onChangeState(StateType.run);
                }
                else
                {
                    dyAgent.onChangeState(StateType.attack);
                    if (dyAgent.SonType == EntitySonType.second)
                    {
                        //dyAgent.onReleaseSkill(1001);
                    }
                }
            }
        }
    }

}

