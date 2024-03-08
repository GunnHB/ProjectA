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

    private string _animNameLocomotion = "BlendTree_Locomotion";

    private string _animNameOneHandLocomotion = "OneHandLocomotion";
    private string _animNameOneHandDraw01 = "OneHandDraw01";
    private string _animNameOneHandDraw02 = "OneHandDraw02";
    private string _animNameOneHandSheath01 = "OneHandSheath01";
    private string _animNameOneHandSheath02 = "OneHandSheath02";

    private string _animNameAttack01 = "Attack01";
    private string _animNameAttack02 = "Attack02";
    private string _animNameAttack03 = "Attack03";
    private string _animNameAttack04 = "Attack04";

    public int AnimParamJump { get; private set; }
    public int AnimParamCrouch { get; private set; }
    public int AnimParamCombatMode { get; private set; }
    public int AnimParamBlendCrouch { get; private set; }
    public int AnimParamAttackIndex { get; private set; }

    public int AnimNameLocomotion { get; private set; }

    public int AnimNameOneHandLocomotion { get; private set; }
    public int AnimNameOneHandDraw01 { get; private set; }
    public int AnimNameOneHandDraw02 { get; private set; }
    public int AnimNameOneHandSheath01 { get; private set; }
    public int AnimNameOneHandSheath02 { get; private set; }

    public int AnimNameAttack01 { get; private set; }
    public int AnimNameAttack02 { get; private set; }
    public int AnimNameAttack03 { get; private set; }
    public int AnimNameAttack04 { get; private set; }

    public override void Initialize()
    {
        AnimParamJump = Animator.StringToHash(_animParamJump);
        AnimParamCrouch = Animator.StringToHash(_animParamCrouch);
        AnimParamCombatMode = Animator.StringToHash(_animParamCombatMode);
        AnimParamBlendCrouch = Animator.StringToHash(_animParamBlendCrouch);
        AnimParamAttackIndex = Animator.StringToHash(_animParamAttackIndex);

        AnimNameLocomotion = Animator.StringToHash(_animNameLocomotion);

        AnimNameOneHandLocomotion = Animator.StringToHash(_animNameOneHandLocomotion);
        AnimNameOneHandDraw01 = Animator.StringToHash(_animNameOneHandDraw01);
        AnimNameOneHandDraw02 = Animator.StringToHash(_animNameOneHandDraw02);
        AnimNameOneHandSheath01 = Animator.StringToHash(_animNameOneHandSheath01);
        AnimNameOneHandSheath02 = Animator.StringToHash(_animNameOneHandSheath02);

        AnimNameAttack01 = Animator.StringToHash(_animNameAttack01);
        AnimNameAttack02 = Animator.StringToHash(_animNameAttack02);
        AnimNameAttack03 = Animator.StringToHash(_animNameAttack03);
        AnimNameAttack04 = Animator.StringToHash(_animNameAttack04);

        base.Initialize();
    }
}
