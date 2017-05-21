using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRunState : FSMState
{
    private EntityDynamicActor dyAgent = null;
    private float minDis;
    public MonsterRunState(BaseEntity agent) : base(agent)
    {
    }

    public override void setStateInfo()
    {
        this.SType = StateType.run;
    }

    public override void onEnter()
    {
        base.onEnter();
        dyAgent = this.agent as EntityDynamicActor;
        if (dyAgent != null)
        {
            dyAgent.anim.CrossFade("walk");
            dyAgent.navAgent.speed = 1;
            dyAgent.navAgent.Resume();
        }
        minDis = dyAgent.SonType == EntitySonType.first ? 0.3f : 2f;
    }

    public override void onUpdate()
    {
        if (dyAgent != null)
        {
            dyAgent.navAgent.SetDestination(dyAgent.Target.CacheTrans.position);
            if (Vector3.Distance(dyAgent.CacheTrans.position,dyAgent.Target.CacheTrans.position) < minDis)
          //if (dyAgent.navAgent.remainingDistance < minDis)
            {
                dyAgent.onChangeState(StateType.idle);
                if (dyAgent.SonType == EntitySonType.first)
                {
                    EntityStaticActor ES = dyAgent.Target as EntityStaticActor;
                    if (ES != null)
                    {
                        ES.onEnter(dyAgent);
                    }
                }
            }
        }
    }

}

