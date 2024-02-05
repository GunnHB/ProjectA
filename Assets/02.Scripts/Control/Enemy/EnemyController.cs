using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BehaviorTree;

using Sirenix.OdinInspector;

public class EnemyController : MonoBehaviour
{
    [Title("[WayPoints]")]
    [SerializeField] private bool _setRandom = true;
    [SerializeField, HideIf(nameof(_setRandom))]
    protected List<GameObject> _wayPointList = new();
    [SerializeField, ShowIf(nameof(_setRandom))]
    protected float _radius = 5f;

    // Components
    private Movement _movement;
    private Animator _animator;

    // speed
    private float _speed = 3f;

    // BahaviorTree
    private BehaviorTreeRunner _btRunner;

    protected List<INode> _rootNodeList;

    protected List<INode> _alertNodeList;
    protected List<INode> _patrolNodeList;

    private bool _missTarget = false;                   // 추격 대상을 놓침

    private Vector3 _currWayPoint = Vector3.zero;       // 현재 이동 중인 지점의 벡터
    private int _currListIndex;                         // waypoint 리스트 인덱스

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();

        _btRunner = new BehaviorTreeRunner(SettingBT());
    }

    private void Update()
    {
        _btRunner.Operate();
    }

    private INode SettingBT()
    {
        return EntireNode();
    }

    protected virtual SelectorNode EntireNode()
    {
        _rootNodeList = new List<INode>();

        // _rootNodeList.Add(AlertNode());
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
        if (_missTarget)
            _missTarget = false;

        if (_setRandom)
            SetWayPointRandom();
        else
            SetWayPointByList();

        if (_currWayPoint == null || _currWayPoint == Vector3.zero)
            return INode.ENodeState.FailureState;

        var distance = Vector3.SqrMagnitude(_currWayPoint = transform.position);

        if (distance <= .1f)
        {
            _currWayPoint = Vector3.zero;

            return INode.ENodeState.SuccessState;
        }

        _movement.SetDirection(_currWayPoint);
        _movement.SetSpeed(_speed);

        _movement.MovementUpdate();

        return _currWayPoint != Vector3.zero ? INode.ENodeState.RunningState : INode.ENodeState.FailureState;
    }

    private Vector3 SetWayPointRandom()
    {
        if (_currWayPoint == Vector3.zero)
        {
            // 현재 위치에서 랜덤한 지점을 반환
            var randomSite = (UnityEngine.Random.insideUnitSphere * _radius) + transform.position;

            _currWayPoint = new Vector3(randomSite.x, 0f, randomSite.z);
        }

        return _currWayPoint;
    }

    private Vector3 SetWayPointByList()
    {
        _currListIndex++;

        if (_wayPointList.Count >= _currListIndex)
            _currListIndex = 0;

        _currWayPoint = _wayPointList[_currListIndex].transform.localPosition;

        return _currWayPoint;
    }

    private INode.ENodeState DoIdle()
    {
        throw new NotImplementedException();
    }
}
