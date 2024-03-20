using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class CrouchState : BaseState
    {
        private float _maxBlendValue = .5f;

        public CrouchState(PlayerController player) : base(player)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _currLengthOfVector = 0f;
            StartAnimation(_player.ThisAnimData.AnimParamCrouch);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // if (_player.IsMoving)
            //     _dampTarget = _maxBlendValue;
            // else
            //     _dampTarget = 0f;

            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendCrouch);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            StopAnimation(_player.ThisAnimData.AnimParamCrouch);
        }
    }
}