namespace BehaviorTree
{
    public interface INode
    {
        public enum ENodeState
        {
            RunningState,       // 진행 중
            SuccessState,       // 성공
            FailureState,       // 실패
        }

        public ENodeState Evaluate();
    }
}
