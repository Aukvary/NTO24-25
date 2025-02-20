using UnityEngine.Events;

public interface IInteractable
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