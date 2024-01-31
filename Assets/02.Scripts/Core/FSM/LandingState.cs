using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FSM;

public class LandingState : BaseState
{
    public LandingState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        Debug.Log("다쓰요");
        _onAir = false;
        _stateMachine.SetState(_player.ThisIdleState);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();
    }

    public override void OperateExit()
    {
        base.OperateExit();
    }
}
