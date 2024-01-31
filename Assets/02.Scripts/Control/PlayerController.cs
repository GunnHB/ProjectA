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
    private Dictionary<StateType, IState> _stateDic = new();

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

    // about move
    private float _walkSpeed = 4f;
    private float _sprintSpeed = 7f;

    // Properties
    public Animator ThisAnimator => _animator;
    public PlayerAnimData ThisAnimData => _animData;
    public StateMachine ThisStateMachine => _stateMachine;
    public Dictionary<StateType, IState> ThisStateDic => _stateDic;
    public float ThisWalkSpeed => _walkSpeed;
    public float ThisSprintSpeed => _sprintSpeed;
    public Vector3 ThisMoveVector { get; private set; }

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
    }

    private void OnDisable()
    {
        UnregistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        UnregistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);
    }

    private void Update()
    {
        _stateMachine.DoOperatorUpdate();
    }

    public void SetMovementSpeed(float speed)
    {
        _movement.SetSpeed(speed);
    }

    #region StateMachine
    /// <summary>
    /// 상태 등록
    /// </summary>
    private void RegistStateDictionary()
    {
        // 상태 생성
        IState idleState = new IdleState(this, StateType.IDLE);
        IState walkState = new WalkState(this, StateType.WALK);
        IState sprintState = new SprintState(this, StateType.SPRINT);
        IState deathState = new DeathState(this, StateType.DEATH);

        // 상태 등록
        _stateDic.Add(StateType.IDLE, idleState);
        _stateDic.Add(StateType.WALK, walkState);
        _stateDic.Add(StateType.SPRINT, sprintState);
        _stateDic.Add(StateType.DEATH, deathState);

        // Idle을 첫 상태로 세팅
        _stateMachine = new StateMachine(idleState);
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

    /// <summary>
    /// 입력 이벤트를 등록하기 위한 메서드
    /// </summary>
    private void SetPlayerInput()
    {
        _action = new();

        _movementInput = _action.PlayerActionMap.Movement;
        _sprintInput = _action.PlayerActionMap.Sprint;
    }

    private void PerformMovementInput(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        ThisMoveVector = new Vector3(inputValue.x, 0f, inputValue.y);

        _stateMachine.SetState(_stateDic[StateType.WALK]);
        _movement.SetDirection(ThisMoveVector);
    }

    private void CancelMovementInput(InputAction.CallbackContext context)
    {
        ThisMoveVector = Vector3.zero;

        _stateMachine.SetState(_stateDic[StateType.IDLE]);
        _movement.SetDirection(ThisMoveVector);
    }

    private void PerformSprintInput(InputAction.CallbackContext context)
    {
        // if (_stateMachine.CurrentState == _stateDic[StateType.WALK])
        // {
        //     _stateMachine.SetState(_stateDic[StateType.SPRINT]);
        //     _movement.SetSpeed(_sprintSpeed);
        // }
        _stateMachine.SetState(_stateDic[StateType.SPRINT]);
        _movement.SetSpeed(_sprintSpeed);
    }

    private void CancelSprintInput(InputAction.CallbackContext context)
    {
        if (_movement.Direction == Vector3.zero)
            _stateMachine.SetState(_stateDic[StateType.IDLE]);
        else
        {
            _stateMachine.SetState(_stateDic[StateType.WALK]);
            _movement.SetSpeed(_walkSpeed);
        }
    }
    #endregion
}
