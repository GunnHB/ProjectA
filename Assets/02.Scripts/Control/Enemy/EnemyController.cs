using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BehaviorTree;
using System;

public class EnemyController : MonoBehaviour
{
    // Components
    private Movement _movement;
    private Animator _animator;

    // speed
    private float _speed = 3f;

    // BahaviorTree
    private BehaviorTreeRunner _runner;

    protected List<INode> _rootNodeList;

    protected List<INode> _alertNodeList;
    protected List<INode> _patrolNodeList;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();

        _runner = new BehaviorTreeRunner(SettingBT());
    }

    private void Update()
    {
        _runner.Operate();
    }

    private INode SettingBT()
    {
        // throw new NotImplementedException();
        return EntireNode();
    }

    protected virtual SelectorNode EntireNode()
    {
        _rootNodeList = new List<INode>();

        _rootNodeList.Add(AlertNode());
        _rootNodeList.Add(PatrolNode());

        return new SelectorNode(_rootNodeList);
    }

    protected virtual SequenceNode AlertNode()
    {
        _alertNodeList = new List<INode>();

        _alertNodeList.Add(new ActionNode(DoAlert));

        return new SequenceNode(_alertNodeList);
    }

    private INode.ENodeState DoAlert()
    {
        throw new NotImplementedException();
    }

    protected virtual SequenceNode PatrolNode()
    {
        _patrolNodeList = new List<INode>();

        _patrolNodeList.Add(new ActionNode(DoPatrol));
        _patrolNodeList.Add(new ActionNode(DoIdle));

        return new SequenceNode(_patrolNodeList);
    }

    private INode.ENodeState DoPatrol()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState DoIdle()
    {
        throw new NotImplementedException();
    }
}
