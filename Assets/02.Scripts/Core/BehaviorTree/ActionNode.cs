using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class ActionNode : INode
    {
        private Func<INode.ENodeState> _onUpdate;

        public ActionNode(Func<INode.ENodeState> onUpdate)
        {
            _onUpdate = onUpdate;
        }

        public INode.ENodeState Evaluate() => _onUpdate?.Invoke() ?? INode.ENodeState.FailureState;
    }
}