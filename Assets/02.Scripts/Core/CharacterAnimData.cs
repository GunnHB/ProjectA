using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CharacterAnimData
{
    [SerializeField] protected string _animParamWalk = "Walk";
    [SerializeField] protected string _animParamSprint = "Sprint";
    [SerializeField] protected string _animParamBlendSpeed = "BlendSpeed";

    // 읽기 전용
    public int AnimParamWalk { get; private set; }
    public int AnimParamSprint { get; private set; }
    public int AnimParamBlendSpeed { get; private set; }

    public void Initialize()
    {
        AnimParamWalk = Animator.StringToHash(_animParamWalk);
        AnimParamSprint = Animator.StringToHash(_animParamSprint);
        AnimParamBlendSpeed = Animator.StringToHash(_animParamBlendSpeed);
    }
}
