using UnityEngine.Events;

namespace NTO24
{
    public interface IInteractable : IEntity
    {
        bool Interactable { get; }

        UnityEvent<IInteractor> OnInteracEvent { get; }

        void InteractBy(IInteractor entity)
        {
            if (!Interactable)
                return;

            OnInteracEvent.Invoke(entity);
        }
    }
}