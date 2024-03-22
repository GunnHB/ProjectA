using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;
using System.Linq;

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

    private InputAction _focusInput;                    // 포커싱

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

        _focusInput = _action.PlayerActionMap.Focus;

        _inventoryInput = _action.PlayerActionMap.Inventory;

        _settingInput = _action.PlayerActionMap.Setting;
        _escapeInput = _action.UIActionMap.Escape;
    }

    private void OnEnable()
    {
        RegistInputAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        RegistInputAction(_sprintInput, StartSprintInput);

        RegistInputAction(_jumpInput, StartJumpInput);

        RegistInputAction(_crouchInput, StartCrouchInput);

        RegistInputAction(_drawWeaponInput, StartDrawWeaponInput);
        RegistInputAction(_attackInput, StartAttackInput);

        RegistInputAction(_zoomInInput, null, PerformZoomInInput, null);
        RegistInputAction(_zoomOutInput, null, PerformZoomOutInput, null);

        RegistInputAction(_focusInput, null, PerformFocusInput, CancelFocusInput);

        RegistInputAction(_inventoryInput, StartInventoryInput);

        RegistInputAction(_escapeInput, StartEscapeInput);
        RegistInputAction(_settingInput, StartSettingInput);

        // StateActions
        RegistStateAction();
    }

    private void OnDisable()
    {
        UnregistInputAction(_movementInput, null, PerformMovementInput, CancelMovementInput);
        UnregistInputAction(_sprintInput, StartSprintInput);

        UnregistInputAction(_jumpInput, StartJumpInput);

        UnregistInputAction(_crouchInput, StartCrouchInput);

        UnregistInputAction(_drawWeaponInput, StartDrawWeaponInput);
        UnregistInputAction(_attackInput, StartAttackInput);

        UnregistInputAction(_zoomInInput, null, PerformZoomInInput, null);
        UnregistInputAction(_zoomOutInput, null, PerformZoomOutInput, null);

        UnregistInputAction(_inventoryInput, StartInventoryInput);

        UnregistInputAction(_escapeInput, StartEscapeInput);
        UnregistInputAction(_settingInput, StartSettingInput);

        // StateActions
        UnRegistStateAction();
    }

    #region InputSystem
    /// <summary>
    /// 입력 등록
    /// </summary>
    /// <param name="inputAction"></param>
    /// <param name="startCallback"></param>
    /// <param name="performCallback"></param>
    /// <param name="cancelCallback"></param>
    private void RegistInputAction(InputAction inputAction,
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
    private void UnregistInputAction(InputAction inputAction,
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
        _inputDirection = new Vector3(inputValue.x, 0f, inputValue.y);

        _targetDamp = _inputDirection.magnitude;
    }

    private void CancelMovementInput(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        _inputDirection = new Vector3(inputValue.x, 0f, inputValue.y);

        _targetDamp = _inputDirection.magnitude;
    }
    #endregion

    #region Sprint
    private void StartSprintInput(InputAction.CallbackContext context)
    {
        if (IsMoving)
        {
            _readyToSprint = !ReadyToSprint;
            SprintAction?.Invoke(_readyToSprint);
        }
    }
    #endregion

    #region Jump
    private void StartJumpInput(InputAction.CallbackContext context)
    {
        JumpAction?.Invoke();
    }
    #endregion

    #region Crouch
    private void StartCrouchInput(InputAction.CallbackContext context)
    {
        _readyToCrouch = !_readyToCrouch;
        CrouchAction?.Invoke(_readyToCrouch);
    }
    #endregion

    #region Draw / Sheath weapon
    private void StartDrawWeaponInput(InputAction.CallbackContext context)
    {
        // 꺼내는 중 (넣는 중) || 무기 안들고 있음
        if (_equipment.DoAction || ItemManager.Instance.EquipWeaponData._invenItemData.IsEmpty())
            return;

        if (_equipment.IsDraw)
            SheathWeaponAction?.Invoke();
        else
            DrawWeaponAction?.Invoke();
    }
    #endregion

    #region Attack
    private void StartAttackInput(InputAction.CallbackContext context)
    {
        if (!CanAttack || _runningCoroutine || _doCombo || ItemManager.Instance.EquipWeaponData._invenItemData.IsEmpty())
            return;

        if (!ItemManager.Instance.EquipWeaponData._invenItemData.IsEmpty() && !_equipment.IsDraw)
        {
            DrawWeaponAction?.Invoke();
            return;
        }

        if (_attackIndex >= GetAttackDataList().Count - 1)
        {
            ResetAttackIndex();
            _lastAttackIndex = true;
        }

        if (_lastAttackIndex)
            return;

        _attackIndex++;

        if (_isAttacking)
        {
            SetDoCombo(true);
            return;
        }

        AttackAction?.Invoke(GetAttackDataList()[_attackIndex]);
    }
    #endregion

    #region Zoom
    private void PerformZoomInInput(InputAction.CallbackContext context)
    {
        // var zoomValue = context.ReadValue<float>();

        // Debug.Log("IN " + zoomValue);
    }

    private void PerformZoomOutInput(InputAction.CallbackContext context)
    {
        // var zoomValue = context.ReadValue<float>();

        // Debug.Log("OUT " + zoomValue);
    }
    #endregion

    #region Focus
    private void PerformFocusInput(InputAction.CallbackContext context)
    {
        FocusAction?.Invoke();
    }

    private void CancelFocusInput(InputAction.CallbackContext context)
    {
        if (!IsMoving)
            IdleAction?.Invoke();
        // else
        // {
        //     if (_readyToSprint)
        //         SprintAction?.Invoke(true);
        // }
    }
    #endregion

    #region Inventory
    private void StartInventoryInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrGameMode == GameManager.GameMode.UI)
            return;

        GameManager.Instance.SetGameMode(GameManager.GameMode.UI);

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

        UIManager.Instance.CloseTopUIByAllCanvas();

        if (!UIManager.Instance.IsOpenAnyUIAllCanvas())
            GameManager.Instance.SetGameMode(GameManager.GameMode.InGame);
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
