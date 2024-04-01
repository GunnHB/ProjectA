using UnityEngine;

namespace ProjectA.Charactes
{
    public class CharacterAnimData
    {
        private string _animNameLocomotion = "Locomotion";
        private string _animParamLocomotionValue = "LocomotionValue";

        public int AnimNameLocomotion { get; private set; }
        public int AnimParamLocomotionValue { get; private set; }

        public virtual void InitializeDatas()
        {
            AnimNameLocomotion = Animator.StringToHash(_animNameLocomotion);
            AnimParamLocomotionValue = Animator.StringToHash(_animParamLocomotionValue);
        }
    }
}
