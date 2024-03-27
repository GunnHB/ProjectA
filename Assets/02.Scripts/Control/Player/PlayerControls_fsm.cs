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

        // actions
        public UnityAction NormalAction;

        private void InitializeStates()
        {
            _normalState = new NormalState(this);

            _stateMachine = new StateMachine(_normalState);
        }

        private void RegistFSMActions()
        {
            NormalAction += OnNormalCallback;
        }

        private void UnregistFSMActions()
        {
            NormalAction -= OnNormalCallback;
        }

        #region Normal
        private void OnNormalCallback()
        {
            NormalAction?.Invoke();
        }
        #endregion
    }
}
