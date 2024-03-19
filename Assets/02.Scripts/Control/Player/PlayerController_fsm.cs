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

    private IState _drawState;
    private IState _sheathState;

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

    public IState ThisDrawState => _drawState;
    public IState ThisSheathState => _sheathState;

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

        _drawState = new DrawState(this);
        _sheathState = new SheathState(this);

        _attackState = new AttackState(this);

        // Idle을 첫 상태로 세팅
        _stateMachine = new StateMachine(_idleState);
    }
    #endregion

    private void RegistStateAction()
    {
        IdleAction += OnIdle;
        WalkAction += OnWalk;
        SprintAction += OnSprint;

        FocusAction += OnFocus;

        DrawWeaponAction += OnDraw;
        SheathWeaponAction += OnSheath;

        AttackAction += OnAttack;
    }

    private void UnRegistStateAction()
    {
        IdleAction -= OnIdle;
        WalkAction -= OnWalk;
        SprintAction -= OnSprint;

        JumpAction -= OnJump;
        FallingAction -= OnFalling;

        FocusAction -= OnFocus;

        DrawWeaponAction -= OnDraw;
        SheathWeaponAction -= OnSheath;

        AttackAction -= OnAttack;
    }

    private void OnIdle()
    {
        _stateMachine.SetState(_idleState);
    }

    private void OnWalk()
    {
        if (!_doNotMovePlayer)
            _stateMachine.SetState(_walkState);
    }

    private void OnSprint(bool readyToSprint)
    {
        _readyToSprint = readyToSprint;

        if (IsOnAir)
            return;

        // 상태 세팅
        if (IsMoving)
        {
            if (readyToSprint)
                _stateMachine.SetState(_sprintState);
            else
                WalkAction?.Invoke();
        }
    }

    private void OnJump()
    {

    }

    private void OnFalling()
    {

    }

    private void OnFocus()
    {

    }

    private void OnDraw()
    {
        _stateMachine.SetState(_drawState);
    }

    private void OnSheath()
    {
        _stateMachine.SetState(_sheathState);
    }

    private void OnAttack(AttackData attackData)
    {
        (_attackState as AttackState).SetCurrAttackData(attackData);
        _stateMachine.SetState(_attackState, true);
    }
}
