using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /// <summary>
    /// 상태머신을 제어
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        private IState _previousState;

        public StateMachine(IState defaultState)
        {
            _currentState = defaultState;
            _previousState = defaultState;

            _currentState.OperateEnter();
        }

        public IState CurrentState => _currentState;
        public IState PreviousState => _previousState;

        public void SetState(IState state, bool isLoop = false)
        {
            // 같은 상태를 다시 불러내지 않는다면
            // 들어온 상태가 현재 상태와 같으면 리턴
            if (!isLoop && _currentState == state)
                return;

            // 현재 상태를 빠져나감
            _currentState.OperateExit();

            // 이전 상태 갱신
            _previousState = _currentState;

            // 새로운 상태로 교체
            _currentState = state;

            // 새로운 상태로 진입함
            _currentState.OperateEnter();
        }

        public void DoOperatorUpdate()
        {
            _currentState.OperateUpdate();
        }

        public bool IsPreviousState(IState state)
        {
            return _previousState != null && _previousState == state;
        }

        public bool IsCurrentState(IState state)
        {
            return _currentState != null && _currentState == state;
        }
    }
}