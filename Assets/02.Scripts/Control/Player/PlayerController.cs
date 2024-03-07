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
public partial class PlayerController : MonoBehaviour
{
    private const string PLAYER_ACTION_MAP = "PlayerActionMap";
    private const string UI_ACTION_MAP = "UIActionMap";

    // public enum PlayerMode
    // {
    //     None = -1,
    //     Normal,         // 일반 모드
    //     Combat,         // 전투 모드
    // }

    // State machine
    private StateMachine _stateMachine;

    // Components
    private Movement _movement;
    private Equipment _equipment;
    private Attack _attack;
    private PlayerInput _playerInput;
    private Animator _animator;

    // datas
    protected PlayerAnimData _animData = new();         // 애니 해시

    // about move
    private float _moveSpeed = 4f;
    private Vector3 _moveDirection;
    private bool _readyToSprint = false;                // 스프린트 키 입력 여부

    // about jump
    private float _jumpForce = 1f;

    // combo attack
    private bool _isAttacking;                          // 공격 중인지
    private bool _doCombo;
    private int _attackIndex = -1;

    // private PlayerMode _playerMode;

    public UnityAction DrawWeaponAction;
    public UnityAction SheathWeaponAction;
    public UnityAction<AttackData> AttackAction;

    // Properties
    public Animator ThisAnimator => _animator;
    public PlayerAnimData ThisAnimData => _animData;
    public StateMachine ThisStateMachine => _stateMachine;
    public Vector3 ThisMoveDirection => _moveDirection;
    public float ThisMoveSpeed => _moveSpeed;

    public bool IsGrounded => _movement.IsGrounded;
    public bool IsPeak => _movement.IsPeak;
    public bool ReadyToSprint => _readyToSprint;

    public bool DoCombo => _doCombo;
    public bool IsAttacking => _isAttacking;
    public int AttackIndex => _attackIndex;

    // public PlayerMode ThisPlayerMode => _playerMode;

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
        get { return /* _equipment.IsDraw && */  (IsNormalState || IsCrouching || _isAttacking); }
    }

    private void Awake()
    {
        SetPlayerInput();
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _equipment = GetComponent<Equipment>();
        _attack = GetComponent<Attack>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _animData.Initialize();

        SetAttackData();
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

    private void SetAttackData()
    {
        var onehandDataList = new List<AttackData>();

        if (_animData.IsInit)
        {
            onehandDataList = new List<AttackData>()
            {
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimParamAttack01,
                    _transitionDuration = .1f,
                    _comboAttackTime = .5f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimParamAttack02,
                    _transitionDuration = .1f,
                    _comboAttackTime = .5f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimParamAttack03,
                    _transitionDuration = .1f,
                    _comboAttackTime = .5f,
                },
                new AttackData()
                {
                    _attackAnimHash = _animData.AnimParamAttack04,
                    _transitionDuration = .2f,
                    _comboAttackTime = .5f,
                },
            };
        }

        _attack.RegistData(GameValue.WeaponType.OneHand, onehandDataList);
    }

    public List<AttackData> GetAttackDataList()
    {
        if (ItemManager.Instance.ThisEquipmentData._itemWeaponData.IsEmpty())
            return null;

        var weaponData = ModelWeapon.Model.DataDic[ItemManager.Instance.ThisEquipmentData._itemWeaponData._itemData.ref_id];

        if (weaponData == null)
            return null;

        return _attack.AttackDic[weaponData.type];
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
}
