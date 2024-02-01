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

        StartAnimation(_player.ThisAnimData.AnimParamLanding);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if (_player.ThisMoveDirection == Vector3.zero)
            _stateMachine.SetState(_player.ThisIdleState);
        else
        {
            if (_player.ReadyToSprint)
                _stateMachine.SetState(_player.ThisSprintState);
            else
                _stateMachine.SetState(_player.ThisWalkState);
        }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        StopAnimation(_player.ThisAnimData.AnimParamLanding);
    }
}
