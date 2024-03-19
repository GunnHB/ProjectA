using System.Linq;
using FSM;

public class AttackState : BaseState
{
    private const string TAG_ATTACK = "Tag_Attack";

    private AttackData _currAttackData;

    private int _layerIndex = 0;
    private float _exitTime = .7f;

    private bool _isLastAttackIndex = false;

    public AttackState(PlayerController player) : base(player)
    {

    }

    public override void OperateEnter()
    {
        base.OperateEnter();

        _layerIndex = GetLayerIndex();

        _player.SetIsAttacking(true);
        _player.ThisAnimator.CrossFadeInFixedTime(_currAttackData._attackAnimHash, _currAttackData._transitionDuration, _layerIndex);

        _isLastAttackIndex = _player.GetAttackDataList().Last() == _currAttackData;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        float normalizedTime = GetNormalizedTimeByTag(TAG_ATTACK, _layerIndex);

        if (normalizedTime > _exitTime)
        {
            // if (_player.AttackQueue.Count > 0)
            //     _player.AttackAction?.Invoke(_player.AttackQueue.Dequeue());
            // else
            // {
            if (_player.DoCombo)
                _player.AttackAction?.Invoke(_player.GetAttackDataList()[_player.AttackIndex]);
            else
            {
                _player.ResetAttackIndex();

                _player.ThisAnimator.CrossFadeInFixedTime(_player.ThisAnimData.AnimNameLocomotion, .1f, _layerIndex);
                _player.IdleAction?.Invoke();
            }
            // }
        }
    }

    public override void OperateExit()
    {
        base.OperateExit();

        if (!_player.DoCombo)
            _player.StartAttackIntervalCoroutine();
        else
            _player.SetDoCombo(false);

        // if (_player.AttackQueue.Count == 0)
        //     _player.StartAttackIntervalCoroutine();
    }

    public void SetCurrAttackData(AttackData newData)
    {
        _currAttackData = newData;
    }
}
