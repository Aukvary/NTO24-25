using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public interface IDropable : IEntity
    {
        IEnumerable<Pair<Resource, int>> DropableItems { get; }

        public void Drop(IInventoriable inventory)
        {
            foreach (var item in DropableItems) 
                inventory.TryAddItems(item);
        }
    }
}