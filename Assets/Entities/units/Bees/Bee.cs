using System.Collections.Generic;
using UnityEngine;

public class Bee : Unit, IDropable
{
    [SerializeField]
    private List<ResourceCountPair> _dropableItems;
    public IEnumerable<ResourceCountPair> DropableItems => _dropableItems;

    public void Drop(Bear bear)
    {
        foreach (var item in DropableItems)
            for (int i = 0; i < item.Count; i++)
                bear.Inventory.TryToAdd(item.Resource);

    }

    protected override void Die(Unit unit)
    {
        if (unit is Bear bear)
            Drop(bear);
        Destroy(gameObject);
    }
}