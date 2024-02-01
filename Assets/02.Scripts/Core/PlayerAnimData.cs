using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimData : CharacterAnimData
{
    private string _animParamJump = "Bool_Jump";

    public int AnimParamJump { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        AnimParamJump = Animator.StringToHash(_animParamJump);
    }
}
