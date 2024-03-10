using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FSM;
using System;

public class DrawState : BaseState
{
    private const string TAG_DRAW = "Tag_Draw";

    private GameValue.WeaponType _currentType = GameValue.WeaponType.None;

    private bool _doNext = false;
    private bool _drawDone = false;

    private float _exitTime = 1f;

    private int _layerIndex = 0;

    public DrawState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        SetWeaponType(ref _currentType);
        _layerIndex = GetLayerIndex();

        if (_currentType != GameValue.WeaponType.None)
        {
            int animHash = _doNext ? _player.ThisAnimData.AnimNameDraw02 : _player.ThisAnimData.AnimNameDraw01;

            _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, _layerIndex);
        }
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        var currentInfo = GetCurrentAnimatorStateInfo(_layerIndex);
        var nextInfo = GetNextAniomatorStateInfo(_layerIndex);

        float normalizedTime = float.MinValue;

        if (_player.ThisAnimator.IsInTransition(_layerIndex) && nextInfo.IsTag(TAG_DRAW))
            normalizedTime = nextInfo.normalizedTime;
        else if (!_player.ThisAnimator.IsInTransition(_layerIndex) && currentInfo.IsTag(TAG_DRAW))
            normalizedTime = currentInfo.normalizedTime;

        if (normalizedTime >= _exitTime)
        {
            if (_doNext)
            {
                _drawDone = true;

                _player.ThisAnimator.CrossFade(_player.ThisAnimData.AnimNameDefault, 0, _layerIndex);

                _layerIndex = GetLayerIndex();
                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .25f, _layerIndex);

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

    private int GetLayerIndex()
    {
        string layerName = string.Empty;

        switch (_currentType)
        {
            case GameValue.WeaponType.OneHand:
                {
                    if (_drawDone)
                        layerName = GameValue.ANIM_LAYER_ONEHAND;
                    else
                        layerName = _player.IsMoving ? GameValue.ANIM_LAYER_ONEHAND_UPPER : GameValue.ANIM_LAYER_ONEHAND;
                }
                break;
            case GameValue.WeaponType.TwoHand:
                {
                    if (_drawDone)
                        layerName = GameValue.ANIM_LAYER_TOWHAND;
                    else
                        layerName = _player.IsMoving ? GameValue.ANIM_LAYER_TOWHAND_UPPER : GameValue.ANIM_LAYER_TOWHAND;
                }
                break;
        }

        return _player.ThisAnimator.GetLayerIndex(layerName);
    }
}
