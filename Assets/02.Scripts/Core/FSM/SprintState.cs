using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SprintState : BaseState
    {
        public SprintState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _dampTarget = 1.5f;

            if (GetPreviousState(_player.ThisWalkState) || GetPreviousState(_player.ThisLandingState))
                _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
            else
                _currLengthOfVector = 0f;
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