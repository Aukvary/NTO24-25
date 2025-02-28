using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInteractor : IEntity
    {
        public InteractingBehaviour InteractingBehaviour { get; }

        public IInteractable Target => InteractingBehaviour.Target;

        public Vector3 TargetPosition => Target.EntityReference.transform.position;

        public bool CanInteract => InteractingBehaviour.CanInteract;

        public void TryInteract()
            => InteractingBehaviour.TryInteract();

        public void AddOnChangeTargetAction(UnityAction<IInteractable> action)
            => InteractingBehaviour.AddOnChangeTargetAction(action);

        public void RemoveOnChangeTargetAction(UnityAction<IInteractable> action)
            => InteractingBehaviour.RemoveOnChangeTargetAction(action);

        public void AddOnInteractAction(UnityAction<IInteractable> action)
            => InteractingBehaviour.AddOnInteractAction(action);

        public void RemoveOnInteractAction(UnityAction<IInteractable> action)
            => InteractingBehaviour.RemoveOnInteractAction(action);

        public void AddOnInteractFailedAction(UnityAction action)
            => InteractingBehaviour.AddOnInteractFailedAction(action);

        public void RemoveOnInteractFailedAction(UnityAction action)
            => InteractingBehaviour.RemoveOnInteractFailedAction(action);

        public void AddOnPossibilityChangeAction(UnityAction<bool> action)
            => InteractingBehaviour.AddOnPossibilityChangeAction(action);

        public void RemoveOnPossibilityChangeAction(UnityAction<bool> action)
            => InteractingBehaviour.AddOnPossibilityChangeAction(action);
    }
}