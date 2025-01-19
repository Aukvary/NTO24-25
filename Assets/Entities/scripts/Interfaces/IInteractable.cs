using UnityEngine.Events;

public interface IInteractable
{
    UnityEvent<Entity> OnInteracEvent { get; }

    bool Interactable { get; }

    void InteractBy(Entity entity)
    {
        if (!Interactable)
            return;

        OnInteracEvent.Invoke(entity);
    }
}