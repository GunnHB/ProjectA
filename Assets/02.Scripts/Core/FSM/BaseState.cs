using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public class BaseState : IState
    {
        protected PlayerController _player;
        protected StateMachine _stateMachine;

        protected float _currLengthOfVector;
        protected float _smoothVelocity;
        protected float _smoothTime = .15f;
        protected float _dampTarget;

        public BaseState(PlayerController player)
        {
            _player = player;
        }

        public virtual void OperateEnter()
        {
            // Debug.Log($"{this} enter");

            if (_stateMachine == null)
                _stateMachine = _player.ThisStateMachine;
        }

        public virtual void OperateExit()
        {
            // Debug.Log($"{this} exit");

            if (_stateMachine == null)
                _stateMachine = _player.ThisStateMachine;
        }

        public virtual void OperateUpdate()
        {
            // Debug.Log($"{this} update");

            if (_stateMachine == null)
                _stateMachine = _player.ThisStateMachine;
        }

        /// <summary>
        /// 캐릭터의 애니와 속도를 세팅
        /// </summary>
        protected void SetPlayerMovement()
        {
            _currLengthOfVector = Mathf.SmoothDamp(_currLengthOfVector, _dampTarget, ref _smoothVelocity, _smoothTime);

            SetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion, _currLengthOfVector);
            _player.SetMovementSpeed(_currLengthOfVector * _player.ThisMoveSpeed);
        }

        protected bool GetPreviousState(IState state)
        {
            return _stateMachine != null && _stateMachine.IsPreviousState(state);
        }

        protected void StartAnimation(int animHash)
        {
            _player.ThisAnimator.SetBool(animHash, true);
        }

        protected void StopAnimation(int animHash)
        {
            _player.ThisAnimator.SetBool(animHash, false);
        }

        protected void SetTriggerAnimation(int animHash)
        {
            _player.ThisAnimator.SetTrigger(animHash);
        }

        protected void SetFloatParam(int animHash, float value)
        {
            _player.ThisAnimator.SetFloat(animHash, value);
        }

        protected float GetFloatParam(int animHash)
        {
            return _player.ThisAnimator.GetFloat(animHash);
        }
    }
}