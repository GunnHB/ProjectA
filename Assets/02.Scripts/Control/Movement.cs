using System;

using UnityEngine;

namespace ProjectA.Charactes
{
    public class Movement : MonoBehaviour
    {
        // components
        private CharacterController _controller;

        // actions
        public Action<Vector3, float> MoveAction;
        public Action<float> RotateAction;

        // variables
        private Vector3 _currentMovementVector;
        private Vector3 _vectorSmoothVelocity;
        private float _smoothTime = .1f;

        private float _floatSmoothVelocity;

        private bool IsPlayer => TryGetComponent(out PlayerControls player);

        public void Initialize()
        {
            _controller = GetComponent<CharacterController>();

            MoveAction += MoveCallback;
            RotateAction += RotateCallback;
        }

        private void MoveCallback(Vector3 targetDirection, float speed)
        {
            if (targetDirection.magnitude < .1f)
                return;

            float targetAngle = GetTargetAngle(targetDirection);

            RotateAction?.Invoke(targetAngle);
            MoveTo(targetAngle, targetDirection, speed);
        }

        private void MoveTo(float targetAngle, Vector3 direction, float speed)
        {
            if (IsPlayer)
            {
                Vector3 targetDir = Quaternion.Euler(new Vector3(0f, targetAngle, 0f)) * Vector3.forward;
                _currentMovementVector = Vector3.SmoothDamp(_currentMovementVector, targetDir, ref _vectorSmoothVelocity, _smoothTime);
            }
            else
                _currentMovementVector = (direction - transform.position).normalized;

            _controller.Move(_currentMovementVector * speed * Time.deltaTime);
        }

        private void RotateCallback(float targetAngle)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _floatSmoothVelocity, _smoothTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
        }

        private float GetTargetAngle(Vector3 direction)
        {
            float camAngle = IsPlayer ? Camera.main.transform.eulerAngles.y : 0f;
            Vector3 tempDir = IsPlayer ? direction : (direction - transform.position).normalized;

            return Mathf.Atan2(tempDir.x, tempDir.z) * Mathf.Rad2Deg + camAngle;
        }

        public Vector3 GetCurrentMovementVector()
        {
            return _currentMovementVector;
        }
    }
}
