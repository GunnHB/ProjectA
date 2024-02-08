using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;

public partial class PlayerController : MonoBehaviour
{
    // States
    private IState _idleState;

    private IState _walkState;
    private IState _sprintState;

    private IState _jumpState;
    private IState _fallingState;
    private IState _landingState;

    private IState _crouchState;

    private IState _deathState;

    private IState _attackState;

    // Properties
    public IState ThisIdleState => _idleState;

    public IState ThisWalkState => _walkState;
    public IState ThisSprintState => _sprintState;

    public IState ThisJumpState => _jumpState;
    public IState ThisFallingState => _fallingState;
    public IState ThisLandingState => _landingState;

    public IState ThisCrouchState => _crouchState;

    public IState ThisDeathState => _deathState;

    public IState ThisAttackState => _attackState;

    #region StateMachine
    /// <summary>
    /// 상태 등록
    /// </summary>
    private void RegistStateDictionary()
    {
        // 상태 생성

        // idle ~ move
        _idleState = new IdleState(this);
        _walkState = new WalkState(this);
        _sprintState = new SprintState(this);

        // jump
        _jumpState = new JumpState(this);
        _fallingState = new FallingState(this);
        _landingState = new LandingState(this);

        _crouchState = new CrouchState(this);

        _deathState = new DeathState(this);

        _attackState = new AttackState(this);

        // Idle을 첫 상태로 세팅
        _stateMachine = new StateMachine(_idleState);
    }
    #endregion
}
