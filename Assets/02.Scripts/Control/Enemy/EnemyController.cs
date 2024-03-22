using System;
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
        MissTarget,
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
    private FieldOfView _fov;

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

    protected List<INode> _attackNodeList;              // 공격 노드 리스트
    protected List<INode> _readyToAttackNodeList;       // 전투 준비 노드 리스트
    protected List<INode> _chaseNodeList;               // 추격 노드 리스트
    protected List<INode> _alertNodeList;               // 타겟 놓침 노드 리스트
    protected List<INode> _patrolNodeList;              // 순찰 노드 리스트

    protected EnemyState _state;

    private bool _missTarget = false;                   // 추격 대상을 놓침

    private Vector3 _currWayPoint = Vector3.zero;       // 현재 이동 중인 지점의 벡터
    private int _currListIndex;                         // waypoint 리스트 인덱스

    protected float _idleWaitTime = 0f;
    protected float _currIdleWaitTime = 0f;

    private Transform _targetTr;
    private bool _findTarget = false;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();
        _fov = GetComponent<FieldOfView>();

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
            AttackNode(),
            ReadyToAttack(),
            ChaseNode(),
            PatrolNode()
        };

        return new SelectorNode(_rootNodeList);
    }

    #region AttackNode
    protected virtual SequenceNode AttackNode()
    {
        _chaseNodeList = new List<INode>
        {
            new ActionNode(CheckAttaking),
            new ActionNode(CheckAttackRange),
            new ActionNode(DoAttack),
        };

        return new SequenceNode(_chaseNodeList);
    }

    private INode.ENodeState CheckAttaking()
    {
        if (_targetTr == null)
            return INode.ENodeState.FailureState;

        // 현재 공격 중이면 running 반환
        if (_state == EnemyState.Attack)
            return INode.ENodeState.RunningState;

        return INode.ENodeState.SuccessState;
    }

    private INode.ENodeState CheckAttackRange()
    {
        return INode.ENodeState.FailureState;
    }

    private INode.ENodeState DoAttack()
    {
        return INode.ENodeState.FailureState;
    }
    #endregion

    #region ReadyToAttack
    private SequenceNode ReadyToAttack()
    {
        _readyToAttackNodeList = new List<INode>()
        {
            new ActionNode(DoCombatIdle),
            new ActionNode(DoCombatPatrol),
        };

        return new SequenceNode(_readyToAttackNodeList);
    }

    // 공격 대기
    private INode.ENodeState DoCombatIdle()
    {
        return INode.ENodeState.FailureState;
    }

    private INode.ENodeState DoCombatPatrol()
    {
        return INode.ENodeState.FailureState;
    }
    #endregion

    #region ChaseNode
    protected virtual SequenceNode ChaseNode()
    {
        _chaseNodeList = new List<INode>
        {
            new ActionNode(DoCheckDetectTarget),
            new ActionNode(DoChaseTarget),
        };

        return new SequenceNode(_chaseNodeList);
    }

    private INode.ENodeState DoCheckDetectTarget()
    {
        if (_fov == null)
            return INode.ENodeState.FailureState;

        _fov.FindVisibleTargets();
        _targetTr = _fov.NearestTarget;

        if (_targetTr != null)
        {
            _findTarget = true;
            return INode.ENodeState.SuccessState;
        }
        else
        {
            if (_findTarget)
                _findTarget = false;
        }

        return INode.ENodeState.FailureState;
    }

    private INode.ENodeState DoChaseTarget()
    {
        if (_targetTr == null)
            return INode.ENodeState.FailureState;

        SwitchEnemyState(EnemyState.Chase);

        var distance = Vector3.SqrMagnitude(this.transform.position - _targetTr.position);

        if (distance < Mathf.Pow(_fov.MeleeAttackRange, 2))
            return INode.ENodeState.SuccessState;

        MoveToTarget(_targetTr);

        return INode.ENodeState.RunningState;
    }

    private void MoveToTarget(Transform target)
    {
        _movement.SetDirection(target.position);
        _movement.SetSpeed(_speed);

        _movement.MovementUpdate();
    }
    #endregion

    #region AlertNode
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
        // throw new NotImplementedException();
        return INode.ENodeState.FailureState;
    }
    #endregion

    #region PatrolNode
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
            SwitchEnemyState(EnemyState.Patrol);

        return _currWayPoint != Vector3.zero ? INode.ENodeState.RunningState : INode.ENodeState.FailureState;
    }

    private INode.ENodeState DoIdle()
    {
        _targetDamp = 0f;
        SetAnimDamp(_targetDamp);

        if (_doNotPatrol)
        {
            SwitchEnemyState(EnemyState.Idle);
            return INode.ENodeState.RunningState;
        }

        if (_state != EnemyState.Idle)
        {
            SwitchEnemyState(EnemyState.Idle);

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
            SwitchEnemyState(EnemyState.None);

            _currIdleWaitTime = 0f;
            _idleWaitTime = 0f;

            return INode.ENodeState.SuccessState;
        }
    }
    #endregion

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

    private void SetAnimDamp(float targetDamp)
    {
        _currDamp = Mathf.SmoothDamp(_currDamp, targetDamp, ref _smoothVelocity, _smoothTime);

        _animator.SetFloat(_animData.AnimParamBlendLocomotion, _currDamp);
    }

    protected void SwitchEnemyState(EnemyState state)
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
