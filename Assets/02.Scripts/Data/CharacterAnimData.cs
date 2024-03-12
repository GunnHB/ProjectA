using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CharacterAnimData
{
    protected string _animParamDeath = "Trigger_Death";
    protected string _animParamDrawWeapon = "Trigger_DrawWeapon";
    protected string _animParamSheathWeapon = "Trigger_SheathWeapon";
    protected string _animParamAttack = "Trigger_Attack";
    protected string _animParamFalling = "Bool_Falling";
    protected string _animParamLanding = "Bool_Landing";
    protected string _animParamBlendLocomotion = "Float_Locomotion";

    // 읽기 전용
    public int AnimParamDeath { get; private set; }
    public int AnimParamDrawWeapon { get; private set; }
    public int AnimParamSheathWeapon { get; private set; }
    public int AnimParamAttack { get; private set; }
    public int AnimParamFalling { get; private set; }
    public int AnimParamLanding { get; private set; }
    public int AnimParamBlendLocomotion { get; private set; }

    protected bool _isInit = false;
    public bool IsInit => _isInit;

    /// <summary>
    /// 애니 해시 초기화
    /// </summary>
    public virtual void Initialize()
    {
        AnimParamDeath = Animator.StringToHash(_animParamDeath);
        AnimParamDrawWeapon = Animator.StringToHash(_animParamDrawWeapon);
        AnimParamSheathWeapon = Animator.StringToHash(_animParamSheathWeapon);
        AnimParamAttack = Animator.StringToHash(_animParamAttack);
        AnimParamFalling = Animator.StringToHash(_animParamFalling);
        AnimParamLanding = Animator.StringToHash(_animParamLanding);
        AnimParamBlendLocomotion = Animator.StringToHash(_animParamBlendLocomotion);

        _isInit = true;
    }
}
