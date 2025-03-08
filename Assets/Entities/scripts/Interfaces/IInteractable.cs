using UnityEngine.Events;

namespace NTO24
{
    public interface IInteractable : IEntity
    {
        Interactable Interactable { get; }

        UnityEvent<IInteractor> OnInteractEvent => Interactable.OnInteractEvent;

        bool IsInteractable(IInteractor interactor);

        void Interact(IInteractor interactor)
            => Interactable.Interact(interactor);
    }
}