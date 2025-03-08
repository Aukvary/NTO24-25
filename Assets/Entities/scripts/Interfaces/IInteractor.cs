using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInteractor : IEntity
    {
        public InteractingController InteractingController { get; }

        public UnityEvent<IInteractable> OnChangeTargetEvent
            => InteractingController.OnChangeTargetEvent;

        public UnityEvent<IInteractable> OnInteractEvent
            => InteractingController.OnInteractEvent;

        public IInteractable Target
        {
            get => InteractingController.Target;
            set => InteractingController.Target = value;
        }

        public Vector3 TargetPosition => Target.EntityReference.transform.position;

        public bool CanInteract => InteractingController.CanInteract;
    }
}