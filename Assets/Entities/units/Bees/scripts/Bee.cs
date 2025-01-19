using System.Collections.Generic;
using UnityEngine;

public class Bee : Unit, IDropable
{
    [SerializeField]
    private List<ResourceCountPair> _dropableItems;
    public IEnumerable<ResourceCountPair> DropableItems => _dropableItems;

    protected override void HealthInitialize()
    {
        base.HealthInitialize();
        HealthComponent.AddOnDeathAction(entity =>
        {
            if (entity is IInventoriable inventory)
                (this as IDropable).Drop(inventory);
        });
    }
}