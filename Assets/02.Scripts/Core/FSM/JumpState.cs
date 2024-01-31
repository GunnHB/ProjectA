using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FSM;

public class JumpState : BaseState
{
    public JumpState(PlayerController player, StateType stateType) : base(player, stateType)
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
