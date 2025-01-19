using UnityEngine;
using UnityEngine.Events;

public class Unit : Entity, IHealthable, IMovable
{
    [field: SerializeField]
    public UnityEvent<Vector3> OnTargetPositionChangedEvent { get; private set; }

    public EntityHealth HealthComponent { get; private set; }

    public MovementBehaviour MovementController { get; private set; }

    protected override void Awake()
    {
        base.Awake();

    }

    protected virtual void HealthInitialize()
    {
        HealthComponent = GetComponent<EntityHealth>();
    }

    protected virtual void MovementInitialize()
    {

    }
}