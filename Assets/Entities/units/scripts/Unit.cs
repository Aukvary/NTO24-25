using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : Entity, IHealthable, IMovable, IStatsable
{
    [SerializeField]
    private List<EntityStat> _stats;

    [field: SerializeField]
    public UnityEvent<Vector3> OnTargetPositionChangedEvent { get; private set; }

    public EntityHealth HealthComponent { get; private set; }

    public MovementBehaviour MovementController { get; private set; }

    public IEnumerable<EntityStat> Stats => _stats;

    protected override void Awake()
    {
        base.Awake();
        HealthInitialize();
        MovementInitialize();

    }

    protected virtual void HealthInitialize()
    {
        HealthComponent = GetComponent<EntityHealth>();
    }

    protected virtual void MovementInitialize()
    {
        MovementController = GetComponent<MovementBehaviour>();
    }
}