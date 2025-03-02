using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IMovable : IEntity
    {
        float StopDistance { get; }

        MovementController MovementController { get; }

        bool HasPath => MovementController.HasPath;

        float AngularSpeed => MovementController.AngularSpeed;

        Vector3 Destination => MovementController.Destination;

        void MoveTo(Vector3 newPosition)
            => MovementController.Destination = newPosition;

        void Stop()
            => MovementController.ResetPath();

        void AddOnDestinationChangeAction(UnityAction<Vector3> action)
            => MovementController.AddOnDestinationChangeAction(action);

        void RemoveOnDestinationChangeAction(UnityAction<Vector3> action)
            => MovementController.RemoveOnDestinationChangeAction(action);
    }
}