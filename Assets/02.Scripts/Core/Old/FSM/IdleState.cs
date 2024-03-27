namespace FSM
{
    /// <summary>
    /// 기본 움직임에 대한 state
    /// </summary>
    public class IdleState : BaseState
    {
        public IdleState(PlayerController player) : base(player)
        {

        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _speedAdjustments = 1f;

            // 레이어에 따른 애니메이션 전환
            CrossFade(_player.ThisAnimData.AnimNameLocomotion);

            // 현재 locomotion 값을 가져옴
            _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // 플레이어의 이동에 따라 애니의 인자값이 갱신됨
            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendLocomotion);

            if (!_player.IsGrounded)
                _player.FallingAction?.Invoke();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}