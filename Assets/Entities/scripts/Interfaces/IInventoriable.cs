using System.Collections.Generic;
using UnityEngine.Events;

public interface IInventoriable : IEntity
{
    public Inventory Inventory { get; }

    public int CellCount { get; }

    public int CellCapacity => int.MaxValue;

    public int this[Resource resource] => Inventory[resource];

    public UnityEvent<ResourceCountPair> OnFailedAddEvent { get; }

    public void AddToInventory(ResourceCountPair resources)
    {

    }

    public void AddToInventory(IEnumerable<ResourceCountPair> resources)
    {

    }
}