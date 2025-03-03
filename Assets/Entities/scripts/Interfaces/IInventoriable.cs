using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInventoriable : IEntity
    {
        Inventory Inventory { get; }

        IEnumerable<Pair<Resource, int>> Items => Inventory.Items;

        bool HasItems => Inventory.HasItems;

        int this[Resource resource] => Inventory[resource];

        void TryAddItems(Pair<Resource, int> resources)
        {
            if (Inventory.TryAddItems(resources, out var items))
                return;
        }

        void RemoveResources(Resource resource, int count)
            => Inventory.RemoveResources(resource, count);

        IEnumerable<Pair<Resource, int>> GetItems()
            => Inventory.GetItems();

        public void AddOnItemsChangeAction(UnityAction action)
            => Inventory.AddOnItemsChangeAction(action);

        public void RemoveOnItemsChangeAction(UnityAction action)
            => Inventory.RemoveOnItemsChangeAction(action);
    }
}