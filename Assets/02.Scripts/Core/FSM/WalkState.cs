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

            if (GetPreviousState(_player.ThisSprintState) || GetPreviousState(_player.ThisLandingState))
                _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
            else
                _currLengthOfVector = 0;

            _dampTarget = 1f;
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetPlayerMovement();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}