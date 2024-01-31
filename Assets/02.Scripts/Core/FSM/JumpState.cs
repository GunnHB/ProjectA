using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FSM;

public class JumpState : BaseState
{
    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        Debug.Log("점프");
        _player.DoJump();
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if (!_player.IsGrounded)
            _onAir = true;

        if (_onAir && _player.IsGrounded)
            _stateMachine.SetState(_player.ThisLandingState);
    }

    public override void OperateExit()
    {
        base.OperateExit();
    }
}
