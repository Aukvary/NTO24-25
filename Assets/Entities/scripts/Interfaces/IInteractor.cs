using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInteractor : IEntity
    {
        public InteractingController InteractingController { get; }

        public IInteractable Target
        {
            get => InteractingController.Target;
            set => InteractingController.Target = value;
        }

        public Vector3 TargetPosition => Target.EntityReference.transform.position;

        public bool CanInteract => InteractingController.CanInteract;

        public void AddOnChangeTargetAction(UnityAction<IInteractable> action)
            => InteractingController.AddOnChangeTargetAction(action);

        public void RemoveOnChangeTargetAction(UnityAction<IInteractable> action)
            => InteractingController.RemoveOnChangeTargetAction(action);

        public void AddOnInteractAction(UnityAction<IInteractable> action)
            => InteractingController.AddOnInteractAction(action);

        public void RemoveOnInteractAction(UnityAction<IInteractable> action)
            => InteractingController.RemoveOnInteractAction(action);
    }
}