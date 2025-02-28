using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class Inventory : EntityComponent
    {
        [field: SerializeField, Min(1)]
        public int CellCapacity { get; private set; }

        [field: SerializeField, Min(1)]
        public int CellCount { get; private set; }

        [SerializeField]
        private UnityEvent _onItemsChangeEvent;

        private Pair<Resource, int>[] _items;

        public IEnumerable<Pair<Resource, int>> Items => _items;

        public int this[Resource res]
            => _items.Sum(p => p.Value1 == res ? p.Value2 : 0);

        protected override void Awake()
        {
            _items = new Pair<Resource, int>[CellCount];
        }

        public bool TryAddItems(Pair<Resource, int> items, out Pair<Resource, int> overflowItems)
        {
            int count = items.Value2;

            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Value2 == CellCapacity)
                    continue;

                else if (_items[i].Value1 == null)
                {
                    _items[i] = new(items.Value1, Mathf.Min(count, CellCapacity));
                    if (count > CellCapacity)
                    {
                        count -= CellCapacity;
                        continue;
                    }
                    else break;
                }

                else if (_items[i].Value1 == items.Value1)
                {
                    int cellCapacity = CellCapacity - _items[i].Value2;

                    _items[i] = new(items.Value1, Mathf.Min(count, cellCapacity) + _items[i].Value2);
                    if (count > cellCapacity)
                    {
                        count -= cellCapacity;
                        continue;
                    }
                    else break;
                }
            }


            overflowItems = new(items.Value1, count);
            _onItemsChangeEvent.Invoke();
            return overflowItems.Value2 == 0;
        }

        public void AddOnItemsChangeAction(UnityAction action)
            => _onItemsChangeEvent.AddListener(action);

        public void RemoveOnItemsChangeAction( UnityAction action)
            => _onItemsChangeEvent.RemoveListener(action);
    }
}