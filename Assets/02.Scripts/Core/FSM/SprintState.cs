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

            // StartAnimation(_player.ThisAnimData.AnimParamSprint);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();
        }

        public override void OperateExit()
        {
            base.OperateExit();

            // StopAnimation(_player.ThisAnimData.AnimParamSprint);
        }
    }
}