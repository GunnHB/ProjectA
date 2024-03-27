using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SheathState : BaseState
    {
        private const string TAG_SHEATH = "Tag_Sheath";

        // private GameValue.WeaponType _currentType = GameValue.WeaponType.None;

        private bool _sheathDone = false;
        private float _exitTime = 1f;

        private int _layerIndex = 0;

        public SheathState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _layerIndex = GetLayerIndex(_sheathDone);

            if (_weaponType != GameValue.WeaponType.None)
            {
                // int animHash = _player.ThisAnimData.AnimNameSheath;

                // _player.ThisAnimator.CrossFadeInFixedTime(animHash, .1f, _layerIndex);
                CrossFade(_player.ThisAnimData.AnimNameSheath, _sheathDone);
            }
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_SHEATH);

            if (normalizedTime >= _exitTime)
            {
                _sheathDone = true;

                if (_player.IsMoving)
                    // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);
                    CrossFade(_player.ThisAnimData.AnimNameDefault);

                _layerIndex = GetLayerIndex(_sheathDone);
                // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameDefault, .1f, _layerIndex);
                CrossFade(_player.ThisAnimData.AnimNameDefault, _sheathDone);

                // baselayer로
                // _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, 0);
                CrossFade(_player.ThisAnimData.AnimNameLocomotion);

                _player.IdleAction?.Invoke();
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _sheathDone = false;
        }
    }
}