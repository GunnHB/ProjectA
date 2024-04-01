using UnityEngine;
using UnityEngine.Events;

namespace ProjectA.Charactes
{
    /// <summary>
    /// 게임 상 캐릭터들의 부모 클래스
    /// </summary>
    [RequireComponent(typeof(Movement))]
    public class CharacterControls : MonoBehaviour
    {
        // components
        protected Movement _movement;
        protected Animator _animator;
        protected CharacterAnimData _animData;

        // variables
        protected float _movementSpeed = 4f;
        protected bool _doMovement = false;
        protected float _moveAmount;
        protected float _smoothVelocity;
        protected float _smoothTime = .1f;

        // properties
        public Animator ThisAnimator => _animator;
        public CharacterAnimData ThisAnimData => _animData;
        public float MoveAmount => _moveAmount;

        protected virtual void Awake()
        {
            _movement = GetComponent<Movement>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void Update()
        {

        }
    }
}
