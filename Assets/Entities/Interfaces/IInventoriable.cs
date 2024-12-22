using System.Collections.Generic;
using UnityEngine.Events;

public interface IInventoriable : IEntity
{
    public Inventory Inventory { get; }

    public bool CanAppend { get; }

    public string Name { get; }

    public int this[Resource resource] { get; }

    public event UnityAction<IInventoriable, ResourceCountPair> OnFailedAddEvent;

    public void AddToInventory(ResourceCountPair resources);

    public void AddToInventory(IEnumerable<ResourceCountPair> resources);

    public void InitializeInventory()
        => Inventory.Initialize();
}