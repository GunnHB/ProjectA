using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 왼쪽에서 오른쪽으로 판단을 진행하면서 모든 자식 노드가 성공이면 성공을 반환
    /// </summary> <summary>
    /// 하나라도 실패면 바로 실패를 반환
    /// </summary>
    public class SequenceNode : INode
    {
        private List<INode> _childNodes;

        public SequenceNode(List<INode> childNodes)
        {
            _childNodes = childNodes;
        }

        public INode.ENodeState Evaluate()
        {
            if (_childNodes == null || _childNodes.Count == 0)
                return INode.ENodeState.FailureState;

            for (int index = 0; index < _childNodes.Count; index++)
            {
                switch (_childNodes[index].Evaluate())
                {
                    case INode.ENodeState.RunningState:
                        return INode.ENodeState.RunningState;
                    case INode.ENodeState.SuccessState:
                        continue;
                    case INode.ENodeState.FailureState:
                        return INode.ENodeState.FailureState;
                }
            }

            return INode.ENodeState.SuccessState;
        }
    }
}