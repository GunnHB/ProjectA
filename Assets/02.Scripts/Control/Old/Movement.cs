using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;
using Unity.Mathematics;

/// <summary>
/// 캐릭터의 움직임과 관련된 클래스
/// </summary>
public class Movement : MonoBehaviour
{
    [Title("[Ground check]")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _boxSize;
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _groundMask;

    [Title("[Debug]")]
    [SerializeField] private bool _drawGizmo;

    // Components
    private CharacterController _controller;

    // speed
    private float _applySpeed;

    // direction
    private Vector3 _direction = Vector3.zero;  // 외부에서 입력되는 값으로 갱신
    private Vector3 _gravityVelocity;

    // character rotate
    private float _turnSmoothTime = .1f;
    private float _turnSmoothVelocity;

    // jump peak
    private bool _wasPeaked = false;        // 고점 여러 번 체크되는 것 방지

    public UnityEngine.Events.UnityAction<Vector3, float> MovementAction;

    // properties
    private float _camAngle
    {
        get
        {
            if (GetComponent<PlayerController>())
                return Camera.main.transform.eulerAngles.y;
            else
                return 0f;
        }
    }

    public bool IsGrounded
    {
        get
        {
            if (_targetTransform == null)
                return false;
            else
                return Physics.BoxCast(_targetTransform.position, _boxSize, -transform.up, transform.rotation, _maxDistance, _groundMask);
        }
    }

    public bool IsPeak
    {
        get
        {
            CheckPeak();

            if (_gravityVelocity.sqrMagnitude <= .1f && !_wasPeaked)
            {
                _wasPeaked = true;
                return true;
            }
            else
                return false;
        }
    }

    public Vector3 Direction => _direction;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        MovementAction += MoveCallback;
    }

    private void MoveCallback(Vector3 direction, float speed)
    {
        if (direction == Vector3.zero || direction.magnitude < .1f)
            return;

        float targetAngle = GetTargetAngleV2(direction);
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        SetRotation(angle);
        MoveToV2(targetAngle, direction, speed);
    }

    // 실제 캐릭터를 이동시키는 함수
    public void MovementUpdate()
    {
        // 이동하지 않는 것으로 판단
        if (_direction == Vector3.zero || _direction.magnitude < .1f)
            return;

        float targetAngle = GetTargetAngle();
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        SetRotation(angle);
        MoveTo(targetAngle);
    }

    private float GetTargetAngle()
    {
        if (TryGetComponent(out PlayerController player))
            return Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camAngle;
        else
        {
            var directionToTarget = (_direction - transform.position).normalized;

            return Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        }
    }

    private float GetTargetAngleV2(Vector3 direction)
    {
        if (TryGetComponent(out PlayerController player))
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        else
        {
            var directionToTarget = (direction - transform.position).normalized;

            return Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        }
    }

    private void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    }

    private void MoveTo(float targetAngle)
    {
        Vector3 moveDirection;

        if (TryGetComponent(out PlayerController player))
            moveDirection = Quaternion.Euler(new Vector3(0f, targetAngle, 0f)) * Vector3.forward;
        else
            moveDirection = (_direction - transform.position).normalized;

        _controller.Move(moveDirection * _applySpeed * Time.deltaTime);
    }

    private void MoveToV2(float targetAngle, Vector3 direction, float speed)
    {
        Vector3 moveDirection;

        if (TryGetComponent(out PlayerController player))
            moveDirection = Quaternion.Euler(new Vector3(0f, targetAngle, 0f)) * Vector3.forward;
        else
            moveDirection = (direction - transform.position).normalized;

        _controller.Move(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void SetSpeed(float speed)
    {
        _applySpeed = speed;
    }

    public void GravityUpdate()
    {
        _gravityVelocity.y += GameValue._gravity * Time.deltaTime;

        if (IsGrounded && _gravityVelocity.y < 0f)
            _gravityVelocity.y = Mathf.Max(_gravityVelocity.y, -2f);

        _controller.Move(_gravityVelocity * Time.deltaTime);
    }

    public void Jump(float force)
    {
        if (IsGrounded)
            _gravityVelocity.y = Mathf.Sqrt(force * -2f * GameValue._gravity);
    }

    private void CheckPeak()
    {
        if (IsGrounded)
            _wasPeaked = false;
    }

    // ground check gizmo
    private void OnDrawGizmos()
    {
        if (!_drawGizmo)
            return;

        Gizmos.color = IsGrounded ? Color.red : Color.blue;
        Gizmos.DrawCube(_targetTransform.position - transform.up * _maxDistance, _boxSize);
    }
}
