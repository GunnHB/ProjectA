using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FSM;

public class DrawState : BaseState
{
    private GameValue.WeaponType _currentType;

    public DrawState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        CrossFadeInFixedUpdate(_player.ThisAnimData.AnimNameOneHandDraw01);
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
