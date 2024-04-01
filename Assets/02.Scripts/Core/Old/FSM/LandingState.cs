using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FSM;

public class LandingState : BaseState
{
    public UnityEngine.Events.UnityAction ExitAction;

    public LandingState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLanding, .1f);
        CrossFade(_player.ThisAnimData.AnimNameLanding);
        ExitAction += () => { CrossFade(_player.ThisAnimData.AnimNameLocomotion); };

        // StartAnimation(_player.ThisAnimData.AnimParamLanding);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        // if (!_player.IsMoving)
        //     _stateMachine.SwitchState(_player.ThisIdleState);
        // else
        // {
        //     if (_player.ReadyToSprint)
        //         _stateMachine.SwitchState(_player.ThisSprintState);
        // }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        // StopAnimation(_player.ThisAnimData.AnimParamLanding);
    }
}