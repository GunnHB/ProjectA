using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class JumpState : BaseState
    {
        // 점프 시작했는지 확인하기 위한 플래그
        bool _jumpStarted = false;

        public JumpState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _player.DoJump();
            // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameJump, .1f);
            CrossFade(_player.ThisAnimData.AnimNameJump);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (!_player.IsGrounded)
                _jumpStarted = true;

            if (_player.IsPeak)
                _player.FallingAction?.Invoke();
            else if (_jumpStarted && _player.IsGrounded)
                _player.LandingAction?.Invoke();

            // if (!_player.IsGrounded)
            //     _isjumped = true;

            // if (_player.IsPeak)
            //     _stateMachine.SwitchState(_player.ThisFallingState);
            // else if (_isjumped && _player.IsGrounded)
            //     _stateMachine.SwitchState(_player.ThisLandingState);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _jumpStarted = false;

            // _isjumped = false;
            // StopAnimation(_player.ThisAnimData.AnimParamJump);
        }
    }
}