using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 캐릭터의 움직임과 관련된 클래스
/// </summary>
public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private float _applySpeed;

    // direction
    private Vector3 _direction = Vector3.zero;

    // character rotate
    private float _turnSmoothTime = .1f;
    private float _turnSmoothVelocity;

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
    public Vector3 Direction => _direction;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // 실제 캐릭터를 이동시키는 함수
    public void MovementUpdate()
    {
        // 이동하지 않는 것으로 판단
        if (_direction == Vector3.zero || _direction.magnitude < .1f)
            return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camAngle;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        _controller.Move(moveDirection * _applySpeed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void SetSpeed(float speed)
    {
        _applySpeed = speed;
    }
}
