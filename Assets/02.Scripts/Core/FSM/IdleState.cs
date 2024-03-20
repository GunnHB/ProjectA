using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    /// <summary>
    /// 기본 움직임에 대한 state
    /// </summary>
    public class IdleState : BaseState
    {
        // bool _isStopped = false;
        // float _currBlendValue = 0f;

        public IdleState(PlayerController player) : base(player)
        {
            if (player == null)
                return;
        }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendLocomotion);
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}