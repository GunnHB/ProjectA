using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CharacterAnimData
{
    [SerializeField] protected string _animParamWalk = "Walk";
    [SerializeField] protected string _animParamSprint = "Sprint";
    [SerializeField] protected string _animParamDeath = "Death";
    [SerializeField] protected string _animParamBlendSpeed = "BlendSpeed";

    // 읽기 전용
    public int AnimParamWalk { get; private set; }
    public int AnimParamSprint { get; private set; }
    public int AnimParamDeath { get; private set; }
    public int AnimParamBlendSpeed { get; private set; }

    /// <summary>
    /// 애니 해시 초기화
    /// </summary>
    public void Initialize()
    {
        AnimParamWalk = Animator.StringToHash(_animParamWalk);
        AnimParamSprint = Animator.StringToHash(_animParamSprint);
        AnimParamDeath = Animator.StringToHash(_animParamDeath);
        AnimParamBlendSpeed = Animator.StringToHash(_animParamBlendSpeed);
    }
}
