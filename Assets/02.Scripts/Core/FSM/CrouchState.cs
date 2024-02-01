using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FSM;

public class CrouchState : BaseState
{
    public CrouchState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        _currLengthOfVector = 0f;
        StartAnimation(_player.ThisAnimData.AnimParamCrouch);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if (_player.ThisMoveDirection != Vector3.zero)
        {
            _dampTarget = 1f;
            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendCrouch);
        }
        else
            _dampTarget = .5f;
    }

    public override void OperateExit()
    {
        base.OperateExit();

        SetFloatParam(_player.ThisAnimData.AnimParamBlendCrouch, 1f);
        StopAnimation(_player.ThisAnimData.AnimParamCrouch);
    }
}
