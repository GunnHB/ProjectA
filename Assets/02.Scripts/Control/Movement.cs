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
        public Action<Vector3> RotateAction;

        // variables
        private Vector3 _currentMovementVector;
        // private Vector3 _targetMovementValue;
        private Vector3 _smoothVelocity;
        private float _smoothTime = .1f;

        public void Initialize()
        {
            _controller = GetComponent<CharacterController>();

            MoveAction += MoveCallback;
            RotateAction += RotateCallback;
        }

        private void MoveCallback(Vector3 targetDirection, float speed)
        {
            _currentMovementVector = Vector3.SmoothDamp(_currentMovementVector, targetDirection, ref _smoothVelocity, _smoothTime);
            _controller.Move(_currentMovementVector * speed * Time.deltaTime);
        }

        private void RotateCallback(Vector3 direction)
        {

        }
    }
}
