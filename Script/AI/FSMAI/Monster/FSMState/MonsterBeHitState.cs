﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBeHitState : FSMState
{
    private float resetTime = .5f;

    public MonsterBeHitState(BaseEntity agent) : base(agent)
    {
    }

    public override void setStateInfo()
    {
        this.SType = StateType.beHit;
        this.allowStates.Add(StateType.idle);
    }

    public override void onEnter()
    {
        base.onEnter();
        EntityDynamicActor dyAgent = this.agent as EntityDynamicActor;
        if (dyAgent != null)
        {
            int num = UnityEngine.Random.Range(1, 3);
            dyAgent.anim.CrossFade("hit" + num, .1f);
            dyAgent.navAgent.Stop();
        }
    }

    public override void onUpdate()
    {
        resetTime -= Time.deltaTime;
        if (resetTime < 0)
        {
            EntityDynamicActor dyAgent = this.agent as EntityDynamicActor;
            if (dyAgent != null)
            {
                dyAgent.onChangeState(StateType.idle);
            }
        }
    }

    public override void onExit()
    {
        base.onExit();
        resetTime = .5f;
    }

}

