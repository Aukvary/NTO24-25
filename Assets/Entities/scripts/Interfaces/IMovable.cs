using UnityEngine;
using UnityEngine.Events;

public interface IMovable : IEntity
{
    MovementBehaviour MovementController { get; }

    UnityEvent<Vector3> OnTargetPositionChangedEvent { get; }

    new void Initialize()
    {
        if (EntityReference is IStatsable stats)
            MovementController.Speed = stats[EntityStatsType.Speed].StatValue;
        else
            throw new System.Exception("stats component was missed");
    }

    void MoveTo(Vector3 newPosition)
    {
        MovementController.TargetPosition = newPosition;
        OnTargetPositionChangedEvent.Invoke(newPosition);
    }
}