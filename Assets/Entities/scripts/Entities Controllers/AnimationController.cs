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

        [field: SerializeField]
        public UnityEvent<string> OnAnimationChangeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent OnAttackEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent OnStepEvent { get; private set; }

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
            OnAnimationChangeEvent.Invoke(name);
        }

        public void InvokeOnAttackEvent()
            => OnAttackEvent.Invoke();

        public void InvokeOnStepEvent()
            => OnStepEvent.Invoke();
    }
}