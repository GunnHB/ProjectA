using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using FSM;
using UnityEngine.Events;

/// <summary>
/// 플레이어의 총괄 클래스
/// </summary>
public partial class PlayerController : MonoBehaviour, IAttack, IDamage
{
    private const string PLAYER_ACTION_MAP = "PlayerActionMap";
    private const string UI_ACTION_MAP = "UIActionMap";

    // State machine
    private StateMachine _stateMachine;

    // Components
    private Movement _movement;
    private Equipment _equipment;
    private PlayerInput _playerInput;
    private PlayerCustomizer _customizer;
    private Animator _animator;

    // datas
    protected PlayerAnimData _animData = new();         // 애니 해시

    // about move
    private float _moveSpeed = 4f;
    private Vector3 _moveDirection;
    private float _targetDamp;
    private bool _readyToSprint = false;                // 스프린트 키 입력 여부

    // about jump
    private float _jumpForce = 1f;

    // combo attack
    private bool _isAttacking;                          // 공격 중인지
    private bool _doCombo;
    private bool _lastAttackIndex;                      // 공격 데이터 중 마지막 값인지
    private int _attackIndex = -1;
    private Dictionary<GameValue.WeaponType, List<AttackData>> _attackDataDic;

    public UnityAction IdleAction;
    public UnityAction<bool> SprintAction;
    public UnityAction SprintCancelAction;

    public UnityAction JumpAction;
    public UnityAction FallingAction;
    public UnityAction LandingAction;

    public UnityAction CrouchAction;

    public UnityAction FocusAction;

    public UnityAction DrawWeaponAction;
    public UnityAction SheathWeaponAction;

    public UnityAction<AttackData> AttackAction;

    // Properties
    public Animator ThisAnimator => _animator;
    public PlayerAnimData ThisAnimData => _animData;
    public StateMachine ThisStateMachine => _stateMachine;
    public float ThisMoveSpeed => _moveSpeed;

    public bool IsMoving => _moveDirection != Vector3.zero;         // 이동 입력이 있는지
    public bool IsGrounded => _movement.IsGrounded;                 // 땅에 발이 닿았는지
    public bool IsPeak => _movement.IsPeak;                         // 최고 높이를 찍었는지
    public bool ReadyToSprint => _readyToSprint;                    // 달리기 입력이 들어갔는지

    public bool DoCombo => _doCombo;
    public bool IsAttacking => _isAttacking;
    public int AttackIndex => _attackIndex;

    public float TargetDamp => _targetDamp;

    public Dictionary<GameValue.WeaponType, List<AttackData>> AttackDataDic => _attackDataDic;
    // public Queue<AttackData> AttackQueue => _attackQueue;

    // 마지막 공격 후 다음 공격의 텀을 가지기 위한 코루틴
    private Coroutine _attackintervalCoroutin;
    private bool _runningCoroutine;

    private bool _doNotMovePlayer = false;                          // 플레이어의 이동을 강제로 막는 플래그

    // 일반적인 움직임의 상태 (대기, 걷기, 달리기...)
    public bool IsNormalState
    {
        get
        {
            return _stateMachine.IsCurrentState(_idleState) ||
                    _stateMachine.IsCurrentState(_walkState) ||
                    _stateMachine.IsCurrentState(_sprintState);
        }
    }

    // 공중에 있는 상태
    public bool IsOnAir
    {
        get
        {
            return _stateMachine.IsCurrentState(_jumpState) ||
                _stateMachine.IsCurrentState(_fallingState) ||
                _stateMachine.IsCurrentState(_landingState);
        }
    }

    // 웅크린 상태
    public bool IsCrouching => _stateMachine.IsCurrentState(_crouchState);

    // 공격이 가능한 상태
    public bool CanAttack
    {
        get { return IsNormalState || IsCrouching || _isAttacking; }
    }

    private void Awake()
    {
        SetPlayerInput();
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _equipment = GetComponent<Equipment>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _customizer = GetComponent<PlayerCustomizer>();

        _animData.Initialize();

        RegistAttackData();
        SetIsAttacking(false);

        RegistStateDictionary();

        GameManager.Instance.InGameModeAction -= InGameModeAction;
        GameManager.Instance.UIModeAction -= UIModeAction;

        GameManager.Instance.InGameModeAction += InGameModeAction;
        GameManager.Instance.UIModeAction += UIModeAction;
    }

    private void Update()
    {
        _stateMachine.DoOperatorUpdate();

        FindDropItemObject();
    }

    private void FixedUpdate()
    {
        if (!_doNotMovePlayer)
            _movement.MovementUpdate();

        _movement.GravityUpdate();
    }

    private void FindDropItemObject()
    {
        // [1 << layermask] 요 형태로 해야 인식함
        var colliders = Physics.OverlapSphere(transform.localPosition, .3f, 1 << GameManager.Instance.ItemMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out DropItemObject dropItem))
                Debug.Log(dropItem);        // 아이템 습득 ui가 떠야함
        }
    }

    public void SetMovementSpeed(float speed)
    {
        _movement.SetSpeed(speed);
    }

    public void DoJump()
    {
        _movement.Jump(_jumpForce);
    }

    public void SetDoCombo(bool active)
    {
        _doCombo = active;
    }

    private void InGameModeAction()
    {
        _action.UIActionMap.Disable();

        _playerInput.SwitchCurrentActionMap(PLAYER_ACTION_MAP);

        _action.PlayerActionMap.Enable();
    }

    private void UIModeAction()
    {
        _action.PlayerActionMap.Disable();

        _playerInput.SwitchCurrentActionMap(UI_ACTION_MAP);

        _action.UIActionMap.Enable();
    }

    public void SetIsAttacking(bool isActive = true)
    {
        _isAttacking = isActive;
    }

    public List<AttackData> GetAttackDataList()
    {
        if (ItemManager.Instance.EquipWeaponData._invenItemData.IsEmpty())
            return null;

        var weaponData = ModelWeapon.Model.DataDic[ItemManager.Instance.EquipWeaponData._invenItemData._itemData.ref_id];

        if (weaponData == null)
            return null;

        return _attackDataDic[weaponData.type];
    }

    public void ResetAttackIndex()
    {
        _attackIndex = -1;
    }

    public void ClearActions()
    {
        DrawWeaponAction = null;
        SheathWeaponAction = null;
        AttackAction = null;
    }

    public void RegistAttackData()
    {
        _attackDataDic = new();

        var onehandDataList = new List<AttackData>();
        var twoHandDataList = new List<AttackData>();

        if (_animData.IsInit)
        {
            onehandDataList = new List<AttackData>()
            {
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimNameAttack01,
                    _transitionDuration = .1f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimNameAttack02,
                    _transitionDuration = .1f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimNameAttack03,
                    _transitionDuration = .1f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimNameAttack04,
                    _transitionDuration = .2f,
                },
            };

            twoHandDataList = new List<AttackData>()
            {

            };
        }

        _attackDataDic.Add(GameValue.WeaponType.OneHand, onehandDataList);
        _attackDataDic.Add(GameValue.WeaponType.TwoHand, twoHandDataList);
    }

    public void StartCheckHitCollider()
    {
        if (ItemManager.Instance.EquipWeaponData._itemPrefab == null)
            return;

        // 시작되는 동안은 콜라이더 기능 켜기
        if (ItemManager.Instance.EquipWeaponData._itemPrefab.TryGetComponent(out WeaponItem weaponItem))
            weaponItem.EnableCollider();
    }

    public void EndCheckHitCollider()
    {
        if (ItemManager.Instance.EquipWeaponData._itemPrefab == null)
            return;

        // 끝나면 콜라이더 기능 끄기
        if (ItemManager.Instance.EquipWeaponData._itemPrefab.TryGetComponent(out WeaponItem weaponItem))
            weaponItem.DisableCollider();
    }

    public void GetDamaged(int damagedValue)
    {
        Debug.Log($"{damagedValue} damaged!");
    }

    public void StartAttackIntervalCoroutine()
    {
        if (_attackintervalCoroutin != null)
        {
            StopCoroutine(_attackintervalCoroutin);
            _attackintervalCoroutin = null;
        }

        _attackintervalCoroutin = StartCoroutine(nameof(Cor_UpdateAttackInterval));
    }

    private IEnumerator Cor_UpdateAttackInterval()
    {
        float _currTime = 0f;
        float targetTime = 1.2f;

        _runningCoroutine = true;

        while (_currTime < targetTime)
        {
            _currTime += Time.deltaTime / targetTime;
            Debug.Log(_currTime);
            yield return null;
        }

        _isAttacking = false;
        _runningCoroutine = false;
        _lastAttackIndex = false;
    }

    public void SetDoNotMovePlayer(bool active)
    {
        _doNotMovePlayer = active;
    }
}
