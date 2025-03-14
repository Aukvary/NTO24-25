using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class Inventory : EntityComponent, ISavableComponent
    {
        private int _cellCapacity;

        [field: SerializeField]
        public UnityEvent OnItemsChangeEvent { get; private set; }

        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        private Pair<Resource, int>[] _items;

        public IEnumerable<Pair<Resource, int>> Items => _items;

        public bool HasItems => _items.Any(p => p.Value1 != null);

        public string Name => "Inventory";

        public string[] Data
        {
            get
            {
                string[] data = new string[_items.Count(p => p.Value1 != null)];

                for (int i = 0; i < data.Length; i++)
                    data[i] = _items[i].ToString();

                return data;
            }
        }

        public int this[Resource res]
            => _items.Sum(p => p.Value1 == res ? p.Value2 : 0);


        public void Initialize(int cellCount, int cellCapacity = int.MaxValue)
        {
            _items = new Pair<Resource, int>[cellCount];
            _cellCapacity = cellCapacity;
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (i < data.Count())
                    _items[i] = data.ElementAt(i).ToResources();
                else
                    _items[i] = new(null, 0);
            }
            OnItemsChangeEvent.Invoke();
        }

        public bool TryAddItems(Pair<Resource, int> items, out Pair<Resource, int> overflowItems)
        {
            int count = items.Value2;
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Value2 == _cellCapacity)
                    continue;

                else if (_items[i].Value1 == null)
                {
                    _items[i] = new(items.Value1, Mathf.Min(count, _cellCapacity));
                    if (count > _cellCapacity)
                    {
                        count -= _cellCapacity;
                        continue;
                    }
                    else break;
                }

                else if (_items[i].Value1 == items.Value1)
                {
                    int cellCapacity = _cellCapacity - _items[i].Value2;

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
            OnItemsChangeEvent.Invoke();
            OnDataChangeEvent.Invoke();
            return overflowItems.Value2 == 0;
        }

        public IEnumerable<Pair<Resource, int>> GetItems()
        {
            IEnumerable<Pair<Resource, int>> items = _items.ToArray();
            for (int i = 0; i < _items.Length; i++)
                _items[i] = new(null, 0);
            OnItemsChangeEvent.Invoke();
            return items;
        }

        public void RemoveResources(Resource resource, int count)
        {
            int currentCount = count;

            for (int i = _items.Length - 1; i >= 0; i--)
            {
                if (_items[i].Value1 != resource)
                    continue;

                if (currentCount < _items[i].Value2)
                {
                    _items[i] = new(_items[i].Value1, _items[i].Value2 - currentCount);
                    currentCount = 0;
                }
                else
                {
                    currentCount -= _items[i].Value2;
                    _items[i] = new(null, 0);
                }

                if (currentCount <= 0)
                    break;
            }


            OnItemsChangeEvent.Invoke();
        }
    }
}