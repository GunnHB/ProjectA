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

            _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameCrouch, .1f);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendCrouch, .5f);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f);
        }
    }
}