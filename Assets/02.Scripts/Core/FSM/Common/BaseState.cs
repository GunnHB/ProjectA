using UnityEngine;

namespace ProjectA.Charactes.FSM
{
    public class BaseState : IState
    {
        protected CharacterControls _controls;

        protected float _currLocomotionValue = 0f;

        public BaseState(CharacterControls controls)
        {
            _controls = controls;
        }

        public virtual void OperateEnter()
        {
            _currLocomotionValue = GetFloat(_controls.ThisAnimData.AnimParamLocomotionValue);
        }

        public virtual void OperateExit()
        {

        }

        public virtual void OperateUpdate()
        {

        }

        protected void SetFloat(int animHash, float value)
        {
            _controls.ThisAnimator.SetFloat(animHash, value);
        }

        protected float GetFloat(int animHash)
        {
            return _controls.ThisAnimator.GetFloat(animHash);
        }
    }
}
