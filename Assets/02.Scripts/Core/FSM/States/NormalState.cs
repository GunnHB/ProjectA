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

            SetFloat(_controls.ThisAnimData.AnimParamVerticalValue, _controls.GetVerticalValue());
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}
