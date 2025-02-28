using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface IInventoriable : IEntity
    {
        Inventory Inventory { get; }

        int CellCount => Inventory.CellCount;

        int CellCapacity => Inventory.CellCapacity;

        IEnumerable<Pair<Resource, int>> Items => Inventory.Items;

        int this[Resource resource] => Inventory[resource];

        void TryAddItems(Pair<Resource, int> resources)
        {
            if (Inventory.TryAddItems(resources, out var items))
                return;
        }

        public void AddOnItemsChangeAction(UnityAction action)
            => Inventory.AddOnItemsChangeAction(action);

        public void RemoveOnItemsChangeAction(UnityAction action)
            => Inventory.RemoveOnItemsChangeAction(action);
    }
}