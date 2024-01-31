using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class WalkState : BaseState
    {
        public WalkState(PlayerController player, StateType stateType) : base(player, stateType)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            if (GetPreviousState(StateType.SPRINT))
                _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendSpeed);
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