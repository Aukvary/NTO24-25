using UnityEngine.Events;

namespace NTO24
{
    public interface IAnimationable : IEntity
    {
        AnimationController AnimationController { get; }

        public void SetAnimation(AnimationController.Animations name)
            => AnimationController.SetAnimation(name);

        public void AddOnAnimationChangeAction(UnityAction<string> action)
            => AnimationController.AddOnAnimationChangeAction(action);

        public void RemoveAnimationChangeAction(UnityAction<string> action)
            => AnimationController.RemoveAnimationChangeAction(action);

        public void AddOnAttackAction(UnityAction action)
            => AnimationController.AddOnAttackAction(action);

        public void RemoveOnAttackAction(UnityAction action)
            => AnimationController.RemoveOnAttackAction(action);

        public void AddOnStepAction(UnityAction action)
            => AnimationController.AddOnStepAction(action);

        public void RemoveOnStepAction(UnityAction action)
            => AnimationController.RemoveOnStepAction(action);
    }
}