using System.Collections;
using System.Collections.Generic;

using FSM;

using UnityEngine;

public class CombatState : BaseState
{
    public CombatState(PlayerController player) : base(player)
    {
        if (player == null)
            return;

        player.SheathWeaponAction = () => { SetTriggerAnimation(_player.ThisAnimData.AnimParamSheathWeapon); };
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
