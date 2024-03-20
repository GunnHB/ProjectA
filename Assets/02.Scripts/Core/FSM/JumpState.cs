using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class JumpState : BaseState
    {
        bool _isjumped = false;

        public JumpState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            // StartAnimation(_player.ThisAnimData.AnimParamJump);

            _player.DoJump();
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (!_player.IsGrounded)
                _isjumped = true;

            if (_player.IsPeak)
                _stateMachine.SwitchState(_player.ThisFallingState);
            else if (_isjumped && _player.IsGrounded)
                _stateMachine.SwitchState(_player.ThisLandingState);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _isjumped = false;
            StopAnimation(_player.ThisAnimData.AnimParamJump);
        }
    }
}