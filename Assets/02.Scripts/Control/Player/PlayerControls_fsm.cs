using UnityEngine.Events;

using ProjectA.Charactes.FSM;

namespace ProjectA.Charactes
{
    public partial class PlayerControls : CharacterControls
    {
        // state machine
        private StateMachine _stateMachine;

        // states
        private IState _normalState;
        private IState _sprintState;

        // actions
        public UnityAction NormalAction;
        public UnityAction SprintAction;

        // variables
        private bool _readyToSprint = false;

        private void InitializeStates()
        {
            _normalState = new NormalState(this);
            _sprintState = new SprintState(this);

            _stateMachine = new StateMachine(_normalState);
        }

        private void RegistFSMActions()
        {
            NormalAction += OnNormalCallback;
            SprintAction += OnSprintCallback;
        }

        private void UnregistFSMActions()
        {
            NormalAction -= OnNormalCallback;
            SprintAction -= OnSprintCallback;
        }

        #region Normal
        private void OnNormalCallback()
        {
            // NormalAction?.Invoke();
            _stateMachine.SwitchState(_normalState);
        }
        #endregion

        #region Sprint
        private void OnSprintCallback()
        {
            _stateMachine.SwitchState(_sprintState);
            // SprintAction?.Invoke();
        }
        #endregion
    }
}
