using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class AnimationController : EntityComponent
    {
        public enum Animations
        {
            Idle,
            Move,
            Punch
        }

        public const string IDLE = "idle";
        public const string MOVE = "move";
        public const string PUNCH = "punch";

        [SerializeField]
        private UnityEvent<string> _onAnimationChangeEvent;

        [SerializeField]
        private UnityEvent _onAttackEvent;

        [SerializeField]
        private UnityEvent _onStepEvent;

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public void SetAnimation(Animations anim)
        {
            string name = anim switch
            {
                Animations.Idle => IDLE,
                Animations.Move => MOVE,
                Animations.Punch => PUNCH,

                _ => null
            };


            _animator.SetTrigger(name);
            _onAnimationChangeEvent.Invoke(name);
        }

        public void InvokeOnAttackEvent()
            => _onAttackEvent.Invoke();

        public void InvokeOnStepEvent()
            => _onStepEvent.Invoke();

        public void AddOnAnimationChangeAction(UnityAction<string> action)
            => _onAnimationChangeEvent.AddListener(action);

        public void RemoveAnimationChangeAction(UnityAction<string> action)
            => _onAnimationChangeEvent.RemoveListener(action);

        public void AddOnAttackAction(UnityAction action)
            => _onAttackEvent.AddListener(action);

        public void RemoveOnAttackAction(UnityAction action)
            => _onAttackEvent.RemoveListener(action);

        public void AddOnStepAction(UnityAction action)
            => _onStepEvent.AddListener(action);

        public void RemoveOnStepAction(UnityAction action)
            => _onStepEvent.RemoveListener(action);
    }
}