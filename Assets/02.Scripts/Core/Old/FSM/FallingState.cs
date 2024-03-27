using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class FallingState : BaseState
    {
        public FallingState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameFalling, .1f);
            CrossFade(_player.ThisAnimData.AnimNameFalling);

            // if (_player.IsGrounded)
            // {
            //     _stateMachine.SwitchState(_player.ThisLandingState);
            //     return;
            // }

            // StartAnimation(_player.ThisAnimData.AnimParamFalling);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (_player.IsGrounded)
                _player.LandingAction?.Invoke();

            // if (_player.IsGrounded)
            //     _stateMachine.SwitchState(_player.ThisLandingState);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            // StopAnimation(_player.ThisAnimData.AnimParamFalling);
        }
    }
}