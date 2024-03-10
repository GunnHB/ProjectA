using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class IdleState : BaseState
    {
        bool _isStopped = false;
        float _currBlendValue = 0f;

        public IdleState(PlayerController player) : base(player)
        {
            if (player == null)
                return;
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            if (GetPreviousState(_player.ThisAttackState))
                CrossFadeInFixedUpdate(_player.ThisAnimData.AnimNameLocomotion);

            if (GetPreviousState(_player.ThisWalkState) || GetPreviousState(_player.ThisSprintState))
            {
                _currBlendValue = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
                _isStopped = true;
            }
            else
            {
                _currBlendValue = 0f;
                SetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion, _currBlendValue);
            }
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (_isStopped)
            {
                _currBlendValue = Mathf.SmoothDamp(_currBlendValue, 0, ref _smoothVelocity, _smoothTime);
                float fixedValue = (float)Math.Round(_currBlendValue, 2);

                SetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion, fixedValue);

                if (Mathf.Approximately(fixedValue, 0))
                {
                    _currBlendValue = 0f;
                    _isStopped = false;
                }
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}