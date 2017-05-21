using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    none = -1,
    idle,
    run,
    attack,
    beHit,
    die,
}

public abstract class FSMState
{
    protected List<StateType> allowStates = null;
    public StateType SType;
    protected BaseEntity agent;

    public FSMState(BaseEntity agent)
    {
        this.agent = agent;
        allowStates = new List<StateType>();
        setStateInfo();
    }

    //子类重写FSM类型 和 可切状态
    public abstract void setStateInfo();

    public virtual void onEnter()
    {
        Debug.Log("进入状态-->" + this.SType.ToString());
    }

    public virtual void onUpdate()
    {
    }

    public virtual void onExit()
    {
        Debug.Log("退出状态-->" + this.SType.ToString());
    }

    //是否可以切换状态
    public virtual bool isCanChangeTo(StateType type)
    {
        bool isCan = allowStates.Count > 0 ? false : true;
        if (allowStates.Contains(type))
        {
            isCan = true;
        }
        return isCan;
    }
}

