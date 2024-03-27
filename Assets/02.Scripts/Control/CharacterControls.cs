using UnityEngine;

using Sirenix.OdinInspector;

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

        // variables
        protected float _movementSpeed = 4f;

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
