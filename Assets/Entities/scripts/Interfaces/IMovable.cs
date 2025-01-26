using UnityEngine;
using UnityEngine.Events;

public interface IMovable : IEntity
{
    MovementBehaviour MovementController { get; }

    UnityEvent<Vector3> OnTargetPositionChangedEvent { get; }

    public bool HasPath => MovementController.HasPath;

    void MoveTo(Vector3 newPosition)
    {
        MovementController.TargetPosition = newPosition;
        OnTargetPositionChangedEvent.Invoke(newPosition);
    }

    void Stop()
        => MovementController.ResetPath();
}