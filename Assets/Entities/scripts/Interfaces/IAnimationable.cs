using UnityEngine.Events;

namespace NTO24
{
    public interface IAnimationable : IEntity
    {
        AnimationController AnimationController { get; }

        UnityEvent<string> OnAnimationChangeEvent 
            => AnimationController.OnAnimationChangeEvent;

        UnityEvent OnAttackEvent
            => AnimationController.OnAttackEvent;

        UnityEvent OnStepEvent
            => AnimationController.OnStepEvent;

        void SetAnimation(AnimationController.Animations name)
            => AnimationController.SetAnimation(name);
    }
}