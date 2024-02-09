using System.Collections;
using System.Collections.Generic;

using FSM;

using UnityEngine;

public class AttackState : BaseState
{
    private float _currTime;
    private float _animSpeed;
    private float _animLength;
    private float _actualPlayTime;

    public AttackState(PlayerController player) : base(player)
    {

    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        SetTriggerAnimation(_player.ThisAnimData.AnimParamAttack);

        _animSpeed = GetCurrentClipSpeed(0);
        _animLength = GetCurrentClipLength(0);

        _currTime = 0f;
        _actualPlayTime = _animLength / _animSpeed;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        _currTime += Time.deltaTime;

        if (_player.DoCombo)
        {
            _stateMachine.SetState(this);
            _player.SetDoCombo(false);
        }
        else if (_currTime >= _actualPlayTime)
            SetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion, .01f);
    }

    public override void OperateExit()
    {
        base.OperateExit();

        _currTime = 0f;
        _animSpeed = 0f;
        _animLength = 0f;
    }
}
