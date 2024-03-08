using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FSM;

public class DrawState : BaseState
{
    private const string TAG_DRAW = "Tag_Draw";

    private GameValue.WeaponType _currentType = GameValue.WeaponType.None;

    private bool _doNext = false;
    private bool _drawDone = false;

    private float _exitTime = 1f;

    public DrawState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        SetWeaponType(ref _currentType);

        if (_currentType != GameValue.WeaponType.None)
        {
            int animHash = _doNext ? _player.ThisAnimData.AnimNameDraw02 : _player.ThisAnimData.AnimNameDraw01;

            _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, (int)_currentType);
        }
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        var currentInfo = GetCurrentAnimatorStateInfo((int)_currentType);
        var nextInfo = GetNextAniomatorStateInfo((int)_currentType);

        float normalizedTime = float.MinValue;

        if (_player.ThisAnimator.IsInTransition((int)_currentType) && nextInfo.IsTag(TAG_DRAW))
            normalizedTime = nextInfo.normalizedTime;
        else if (!_player.ThisAnimator.IsInTransition((int)_currentType) && currentInfo.IsTag(TAG_DRAW))
            normalizedTime = currentInfo.normalizedTime;

        if (normalizedTime >= _exitTime)
        {
            if (_doNext)
            {
                _drawDone = true;

                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, (int)_currentType);
                _stateMachine.SetState(_player.ThisIdleState);
            }
            else
            {
                _doNext = true;
                _stateMachine.SetState(this, true);
            }
        }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        if (_drawDone)
        {
            _drawDone = false;
            _doNext = false;
        }
    }
}
