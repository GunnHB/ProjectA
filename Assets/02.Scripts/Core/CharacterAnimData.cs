using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CharacterAnimData
{
    protected string _animParamDeath = "Trigger_Death";
    protected string _animParamFalling = "Bool_Falling";
    protected string _animParamLanding = "Bool_Landing";
    protected string _animParamBlendLocomotion = "Float_Locomotion";

    // 읽기 전용
    public int AnimParamDeath { get; private set; }
    public int AnimParamFalling { get; private set; }
    public int AnimParamLanding { get; private set; }
    public int AnimParamBlendLocomotion { get; private set; }

    /// <summary>
    /// 애니 해시 초기화
    /// </summary>
    public virtual void Initialize()
    {
        AnimParamDeath = Animator.StringToHash(_animParamDeath);
        AnimParamFalling = Animator.StringToHash(_animParamFalling);
        AnimParamLanding = Animator.StringToHash(_animParamLanding);
        AnimParamBlendLocomotion = Animator.StringToHash(_animParamBlendLocomotion);
    }
}
