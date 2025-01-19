using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : Entity, IHealthable, IDropable
{
    [SerializeField]
    private List<ResourceCountPair> _dropableResources;

    [SerializeField]
    private UnityEvent<Entity, HealthChangeType> _onHealthChangeEvent;

    [SerializeField]
    UnityEvent<Entity> _onBrokeEvent;

    public EntityHealth HealthComponent { get; private set; }

    public IEnumerable<ResourceCountPair> DropableItems => _dropableResources;


    protected override void Awake()
    {
        base.Awake();
        InitializeHealth();
    }

    private void InitializeHealth()
    {
        HealthComponent = GetComponent<EntityHealth>();

        HealthComponent.AddOnHealthChangeAction(
            (entity, type) => 
                _onHealthChangeEvent.Invoke(entity, type)
            );

        HealthComponent.AddOnDeathAction(entity => _onBrokeEvent.Invoke(entity));
        HealthComponent.AddOnDeathAction(entity =>
        {
            if (entity is IInventoriable inventory)
                (this as IDropable).Drop(inventory);
        });
    }
}