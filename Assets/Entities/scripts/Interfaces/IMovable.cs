using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IMovable : IEntity
    {
        MovementController MovementController { get; }

        UnityEvent<Vector3> OnDestinationChangedEvent 
            => MovementController.OnDestinationChangedEvent;

        float StopDistance { get; }

        bool HasPath => MovementController.HasPath;

        float AngularSpeed => MovementController.AngularSpeed;

        Vector3 Destination => MovementController.Destination;

        void MoveTo(Vector3 newPosition)
            => MovementController.Destination = newPosition;

        void Stop()
            => MovementController.ResetPath();
    }
}