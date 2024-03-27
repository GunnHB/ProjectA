using UnityEngine;

namespace ProjectA.Charactes
{
    public class CharacterAnimData
    {
        private string _animNameLocomotion = "Locomotion";
        private string _animParamVerticalValue = "VerticalValue";
        private string _animParamHorizontalValue = "HorizontalValue";

        public int AnimNameLocomotion { get; private set; }
        public int AnimParamVerticalValue { get; private set; }
        public int AnimParamHorizontalValue { get; private set; }

        public virtual void InitializeDatas()
        {
            AnimNameLocomotion = Animator.StringToHash(_animNameLocomotion);
            AnimParamVerticalValue = Animator.StringToHash(_animParamVerticalValue);
            AnimParamHorizontalValue = Animator.StringToHash(_animParamHorizontalValue);
        }
    }
}
