using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;
using UnityEngine.Events;

/// <summary>
/// 플레이어의 총괄 클래스
/// </summary>
public partial class PlayerController : MonoBehaviour
{
    public enum PlayerMode
    {
        None = -1,
        Normal,         // 일반 모드
        Combat,         // 전투 모드
    }

    // State machine
    private StateMachine _stateMachine;

    // Components
    private Movement _movement;
    private PlayerInput _playerInput;
    private Animator _animator;

    // datas
    protected PlayerAnimData _animData = new();         // 애니 해시

    // about move
    private float _moveSpeed = 4f;
    private Vector3 _moveDirection;
    private bool _readyToSprint = false;                // 스프린트 키 입력 여부

    // about jump
    private float _jumpForce = 1f;

    private PlayerMode _playerMode;

    public UnityAction DrawWeaponAction;
    public UnityAction SheathWeaponAction;

    // Properties
    public Animator ThisAnimator => _animator;
    public PlayerAnimData ThisAnimData => _animData;
    public StateMachine ThisStateMachine => _stateMachine;
    public Vector3 ThisMoveDirection => _moveDirection;
    public float ThisMoveSpeed => _moveSpeed;

    public bool IsGrounded => _movement.IsGrounded;
    public bool IsPeak => _movement.IsPeak;
    public bool ReadyToSprint => _readyToSprint;

    public PlayerMode ThisPlayerMode => _playerMode;

    // 공중에 있는 상태
    public bool IsOnAir
    {
        get
        {
            return _stateMachine.IsCurrentState(_jumpState) ||
                _stateMachine.IsCurrentState(_fallingState) ||
                _stateMachine.IsCurrentState(_landingState);
        }
    }

    // 웅크린 상태
    public bool IsCrouching => _stateMachine.IsCurrentState(_crouchState);

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _animData.Initialize();

        _playerMode = PlayerMode.Normal;

        RegistStateDictionary();
        SetPlayerInput();
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
}
