using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BehaviorTree;

using Sirenix.OdinInspector;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        None = -1,
        Idle,
        Patrol,
        Chase,
        Attack,
        Alert,
        RunAway,
    }

    [Title("[WayPoints]")]
    [SerializeField] private bool _doNotPatrol = false;
    [SerializeField, HideIf(nameof(_doNotPatrol))]
    private bool _setRandom = true;
    [SerializeField, HideIf(nameof(_setRandom))]
    protected List<GameObject> _wayPointList = new();
    [SerializeField, ShowIf(nameof(_setRandom))]
    protected float _radius = 5f;

    [Title("[Draw gizmos]"), SerializeField]
    protected bool _drawGizmos;

    // Components
    private Movement _movement;
    private Animator _animator;

    // speed
    private float _speed = 3f;

    // for anim damp
    private float _currDamp;
    private float _targetDamp;
    private float _smoothVelocity;
    private float _smoothTime = .1f;

    protected CharacterAnimData _animData;

    // BahaviorTree
    private BehaviorTreeRunner _btRunner;

    protected List<INode> _rootNodeList;

    protected List<INode> _alertNodeList;
    protected List<INode> _patrolNodeList;

    protected EnemyState _state;

    private bool _missTarget = false;                   // 추격 대상을 놓침

    private Vector3 _currWayPoint = Vector3.zero;       // 현재 이동 중인 지점의 벡터
    private int _currListIndex;                         // waypoint 리스트 인덱스

    protected float _idleWaitTime = 0f;
    protected float _currIdleWaitTime = 0f;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();

        _animData = new CharacterAnimData();

        if (_setRandom)
            SetWayPointByRandom();

        _btRunner = new BehaviorTreeRunner(SettingBT());
    }

    private void Start()
    {
        _animData.Initialize();
    }

    private void Update()
    {
        _btRunner.Operate();

        _movement.GravityUpdate();
    }

    private INode SettingBT()
    {
        return EntireNode();
    }

    protected virtual SelectorNode EntireNode()
    {
        _rootNodeList = new List<INode>
        {
            // AlertNode(),
            PatrolNode()
        };

        return new SelectorNode(_rootNodeList);
    }

    protected virtual SequenceNode AlertNode()
    {
        _alertNodeList = new List<INode>
        {
            new ActionNode(DoAlert)
        };

        return new SequenceNode(_alertNodeList);
    }

    private INode.ENodeState DoAlert()
    {
        throw new NotImplementedException();
    }

    protected virtual SequenceNode PatrolNode()
    {
        _patrolNodeList = new List<INode>
        {
            new ActionNode(DoPatrol),
            new ActionNode(DoIdle)
        };

        return new SequenceNode(_patrolNodeList);
    }

    private INode.ENodeState DoPatrol()
    {
        if (_doNotPatrol)
            return INode.ENodeState.SuccessState;

        if (_missTarget)
            _missTarget = false;

        if (_setRandom)
            SetWayPointByRandom();
        else
            SetWayPointByList();

        if (_currWayPoint == null || _currWayPoint == Vector3.zero)
            return INode.ENodeState.FailureState;
        else if (_state == EnemyState.Idle)
            return INode.ENodeState.SuccessState;

        var distance = Vector3.SqrMagnitude(_currWayPoint - transform.position);

        if (distance <= .1f)
        {
            _currWayPoint = Vector3.zero;

            return INode.ENodeState.SuccessState;
        }

        _targetDamp = .5f;
        SetAnimDamp(_targetDamp);

        _movement.SetDirection(_currWayPoint);
        _movement.SetSpeed(_speed * _currDamp);

        _movement.MovementUpdate();

        if (_state != EnemyState.Patrol)
            SetEnemyState(EnemyState.Patrol);

        return _currWayPoint != Vector3.zero ? INode.ENodeState.RunningState : INode.ENodeState.FailureState;
    }

    private Vector3 SetWayPointByRandom()
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
        _targetDamp = 0f;
        SetAnimDamp(_targetDamp);

        if (_doNotPatrol)
        {
            SetEnemyState(EnemyState.Idle);
            return INode.ENodeState.RunningState;
        }

        if (_state != EnemyState.Idle)
        {
            SetEnemyState(EnemyState.Idle);

            if (_idleWaitTime == 0f)
                _idleWaitTime = UnityEngine.Random.Range(3f, 5f);
        }

        if (_currIdleWaitTime <= _idleWaitTime)
        {
            _currIdleWaitTime += Time.deltaTime;
            return INode.ENodeState.RunningState;
        }
        else
        {
            SetEnemyState(EnemyState.None);

            _currIdleWaitTime = 0f;
            _idleWaitTime = 0f;

            return INode.ENodeState.SuccessState;
        }
    }

    private void SetAnimDamp(float targetDamp)
    {
        _currDamp = Mathf.SmoothDamp(_currDamp, targetDamp, ref _smoothVelocity, _smoothTime);

        _animator.SetFloat(_animData.AnimParamBlendLocomotion, _currDamp);
    }

    protected void SetEnemyState(EnemyState state)
    {
        _state = state;
    }

    // 확인용 기즈모
    private void OnDrawGizmos()
    {
        if (!_drawGizmos)
            return;

        if (_currWayPoint != Vector3.zero)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _radius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_currWayPoint, .3f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _currWayPoint);
        }
    }
}
