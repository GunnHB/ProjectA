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
    private InputAction _attackInput;                   // 공격

    private InputAction _zoomInInput;                   // 카메라 줌 인
    private InputAction _zoomOutInput;                  // 카메라 줌 아웃

    private InputAction _inventoryInput;                // 인벤토리

    private InputAction _escapeInput;                   // ui 나가기

    private InputAction _settingInput;                  // 설정

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
        _attackInput = _action.PlayerActionMap.Attack;

        _zoomInInput = _action.PlayerActionMap.ZoomIn;
        _zoomOutInput = _action.PlayerActionMap.ZoomOut;

        _inventoryInput = _action.PlayerActionMap.Inventory;

        _settingInput = _action.PlayerActionMap.Setting;
        _escapeInput = _action.UIActionMap.Escape;
    }

    private void OnEnable()
    {
        RegistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        RegistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);

        RegistAction(_jumpInput, StartJumpInput);

        RegistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);

        RegistAction(_drawWeaponInput, StartDrawWeaponInput);
        RegistAction(_attackInput, StartAttackInput);

        RegistAction(_zoomInInput, null, PerformZoomInInput, null);
        RegistAction(_zoomOutInput, null, PerformZoomOutInput, null);

        RegistAction(_inventoryInput, StartInventoryInput);

        RegistAction(_escapeInput, StartEscapeInput);
        RegistAction(_settingInput, StartSettingInput);
    }

    private void OnDisable()
    {
        UnregistAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        UnregistAction(_sprintInput, null, PerformSprintInput, CancelSprintInput);

        UnregistAction(_jumpInput, StartJumpInput);

        UnregistAction(_crouchInput, null, PerformCrouchInput, CancelCrouchInput);

        UnregistAction(_drawWeaponInput, StartDrawWeaponInput);
        UnregistAction(_attackInput, StartAttackInput);

        UnregistAction(_zoomInInput, null, PerformZoomInInput, null);
        UnregistAction(_zoomOutInput, null, PerformZoomOutInput, null);

        UnregistAction(_inventoryInput, StartInventoryInput);

        UnregistAction(_escapeInput, StartEscapeInput);
        UnregistAction(_settingInput, StartSettingInput);
    }

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

    #region Draw / Sheath weapon
    private void StartDrawWeaponInput(InputAction.CallbackContext context)
    {
        // if (_equipment.DoAction)
        //     return;

        // if (_playerMode == PlayerMode.Combat)
        // {
        //     SheathWeaponAction?.Invoke();
        //     _playerMode = PlayerMode.Normal;
        // }
        // else
        // {
        //     DrawWeaponAction?.Invoke();
        //     _playerMode = PlayerMode.Combat;
        // }
    }
    #endregion

    #region Attack
    private void StartAttackInput(InputAction.CallbackContext context)
    {
        if (_playerMode != PlayerMode.Combat)
            return;

        if (_stateMachine.IsCurrentState(_attackState))
            _doCombo = true;
        else
            _stateMachine.SetState(_attackState);
    }
    #endregion

    #region Zoom
    private void PerformZoomInInput(InputAction.CallbackContext context)
    {
        var zoomValue = context.ReadValue<float>();

        Debug.Log("IN " + zoomValue);
    }

    private void PerformZoomOutInput(InputAction.CallbackContext context)
    {
        var zoomValue = context.ReadValue<float>();

        Debug.Log("OUT " + zoomValue);
    }
    #endregion

    #region Inventory
    private void StartInventoryInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrGameMode == GameManager.GameMode.UI)
            return;

        GameManager.Instance.SetGameMode(GameManager.GameMode.UI);
        // GameManager.Instance.PauseGame(true);

        var menuPanel = UIManager.Instance.OpenUI<UIMenuPanel>();

        if (menuPanel != null)
            menuPanel._inventoryAction?.Invoke();
    }
    #endregion

    #region Escape
    private void StartEscapeInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrGameMode == GameManager.GameMode.InGame)
            return;

        GameManager.Instance.SetGameMode(GameManager.GameMode.InGame);
        // GameManager.Instance.PauseGame(false);

        UIManager.Instance.CloseAllUI(UIManager.Instance.PanelCanvas);
    }
    #endregion

    #region Setting
    private void StartSettingInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrGameMode == GameManager.GameMode.UI)
            return;

        GameManager.Instance.SetGameMode(GameManager.GameMode.UI);
    }
    #endregion
}
