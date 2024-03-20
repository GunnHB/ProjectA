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

        _idleState = new IdleState(this);
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
        SprintAction += OnSprint;
        CancelSprintAction += CancelSprint;

        CrouchAction += OnCrouch;
        CancelCrouchAction += CancelCrouch;

        JumpAction += OnJump;
        FallingAction += OnFalling;
        LandingAction += OnLanding;

        FocusAction += OnFocus;

        DrawWeaponAction += OnDraw;
        SheathWeaponAction += OnSheath;

        AttackAction += OnAttack;
    }

    private void UnRegistStateAction()
    {
        IdleAction -= OnIdle;
        SprintAction -= OnSprint;
        CancelSprintAction -= CancelSprint;

        CrouchAction += OnCrouch;
        CancelCrouchAction += CancelCrouch;

        JumpAction -= OnJump;
        FallingAction -= OnFalling;
        LandingAction += OnLanding;

        FocusAction -= OnFocus;

        DrawWeaponAction -= OnDraw;
        SheathWeaponAction -= OnSheath;

        AttackAction -= OnAttack;
    }

    private void OnIdle()
    {
        _stateMachine.SwitchState(_idleState);
    }

    private void OnSprint(bool doSprint)
    {
        if (_readyToCrouch)
            CancelCrouchAction?.Invoke();

        _targetDamp = doSprint ? GameValue._baseLocomotionMaxValue : _moveDirection.magnitude;
        _stateMachine.SwitchState(doSprint ? _sprintState : _idleState);
    }

    private void CancelSprint()
    {
        _readyToSprint = false;
        _targetDamp = _moveDirection.magnitude;

        if (_readyToCrouch)
            return;

        _stateMachine.SwitchState(_idleState);
    }

    private void OnCrouch(bool doCrouch)
    {
        // 달리고 있던 중이면 호출
        if (_readyToSprint)
            CancelSprintAction?.Invoke();

        if (doCrouch)
            _stateMachine.SwitchState(_crouchState);
        else
            CancelCrouchAction?.Invoke();
    }

    private void CancelCrouch()
    {
        _readyToCrouch = false;

        if (!_readyToSprint)
            _stateMachine.SwitchState(_idleState);
    }

    private void OnJump()
    {
        _stateMachine.SwitchState(_jumpState);
    }

    private void OnFalling()
    {
        _stateMachine.SwitchState(_fallingState);
    }

    private void OnLanding()
    {
        _stateMachine.SwitchState(_landingState);
    }

    private void OnFocus()
    {

    }

    private void OnDraw()
    {
        _stateMachine.SwitchState(_drawState);
    }

    private void OnSheath()
    {
        _stateMachine.SwitchState(_sheathState);
    }

    private void OnAttack(AttackData attackData)
    {
        (_attackState as AttackState).SetCurrAttackData(attackData);
        _stateMachine.SwitchState(_attackState, true);
    }
}
