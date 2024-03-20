using System.Linq;

namespace FSM
{
    public class AttackState : BaseState
    {
        private const string TAG_ATTACK = "Tag_Attack";

        private AttackData _currAttackData;

        private int _layerIndex = 0;
        private float _exitTime = .3f;

        public AttackState(PlayerController player) : base(player)
        {

        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _player.SetDoNotMovePlayer(true);

            _layerIndex = GetLayerIndex();

            _player.SetIsAttacking(true);
            _player.ThisAnimator.CrossFadeInFixedTime(_currAttackData._attackAnimHash, _currAttackData._transitionDuration, _layerIndex);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK, _layerIndex);

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

            _player.SetDoNotMovePlayer(false);

            if (!_player.DoCombo)
                _player.StartAttackIntervalCoroutine();
            else
                _player.SetDoCombo(false);
        }

        public void SetCurrAttackData(AttackData newData)
        {
            _currAttackData = newData;
        }
    }
}