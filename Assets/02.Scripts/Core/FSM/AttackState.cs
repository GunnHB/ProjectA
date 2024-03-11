using System.Linq;

using FSM;
using UnityEngine;

public class AttackState : BaseState
{
    private const string TAG_ATTACK = "Tag_Attack";
    private AttackData _currAttackData;

    private int _layerIndex = 0;
    private float _exitTime = .75f;

    public AttackState(PlayerController player) : base(player)
    {

    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        _layerIndex = GetLayerIndex();

        _player.SetIsAttacking(true);
        _player.ThisAnimator.CrossFadeInFixedTime(_currAttackData._attackAnimHash, _currAttackData._transitionDuration, _layerIndex);
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK, _layerIndex);

        if (normalizedTime > _exitTime)
        {
            if (_player.AttackQueue.Count > 0)
                _player.AttackAction?.Invoke(_player.AttackQueue.Dequeue());
            else
            {
                _player.ResetAttackIndex();

                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, _layerIndex);
                _player.IdleAction?.Invoke();
            }
        }

        // if (normalizedTime > _prevFrameTime && normalizedTime < _exitTime)
        // {
        //     if (!_startAttackAnim)
        //         _startAttackAnim = true;

        //     if (_player.DoCombo)
        //         TryComboAttack(normalizedTime);
        // }
        // else if (_startAttackAnim)
        // {
        //     // back to locomotion
        //     _stateMachine.SetState(_player.ThisIdleState);

        //     if (_player.DoCombo)
        //         _player.SetDoCombo(false);

        //     _player.ResetAttackIndex();         // 여기서 걸림
        //     // _startAttackAnim = false;
        // }

        // _prevFrameTime = normalizedTime;
    }

    public override void OperateExit()
    {
        base.OperateExit();

        // if (!_player.DoCombo)
        // _player.SetDoCombo(false);
        _player.SetIsAttacking(false);
    }

    // private void TryComboAttack(float normalizedTime)
    // {
    //     if (_player.GetAttackDataList().Last() == _currAttackData)
    //         return;

    //     if (normalizedTime < _currAttackData._comboAttackTime)
    //         return;

    //     _stateMachine.SetState(this, true);
    // }

    public void SetCurrAttackData(AttackData newData)
    {
        _currAttackData = newData;
    }
}
