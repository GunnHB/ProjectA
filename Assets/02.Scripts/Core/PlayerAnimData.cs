using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimData : CharacterAnimData
{
    private string _animParamJump = "Bool_Jump";
    private string _animParamCrouch = "Bool_Crouch";
    protected string _animParamCombatMode = "Bool_CombatMode";
    private string _animParamBlendCrouch = "Float_Crouch";
    private string _animParamAttackIndex = "Int_AttackIndex";

    public int AnimParamJump { get; private set; }
    public int AnimParamCrouch { get; private set; }
    public int AnimParamCombatMode { get; private set; }
    public int AnimParamBlendCrouch { get; private set; }
    public int AnimParamAttackIndex { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        AnimParamJump = Animator.StringToHash(_animParamJump);
        AnimParamCrouch = Animator.StringToHash(_animParamCrouch);
        AnimParamCombatMode = Animator.StringToHash(_animParamCombatMode);
        AnimParamBlendCrouch = Animator.StringToHash(_animParamBlendCrouch);
        AnimParamAttackIndex = Animator.StringToHash(_animParamAttackIndex);
    }
}
