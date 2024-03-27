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

            // _layerIndex = GetLayerIndex(_drawDone);

            if (_weaponType != GameValue.WeaponType.None)
            {
                // int animHash = _player.ThisAnimData.AnimNameDraw;

                // _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, _layerIndex);
                CrossFade(_player.ThisAnimData.AnimNameDraw, _drawDone);
            }
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_DRAW);

            if (normalizedTime >= _exitTime)
            {
                _drawDone = true;

                if (_player.IsMoving)
                    // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);
                    CrossFade(_player.ThisAnimData.AnimNameDefault);

                // _layerIndex = GetLayerIndex(_drawDone);
                // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, _layerIndex);
                CrossFade(_player.ThisAnimData.AnimNameLocomotion, _drawDone);

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
