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

    private string _animParamAttack01 = "Attack01";
    private string _animParamAttack02 = "Attack02";
    private string _animParamAttack03 = "Attack03";
    private string _animParamAttack04 = "Attack04";

    public int AnimParamJump { get; private set; }
    public int AnimParamCrouch { get; private set; }
    public int AnimParamCombatMode { get; private set; }
    public int AnimParamBlendCrouch { get; private set; }
    public int AnimParamAttackIndex { get; private set; }

    public int AnimParamAttack01 { get; private set; }
    public int AnimParamAttack02 { get; private set; }
    public int AnimParamAttack03 { get; private set; }
    public int AnimParamAttack04 { get; private set; }

    public override void Initialize()
    {
        AnimParamJump = Animator.StringToHash(_animParamJump);
        AnimParamCrouch = Animator.StringToHash(_animParamCrouch);
        AnimParamCombatMode = Animator.StringToHash(_animParamCombatMode);
        AnimParamBlendCrouch = Animator.StringToHash(_animParamBlendCrouch);
        AnimParamAttackIndex = Animator.StringToHash(_animParamAttackIndex);

        AnimParamAttack01 = Animator.StringToHash(_animParamAttack01);
        AnimParamAttack02 = Animator.StringToHash(_animParamAttack02);
        AnimParamAttack03 = Animator.StringToHash(_animParamAttack03);
        AnimParamAttack04 = Animator.StringToHash(_animParamAttack04);

        base.Initialize();
    }
}
