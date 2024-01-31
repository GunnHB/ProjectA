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

        public IdleState(PlayerController player, StateType stateType) : base(player, stateType)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            if (_stateMachine != null && (_stateMachine.PreviousState as BaseState).ThisStateType == StateType.WALK)
            {
                _currBlendValue = GetFloatParam(_player.ThisAnimData.AnimParamBlendSpeed);
                _isStopped = true;
            }
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (_isStopped)
            {
                _currBlendValue = Mathf.SmoothDamp(_currBlendValue, 0, ref _smoothVelocity, _smoothTime);

                SetFloatParam(_player.ThisAnimData.AnimParamBlendSpeed, (float)Math.Round(_currBlendValue, 2));

                if (Mathf.Approximately((float)Math.Round(_currBlendValue, 2), 0))
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