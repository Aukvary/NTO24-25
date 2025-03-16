using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInventoriable : IEntity
    {
        Inventory Inventory { get; }

        UnityEvent OnItemsChangeEvent => Inventory.OnItemsChangeEvent;

        IEnumerable<Pair<Resource, int>> Items => Inventory.Items;

        bool HasItems => Inventory.HasItems;

        int this[Resource resource] => Inventory[resource];

        bool TryAddItems(Pair<Resource, int> resources, out int overflowItems)
            => Inventory.TryAddItems(resources, out overflowItems);

        void RemoveResources(Resource resource, int count)
            => Inventory.RemoveResources(resource, count);

        IEnumerable<Pair<Resource, int>> GetItems()
            => Inventory.GetItems();
    }
}