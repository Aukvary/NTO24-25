using UnityEngine;

public interface IInteractable
{
    Transform Transform { get; }
    bool CanInteract(Unit unit);
    void Interact(Unit unit);
}