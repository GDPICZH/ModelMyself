using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    protected Dictionary<StateType, FSMState> dictStates = null;
    private FSMState currState = null;
    private FSMState nextState = null;
    protected BaseEntity agent;

    public FSM(BaseEntity agent)
    {
        this.agent = agent;
        dictStates = new Dictionary<StateType, FSMState>();
        init();
    }
    public virtual void init()
    {

    }

    public void onTick()
    {
        if (nextState != null)
        {
            if (currState != null)
            {
                currState.onExit();
            }
            currState = nextState;
            nextState = null;
            currState.onEnter();
        }
        if (currState != null)
        {
            currState.onUpdate();
        }
    }

    public void onChangeState(StateType type)
    {
        bool isCan = true;
        if (currState != null)
        {
            isCan = currState.isCanChangeTo(type);
        }
        if (isCan)
        {
            if (dictStates.ContainsKey(type))
                nextState = dictStates[type];
            else
                Debug.LogError("该FSM没有添加这种状态 stateType = " + type);
        }
    }
}
