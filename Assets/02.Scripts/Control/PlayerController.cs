using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;

/// <summary>
/// 플레이어의 총괄 클래스
/// </summary>
public class PlayerController : MonoBehaviour
{
    // State machine
    private StateMachine _stateMachine;

    // Components
    private Movement _movement;
    private PlayerInput _playerInput;
    private Animator _animator;

    // datas
    protected PlayerAnimData _animData = new();         // 애니 해시

    // Input Systems
    private PlayerInputAction _action;                  // 전체 입력 처리
    private InputAction _movementInput;                 // 움직임 입력
    private InputAction _sprintInput;                   // 달리기 입력
    private InputAction _jumpInput;                     // 점프 입력
    private InputAction _crouchInput;                   // 웅크리기 입력

    // States
    private IState _idleState;
    private IState _walkState;
    private IState _sprintState;
    private IState _jumpState;
    private IState _crouchState;
    private IState _landingState;
    private IState _deathState;

    // about move
    private float _moveSpeed = 4f;
    private Vector3 _moveDirection;
    private bool _readyToSprint = false;                // 스프린트 키 입력 여부

    // about jump
    private float _jumpForce = 1f;

    // Properties
    public Animator ThisAnimator => _animator;
    public PlayerAnimData ThisAnimData => _animData;
    public StateMachine ThisStateMachine => _stateMachine;
    public float ThisMoveSpeed => _moveSpeed;
    public bool IsGrounded => _movement.IsGrounded;

    public IState ThisIdleState => _idleState;
    public IState ThisWalkState => _walkState;
    public IState ThisSprintState => _sprintState;
    public IState ThisJumpState => _jumpState;
    public IState ThisCrouchState => _crouchState;
    public IState ThisLandingState => _landingState;
    public IState ThisDeathState => _deathState;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _animData.Initialize();

        RegistStateDictionary();
        SetPlayerInput();
    }

    private void OnEnable()
    {
        RegistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        RegistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);
        RegistAction(_jumpInput, StartJumpInput);
        RegistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);
    }

    private void OnDisable()
    {
        UnregistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        UnregistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);
        UnregistAction(_jumpInput, StartJumpInput);
        UnregistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);
    }

    private void Update()
    {
        _stateMachine.DoOperatorUpdate();
    }

    private void FixedUpdate()
    {
        _movement.MovementUpdate();
        _movement.GravityUpdate();
    }

    public void SetMovementSpeed(float speed)
    {
        _movement.SetSpeed(speed);
    }

    public void DoJump()
    {
        _movement.Jump(_jumpForce);
    }

    #region StateMachine
    /// <summary>
    /// 상태 등록
    /// </summary>
    private void RegistStateDictionary()
    {
        // 상태 생성
        _idleState = new IdleState(this);
        _walkState = new WalkState(this);
        _sprintState = new SprintState(this);
        _jumpState = new JumpState(this);
        _crouchState = new CrouchState(this);
        _landingState = new LandingState(this);
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

    /// <summary>
    /// 입력 이벤트를 등록하기 위한 메서드
    /// </summary>
    private void SetPlayerInput()
    {
        _action = new();

        _movementInput = _action.PlayerActionMap.Movement;
        _sprintInput = _action.PlayerActionMap.Sprint;
        _jumpInput = _action.PlayerActionMap.Jump;
        _crouchInput = _action.PlayerActionMap.Crouch;
    }

    #region Movement
    private void PerformMovementInput(InputAction.CallbackContext context)
    {
        // 입력 감지
        var inputValue = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(inputValue.x, 0f, inputValue.y);

        // 상태 세팅
        if (_readyToSprint)
            _stateMachine.SetState(_sprintState);
        else
            _stateMachine.SetState(_walkState);

        _movement.SetDirection(_moveDirection);
    }

    private void CancelMovementInput(InputAction.CallbackContext context)
    {
        // 상태 세팅
        _moveDirection = Vector3.zero;

        _stateMachine.SetState(_idleState);
        _movement.SetDirection(_moveDirection);
    }
    #endregion

    #region Sprint
    private void PerformSprintInput(InputAction.CallbackContext context)
    {
        _readyToSprint = true;

        // 상태 세팅
        if (_moveDirection != Vector3.zero)
            _stateMachine.SetState(_sprintState);
        else
            _stateMachine.SetState(_jumpState);
    }

    private void CancelSprintInput(InputAction.CallbackContext context)
    {
        _readyToSprint = false;

        // 상태 세팅
        if (_moveDirection != Vector3.zero)
            _stateMachine.SetState(_walkState);
        else
            _stateMachine.SetState(_idleState);
    }
    #endregion

    #region Jump
    private void StartJumpInput(InputAction.CallbackContext context)
    {
        _stateMachine.SetState(_jumpState);
    }
    #endregion

    #region Crouch
    private void PerformCrouchInput(InputAction.CallbackContext context)
    {
        _stateMachine.SetState(_crouchState);
    }

    private void CancelCrouchInput(InputAction.CallbackContext context)
    {
        _stateMachine.SetState(_idleState);
    }
    #endregion
}
