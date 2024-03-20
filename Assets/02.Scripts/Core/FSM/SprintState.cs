namespace FSM
{
    /// <summary>
    /// 달리기 state 
    /// </summary>
    public class SprintState : BaseState
    {
        public SprintState(PlayerController player) : base(player)
        {

        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            // 현재 locomotion 값을 가져옴
            _currLengthOfVector = GetFloatParam(_player.ThisAnimData.AnimParamBlendLocomotion);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // 플레이어의 이동에 따라 애니의 인자값이 갱신됨
            SetPlayerMovement(_player.ThisAnimData.AnimParamBlendLocomotion);

            // 달리는 중에 멈추면 달리기 취소
            if (!_player.IsMoving)
                _player.SprintCancelAction?.Invoke();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}