using UnityEngine;

namespace ProjectA.Charactes
{
    public partial class PlayerControls : CharacterControls
    {
        // variables
        private float _verticalInput;
        private float _horizontalInput;
        private Vector3 _moveDirection;

        protected override void Awake()
        {
            base.Awake();

            _animData = new PlayerAnimData();

            _animData.InitializeDatas();
            _movement.Initialize();

            InitializeStates();
        }

        protected override void Update()
        {
            base.Update();

            MovementUpdate();

            _stateMachine.DoOperatorUpdate();
        }

        private void MovementUpdate()
        {
            _movement.MoveAction?.Invoke(_moveDirection, _movementSpeed);
        }
    }
}
