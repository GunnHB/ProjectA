using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace ProjectA.Charactes
{
    public partial class PlayerControls : CharacterControls
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            if (_inputActions == null)
            {
                _inputActions = new PlayerInputAction();

                if (_inputActions != null)
                    RegistInputActions();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_inputActions != null)
                UnregistInputActions();
        }

        private void RegistInputActions()
        {
            // Movement
            RegistInputAction(_inputActions.PlayerActionMap.Movement, null, PerformedMovement, CanceledMovement);

            _inputActions.Enable();
        }

        private void UnregistInputActions()
        {
            // Movement
            UnregistInputAction(_inputActions.PlayerActionMap.Movement, null, PerformedMovement, CanceledMovement);

            _inputActions.Disable();
        }

        private void RegistInputAction(InputAction action,
                                        Action<InputAction.CallbackContext> startedCallback = null,
                                        Action<InputAction.CallbackContext> performedCallback = null,
                                        Action<InputAction.CallbackContext> canceledCallback = null)
        {
            if (startedCallback != null)
                action.started += startedCallback;

            if (performedCallback != null)
                action.performed += performedCallback;

            if (canceledCallback != null)
                action.canceled += canceledCallback;
        }

        private void UnregistInputAction(InputAction action,
                                        Action<InputAction.CallbackContext> startedCallback = null,
                                        Action<InputAction.CallbackContext> performedCallback = null,
                                        Action<InputAction.CallbackContext> canceledCallback = null)
        {
            if (startedCallback != null)
                action.started -= startedCallback;

            if (performedCallback != null)
                action.performed -= performedCallback;

            if (canceledCallback != null)
                action.canceled -= canceledCallback;
        }

        #region Movement
        private void PerformedMovement(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();

            _verticalInput = input.y;
            _horizontalInput = input.x;

            _moveDirection = new Vector3(_horizontalInput, 0, _verticalInput);
        }

        private void CanceledMovement(InputAction.CallbackContext context)
        {
            _verticalInput = 0f;
            _horizontalInput = 0f;

            _moveDirection = Vector3.zero;
        }
        #endregion
    }
}
