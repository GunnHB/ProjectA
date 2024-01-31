using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SprintState : BaseState
    {
        public SprintState(PlayerController player, StateType stateType) : base(player, stateType)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _dampTarget = 1.5f;

            if (GetPreviousState(StateType.WALK))
                _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendSpeed);
            else
                _currLengthOfVector = 0f;
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