using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;

public partial class PlayerController : MonoBehaviour
{
    // Input Systems
    private PlayerInputAction _action;                  // 전체 입력 처리
    private InputAction _movementInput;                 // 움직임 입력
    private InputAction _sprintInput;                   // 달리기 입력
    private InputAction _jumpInput;                     // 점프 입력
    private InputAction _crouchInput;                   // 웅크리기 입력
    private InputAction _drawWeaponInput;               // 무기 듦

    private void OnEnable()
    {
        RegistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        RegistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);
        RegistAction(_jumpInput, StartJumpInput);
        RegistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);
        RegistAction(_drawWeaponInput, StartDrawWeaponInput);
    }

    private void OnDisable()
    {
        UnregistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        UnregistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);
        UnregistAction(_jumpInput, StartJumpInput);
        UnregistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);
        UnregistAction(_drawWeaponInput, StartDrawWeaponInput);
    }

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
        _drawWeaponInput = _action.PlayerActionMap.DrawWeapon;
    }

    #region Movement
    private void PerformMovementInput(InputAction.CallbackContext context)
    {
        // 입력 감지
        var inputValue = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(inputValue.x, 0f, inputValue.y);

        _movement.SetDirection(_moveDirection);

        if (IsOnAir || IsCrouching)
            return;

        // 상태 세팅
        if (_readyToSprint)
            _stateMachine.SetState(_sprintState);
        else
            _stateMachine.SetState(_walkState);
    }

    private void CancelMovementInput(InputAction.CallbackContext context)
    {
        // 상태 세팅
        _moveDirection = Vector3.zero;
        _movement.SetDirection(_moveDirection);

        // 점프 상태에서 이동 키를 뗐을 때 상태 이상 방지
        if (!IsOnAir && !IsCrouching)
            _stateMachine.SetState(_idleState);
    }
    #endregion

    #region Sprint
    private void PerformSprintInput(InputAction.CallbackContext context)
    {
        _readyToSprint = true;

        if (IsOnAir)
            return;

        // 상태 세팅
        if (_moveDirection != Vector3.zero)
            _stateMachine.SetState(_sprintState);
    }

    private void CancelSprintInput(InputAction.CallbackContext context)
    {
        _readyToSprint = false;

        if (IsOnAir)
            return;

        // 상태 세팅
        if (_moveDirection != Vector3.zero)
            _stateMachine.SetState(_walkState);
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
        // 공중에 있는 상태에서는 웅크리기 불가
        if (!IsOnAir)
            _stateMachine.SetState(_crouchState);
    }

    private void CancelCrouchInput(InputAction.CallbackContext context)
    {
        // 공중에 있는 상태에서는 무시
        if (IsOnAir)
            return;

        if (_moveDirection == Vector3.zero)
            _stateMachine.SetState(_idleState);
        else
        {
            if (_readyToSprint)
                _stateMachine.SetState(_sprintState);
            else
                _stateMachine.SetState(_walkState);
        }
    }
    #endregion

    #region Draw weapon
    private void StartDrawWeaponInput(InputAction.CallbackContext context)
    {
        Debug.Log("Draw weapon");
    }
    #endregion
}
