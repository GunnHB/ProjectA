namespace ProjectA.Charactes.FSM
{
    public class NormalState : BaseState
    {
        public NormalState(CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            SetFloat(_controls.ThisAnimData.AnimParamLocomotionValue, (_controls as PlayerControls).MoveAmount);
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}