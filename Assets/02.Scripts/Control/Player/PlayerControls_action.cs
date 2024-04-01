using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectA.Charactes
{
    public partial class PlayerControls : CharacterControls
    {
        // components
        private PlayerInputAction _inputActions;
        private Coroutine _movementCoroutine;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_inputActions == null)
                _inputActions = new PlayerInputAction();

            RegistInputActions();
            RegistFSMActions();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_inputActions != null)
                UnregistInputActions();

            UnregistFSMActions();
        }

        private void RegistInputActions()
        {
            // Movement
            RegistInputAction(_inputActions.PlayerActionMap.Movement, null, PerformedMovement, CanceledMovement);
            RegistInputAction(_inputActions.PlayerActionMap.Sprint, StartedSprint);

            _inputActions.Enable();
        }

        private void UnregistInputActions()
        {
            // Movement
            UnregistInputAction(_inputActions.PlayerActionMap.Movement, null, PerformedMovement, CanceledMovement);
            UnregistInputAction(_inputActions.PlayerActionMap.Sprint, StartedSprint);

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

            _moveDirection = new Vector3(input.x, 0f, input.y);

            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
                _movementCoroutine = null;
            }

            _movementCoroutine = StartCoroutine(nameof(Cor_Movement));
        }

        private void CanceledMovement(InputAction.CallbackContext context)
        {
            _moveDirection = Vector3.zero;

            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
                _movementCoroutine = null;
            }

            _movementCoroutine = StartCoroutine(nameof(Cor_Movement));
        }

        private IEnumerator Cor_Movement()
        {
            float targetDamp = _moveDirection.magnitude;

            while (true)
            {
                _moveAmount = Mathf.SmoothDamp(_moveAmount, targetDamp, ref _smoothVelocity, _smoothTime);

                if (Mathf.Abs(targetDamp - _moveAmount) < .01f)
                {
                    _moveAmount = targetDamp == 0 ? 0 : targetDamp;
                    yield break;
                }

                yield return null;
            }
        }
        #endregion

        #region Sprint
        private void StartedSprint(InputAction.CallbackContext context)
        {
            _readyToSprint = !_readyToSprint;
        }
        #endregion
    }
}
