using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class WalkState : BaseState
    {
        public WalkState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            // if (GetPreviousState(_player.ThisSprintState) || GetPreviousState(_player.ThisLandingState))
            //     _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
            // else
            //     _currLengthOfVector = 0;

            if (_player.IsMoving)
                _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
            else
                _currLengthOfVector = 0f;

            _dampTarget = 1f;
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendLocomotion);
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}