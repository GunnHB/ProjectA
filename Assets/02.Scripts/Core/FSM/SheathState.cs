using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FSM;

public class SheathState : BaseState
{
    private const string TAG_SHEATH = "Tag_Sheath";

    private GameValue.WeaponType _currentType = GameValue.WeaponType.None;

    private bool _sheathDone = false;
    private float _exitTime = 1f;

    private int _layerIndex = 0;

    public SheathState(PlayerController player) : base(player)
    {
    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        SetWeaponType(ref _currentType);
        _layerIndex = GetLayerIndex(_currentType, _sheathDone);

        if (_currentType != GameValue.WeaponType.None)
        {
            int animHash = _player.ThisAnimData.AnimNameSheath;

            _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, _layerIndex);
        }
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        float normalizedTime = GetNormalizedTimeByTag(TAG_SHEATH, _layerIndex);

        if (normalizedTime >= _exitTime)
        {
            _sheathDone = true;

            if (_player.IsMoving)
                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);

            _layerIndex = GetLayerIndex(_currentType, _sheathDone);
            _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);

            // baselayer로
            _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, 0);

            _player.IdleAction?.Invoke();
        }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        _sheathDone = false;
    }
}