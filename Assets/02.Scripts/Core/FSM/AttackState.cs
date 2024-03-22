using System.Collections;
using System.Linq;
using UnityEngine;

namespace FSM
{
    public class AttackState : BaseState
    {
        private const string TAG_ATTACK = "Tag_Attack";

        private AttackData _currAttackData;

        private float _exitTime = .6f;

        private bool _isMoved = false;
        private float _moveEventTime = .3f;

        private float _intervalTime = .7f;

        private Coroutine _corAttackInterval;
        private Coroutine _corAttackMove;

        public AttackState(PlayerController player) : base(player)
        {

        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _speedAdjustments = 3f;

            SetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion, 0);

            _player.SetIsAttacking(true);
            _player.SetIsLastAttackIndex(_player.GetAttackDataList().Last() == _currAttackData);

            CrossFade(_currAttackData._attackAnimHash, _currAttackData._transitionDuration);
        }

        public override void OperateUpdate()
        {
            // base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK);

            if (!_isMoved && normalizedTime > _moveEventTime)
            {
                if (_player.InputDirection == Vector3.zero)
                    StartAttackMoveCoroutine(Vector3.forward);
                else
                    StartAttackMoveCoroutine(_player.InputDirection);

                _isMoved = true;
            }

            if (normalizedTime > _exitTime)
            {
                if (_player.DoCombo)
                    _player.AttackAction?.Invoke(_player.GetAttackDataList()[_player.AttackIndex]);
                else
                {
                    _player.ResetAttackIndex();

                    CrossFade(_player.ThisAnimData.AnimNameLocomotion);
                    _player.IdleAction?.Invoke();
                }
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _isMoved = false;

            if (!_player.DoCombo)
                StartAttackIntervalCoroutine();
            else
                _player.SetDoCombo(false);
        }

        public void SetCurrAttackData(AttackData newData)
        {
            _currAttackData = newData;
        }

        private void StartAttackIntervalCoroutine()
        {
            if (_corAttackInterval != null)
                CoroutineManager.Instance.ThisStopCoroutine(_corAttackInterval);

            CoroutineManager.Instance.ThisStartCoroutine(_corAttackInterval, Cor_AttackInterval());
        }

        private IEnumerator Cor_AttackInterval()
        {
            float _currTime = 0f;

            while (_currTime < _intervalTime)
            {
                _currTime += Time.deltaTime;

                yield return null;
            }

            _player.SetIsAttacking(false);
            _player.SetIsLastAttackIndex(false);
        }

        // public void StartAttackIntervalCoroutine()
        // {
        //     if (_attackintervalCoroutin != null)
        //     {
        //         StopCoroutine(_attackintervalCoroutin);
        //         _attackintervalCoroutin = null;
        //     }

        //     _attackintervalCoroutin = StartCoroutine(nameof(Cor_UpdateAttackInterval));
        // }

        // private IEnumerator Cor_UpdateAttackInterval()
        // {
        //     float _currTime = 0f;
        //     float targetTime = .8f;

        //     _runningCoroutine = true;

        //     while (_currTime < targetTime)
        //     {
        //         _currTime += Time.deltaTime / targetTime;

        //         yield return null;
        //     }

        //     _isAttacking = false;
        //     _runningCoroutine = false;
        //     _lastAttackIndex = false;
        // }

        private void StartAttackMoveCoroutine(Vector3 direction)
        {
            if (_corAttackMove != null)
                CoroutineManager.Instance.ThisStopCoroutine(_corAttackMove);

            CoroutineManager.Instance.ThisStartCoroutine(_corAttackMove, Cor_AttackMovement(direction));
        }

        private IEnumerator Cor_AttackMovement(Vector3 direction)
        {
            float _currTime = 0f;

            while (_currTime < .1f)
            {
                _player.DoMove(direction, _speedAdjustments);
                _currTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}