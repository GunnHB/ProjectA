using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;

using UnityEngine;

public class AttackState : BaseState
{
    private const string TAG_ATTACK = "AttackTag";
    private AttackData _currAttackData;

    private float _prevFrameTime;
    private bool _startAttackAnim = false;

    public AttackState(PlayerController player) : base(player)
    {

    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        InitDatas();

        _player.SetIsAttacking(true);
        CrossFadeInFixedUpdate(_currAttackData._attackAnimHash, _currAttackData._transitionDuration);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK);

        if (normalizedTime > _prevFrameTime && normalizedTime < 1f)
        {
            if (!_startAttackAnim)
                _startAttackAnim = true;

            if (_player.DoCombo)
                TryComboAttack(normalizedTime);
        }
        else if (_startAttackAnim)
        {
            // back to locomotion
            _stateMachine.SetState(_player.ThisIdleState);

            if (_player.DoCombo)
                _player.SetDoCombo(false);

            _player.ResetAttackIndex();         // 여기서 걸림
            // _startAttackAnim = false;
        }

        _prevFrameTime = normalizedTime;
    }

    public override void OperateExit()
    {
        base.OperateExit();

        _startAttackAnim = false;
        // if (!_player.DoCombo)
        _player.SetDoCombo(false);
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

    private void InitDatas()
    {
        _prevFrameTime = 0f;
        _startAttackAnim = false;
    }
}
