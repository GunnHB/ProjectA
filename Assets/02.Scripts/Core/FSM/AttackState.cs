using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;

using UnityEngine;

public class AttackState : BaseState
{
    // private float _currTime;
    // private float _animSpeed;
    // private float _animLength;
    // private float _actualPlayTime;

    private const string TAG_ATTACK = "AttackTag";
    private AttackData _currAttackData;

    private float _prevFrameTime = 0f;

    public AttackState(PlayerController player) : base(player)
    {

    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        _player.SetIsAttacking(true);
        _player.AttackAction?.Invoke(_currAttackData);

        // Debug.Log(_player.AttackIndex);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK);

        if (normalizedTime > _prevFrameTime && normalizedTime < 1f)
        {
            if (_player.DoCombo)
                TryComboAttack(normalizedTime);
        }
        else
        {
            // back to locomotion

            if (_player.DoCombo)
                _player.SetDoCombo(false);

            _player.ResetAttackIndex();         // 여기서 걸림
        }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        if (!_player.DoCombo)
            _player.SetIsAttacking(false);
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (_player.GetAttackDataList().Last() == _currAttackData)
            return;

        if (normalizedTime < _currAttackData._comboAttackTime)
            return;

        _stateMachine.SetState(this, true);
    }

    public void SetCurrAttackData(AttackData newData)
    {
        _currAttackData = newData;
    }
}
