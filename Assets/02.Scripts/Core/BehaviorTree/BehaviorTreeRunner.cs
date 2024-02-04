using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        private INode _rootNode;

        public BehaviorTreeRunner(INode rootNode)
        {
            _rootNode = rootNode;
        }

        public void Operate()
        {
            _rootNode.Evaluate();
        }
    }
}
