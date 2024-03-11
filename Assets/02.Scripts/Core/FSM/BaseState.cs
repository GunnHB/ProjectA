using UnityEngine;

namespace FSM
{
    public class BaseState : IState
    {
        protected PlayerController _player;
        protected StateMachine _stateMachine;

        protected float _currLengthOfVector;
        protected float _smoothVelocity;
        protected float _smoothTime = .15f;
        protected float _dampTarget;

        private UICheckStateHUD _stateHud;

        public BaseState(PlayerController player)
        {
            _player = player;

            _stateHud = UIManager.Instance.OpenUI<UICheckStateHUD>();
        }

        public virtual void OperateEnter()
        {
            // Debug.Log($"{this} enter");

            if (_stateHud != null)
                _stateHud.SetStateText(this);

            if (_stateMachine == null)
                _stateMachine = _player.ThisStateMachine;
        }

        public virtual void OperateExit()
        {
            // Debug.Log($"{this} exit");
        }

        public virtual void OperateUpdate()
        {
            // Debug.Log($"{this} update");
        }

        /// <summary>
        /// 캐릭터의 애니와 속도를 세팅
        /// </summary>
        protected void SetPlayerMovement(int animHash)
        {
            _currLengthOfVector = Mathf.SmoothDamp(_currLengthOfVector, _dampTarget, ref _smoothVelocity, _smoothTime);

            SetFloatParam(animHash, _currLengthOfVector);
            _player.SetMovementSpeed(_currLengthOfVector * _player.ThisMoveSpeed);
        }

        protected bool GetPreviousState(IState state)
        {
            return _stateMachine != null && _stateMachine.IsPreviousState(state);
        }

        protected void StartAnimation(int animHash)
        {
            _player.ThisAnimator.SetBool(animHash, true);
        }

        protected void StopAnimation(int animHash)
        {
            _player.ThisAnimator.SetBool(animHash, false);
        }

        protected void SetTriggerAnimation(int animHash)
        {
            _player.ThisAnimator.SetTrigger(animHash);
        }

        protected void SetFloatParam(int animHash, float value)
        {
            _player.ThisAnimator.SetFloat(animHash, value);
        }

        protected float GetFloatParam(int animHash)
        {
            return _player.ThisAnimator.GetFloat(animHash);
        }

        protected float GetCurrentClipLength(int layerIndex)
        {
            return _player.ThisAnimator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.length;
        }

        protected float GetCurrentClipSpeed(int layerIndex)
        {
            return _player.ThisAnimator.GetCurrentAnimatorStateInfo(layerIndex).speed;
        }

        protected void CrossFadeInFixedUpdate(int animHash, float transitionDuration = .1f)
        {
            _player.ThisAnimator.CrossFadeInFixedTime(animHash, transitionDuration);
        }

        protected float GetNormalizedTimeByTag(string tagName, int layerIndex = 0)
        {
            var currentInfo = GetCurrentAnimatorStateInfo(layerIndex);
            var nextInfo = GetNextAniomatorStateInfo(layerIndex);

            if (_player.ThisAnimator.IsInTransition(layerIndex) && nextInfo.IsTag(tagName))
                return nextInfo.normalizedTime;
            else if (!_player.ThisAnimator.IsInTransition(layerIndex) && currentInfo.IsTag(tagName))
                return currentInfo.normalizedTime;

            return 0f;
        }

        protected AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex = 0)
        {
            return _player.ThisAnimator.GetCurrentAnimatorStateInfo(layerIndex);
        }

        protected AnimatorStateInfo GetNextAniomatorStateInfo(int layerIndex = 0)
        {
            return _player.ThisAnimator.GetNextAnimatorStateInfo(layerIndex);
        }

        protected void SetWeaponType(ref GameValue.WeaponType type)
        {
            if (ItemManager.Instance.ThisEquipmentData._itemWeaponData.IsEmpty())
                return;

            var itemData = ItemManager.Instance.ThisEquipmentData._itemWeaponData._itemData;

            if (itemData == null)
                return;

            var weaponData = ModelWeapon.Model.DataDic[itemData.ref_id];

            if (weaponData == null)
                return;

            type = weaponData.type;
        }

        protected int GetLayerIndex(GameValue.WeaponType type, bool _animDone)
        {
            string layerName = string.Empty;

            switch (type)
            {
                case GameValue.WeaponType.OneHand:
                    {
                        if (_animDone)
                            layerName = GameValue.ANIM_LAYER_ONEHAND;
                        else
                            layerName = _player.IsMoving ? GameValue.ANIM_LAYER_ONEHAND_UPPER : GameValue.ANIM_LAYER_ONEHAND;
                    }
                    break;
                case GameValue.WeaponType.TwoHand:
                    {
                        if (_animDone)
                            layerName = GameValue.ANIM_LAYER_TOWHAND;
                        else
                            layerName = _player.IsMoving ? GameValue.ANIM_LAYER_TOWHAND_UPPER : GameValue.ANIM_LAYER_TOWHAND;
                    }
                    break;
            }

            return _player.ThisAnimator.GetLayerIndex(layerName);
        }
    }
}