using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class GroundCheker : MonoBehaviour
{
    [Title("[Boxcast properties]")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _boxSize;
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _groundMask;

    [Title("[Debug]")]
    [SerializeField] private bool _drawGizmo;

    private CharacterController _controller;
    private Vector3 _gravityVelocity;

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

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void GravityUpdate()
    {
        _gravityVelocity.y += (-9.81f) * Time.deltaTime;
        _controller.Move(_gravityVelocity * Time.deltaTime);
    }

    // ground check gizmo
    private void OnDrawGizmos()
    {
        if (!_drawGizmo)
            return;

        Gizmos.color = IsGrounded ? Color.red : Color.blue;
        Gizmos.DrawCube(_targetTransform.position - transform.up * _maxDistance, _boxSize);

        Debug.Log("테스트 커밋용");
    }
}
