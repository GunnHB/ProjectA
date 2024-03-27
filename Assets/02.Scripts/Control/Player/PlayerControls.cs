using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Charactes
{
    public partial class PlayerControls : CharacterControls
    {
        // components
        private PlayerInputAction _inputActions;

        // variables
        private float _verticalInput;
        private float _horizontalInput;
        private Vector3 _moveDirection;

        protected override void Awake()
        {
            base.Awake();

            _movement.Initialize();
        }

        protected override void Update()
        {
            base.Update();

            MovementUpdate();
        }

        private void MovementUpdate()
        {
            _movement.MoveAction?.Invoke(_moveDirection, _movementSpeed);
        }
    }
}
