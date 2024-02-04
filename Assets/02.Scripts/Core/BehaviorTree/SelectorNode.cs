using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 왼쪽에서 오른쪽으로 판단을 진행하면서 성공이나 진행 중을 반환하면 즉시 결과를 반환
    /// </summary>
    public class SelectorNode : INode
    {
        private List<INode> _childNodes;

        public SelectorNode(List<INode> childNodes)
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
                        return INode.ENodeState.SuccessState;
                }
            }

            return INode.ENodeState.FailureState;
        }
    }
}
