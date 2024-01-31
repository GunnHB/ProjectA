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

            _currLengthOfVector = 0;
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