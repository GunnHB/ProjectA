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
    private IState _crouchState;
    private IState _landingState;
    private IState _deathState;

    // Properties
    public IState ThisIdleState => _idleState;
    public IState ThisWalkState => _walkState;
    public IState ThisSprintState => _sprintState;
    public IState ThisJumpState => _jumpState;
    public IState ThisFallingState => _fallingState;
    public IState ThisCrouchState => _crouchState;
    public IState ThisLandingState => _landingState;
    public IState ThisDeathState => _deathState;

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

        // Idle을 첫 상태로 세팅
        _stateMachine = new StateMachine(_idleState);
    }
    #endregion

    #region InputSystem
    /// <summary>
    /// 입력 등록
    /// </summary>
    /// <param name="inputAction"></param>
    /// <param name="startCallback"></param>
    /// <param name="performCallback"></param>
    /// <param name="cancelCallback"></param>
    private void RegistAction(InputAction inputAction,
                              Action<InputAction.CallbackContext> startCallback = null,
                              Action<InputAction.CallbackContext> performCallback = null,
                              Action<InputAction.CallbackContext> cancelCallback = null)
    {
        if (inputAction == null)
        {
            Debug.LogError("no input action! please check this");
            return;
        }

        inputAction.Enable();

        if (startCallback != null)
            inputAction.started += startCallback;

        if (performCallback != null)
            inputAction.performed += performCallback;

        if (cancelCallback != null)
            inputAction.canceled += cancelCallback;
    }

    /// <summary>
    /// 등록된 입력 해지
    /// </summary>
    /// <param name="inputAction"></param>
    /// <param name="startCallback"></param>
    /// <param name="performCallback"></param>
    /// <param name="cancelCallback"></param>
    private void UnregistAction(InputAction inputAction,
                              Action<InputAction.CallbackContext> startCallback = null,
                              Action<InputAction.CallbackContext> performCallback = null,
                              Action<InputAction.CallbackContext> cancelCallback = null)
    {
        inputAction.Disable();

        if (startCallback != null)
            inputAction.started -= startCallback;

        if (performCallback != null)
            inputAction.performed -= performCallback;

        if (cancelCallback != null)
            inputAction.canceled -= cancelCallback;
    }
    #endregion
}
