using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class CrouchState : BaseState
    {
        // private float _maxBlendValue = .5f;

        public CrouchState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _speedAdjustments = .5f;

            _currLengthOfVector = 0f;
            // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameCrouch, .1f);
            CrossFade(_player.ThisAnimData.AnimNameCrouch);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendCrouch);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f);
            CrossFade(_player.ThisAnimData.AnimNameLocomotion);
        }
    }
}