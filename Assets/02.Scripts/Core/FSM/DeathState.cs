using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class DeathState : BaseState
    {
        public DeathState(PlayerController player, StateType stateType) : base(player, stateType)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}