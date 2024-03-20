using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class DrawState : BaseState
    {
        private const string TAG_DRAW = "Tag_Draw";

        // private GameValue.WeaponType _currentType = GameValue.WeaponType.None;

        private bool _drawDone = false;
        private float _exitTime = 1f;

        private int _layerIndex = 0;

        public DrawState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _layerIndex = GetLayerIndex(_drawDone);

            if (_weaponType != GameValue.WeaponType.None)
            {
                int animHash = _player.ThisAnimData.AnimNameDraw;

                _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, _layerIndex);
            }
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_DRAW, _layerIndex);

            if (normalizedTime >= _exitTime)
            {
                _drawDone = true;

                if (_player.IsMoving)
                    _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);

                _layerIndex = GetLayerIndex(_drawDone);
                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, _layerIndex);

                _player.IdleAction?.Invoke();
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _drawDone = false;
        }
    }
}
