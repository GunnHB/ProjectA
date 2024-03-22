using UnityEngine;

namespace FSM
{
    public class AttackState : BaseState
    {
        private const string TAG_ATTACK = "Tag_Attack";

        private AttackData _currAttackData;

        private int _layerIndex = 0;
        private float _exitTime = .6f;

        private bool _isMoved = false;
        private float _moveEventTime = .3f;

        private Coroutine _corAttackMove;

        public AttackState(PlayerController player) : base(player)
        {

        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _speedAdjustments = 3f;

            _layerIndex = GetLayerIndex();

            _player.SetIsAttacking(true);
            _player.ThisAnimator.CrossFadeInFixedTime(_currAttackData._attackAnimHash, _currAttackData._transitionDuration, _layerIndex);
        }

        public override void OperateUpdate()
        {
            // base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK, _layerIndex);

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

                    _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, _layerIndex);
                    _player.IdleAction?.Invoke();
                }
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _isMoved = false;

            if (!_player.DoCombo)
                _player.StartAttackIntervalCoroutine();
            else
                _player.SetDoCombo(false);
        }

        public void SetCurrAttackData(AttackData newData)
        {
            _currAttackData = newData;
        }

        private void StartAttackMoveCoroutine(Vector3 direction)
        {
            if (_corAttackMove != null)
                CoroutineManager.Instance.ThisStopCoroutine(_corAttackMove);

            CoroutineManager.Instance.ThisStartCoroutine(_corAttackMove, Cor_AttackMovement(direction));
        }

        private System.Collections.IEnumerator Cor_AttackMovement(Vector3 direction)
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