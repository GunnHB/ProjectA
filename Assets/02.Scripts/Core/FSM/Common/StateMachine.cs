namespace ProjectA.Charactes.FSM
{
    public class StateMachine
    {
        private IState _currState;
        private IState _prevState;

        public StateMachine(IState initState)
        {
            SwitchState(initState);
        }

        public void DoOperatorUpdate()
        {
            // 현재 상태 진행 중
            _currState.OperateUpdate();
        }

        public void SwitchState(IState newState)
        {
            // 현 상태가 새로운 상태와 같으면 교체하지 않음
            if (_currState != null && newState == _currState)
                return;

            // 현재의 상태를 종료
            if (_currState != null)
                _currState.OperateExit();

            // 현재의 상태를 이전 상태로 교체한 뒤, 현재 상태를 새로운 상태로 교체
            _prevState = _currState;
            _currState = newState;

            // 교체된 상태로 진입
            _currState.OperateEnter();
        }
    }
}