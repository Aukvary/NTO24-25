using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.Rendering;

public struct Inventory
{
    private InventoryCell[] _cells;

    private UnityEvent _onChangedEvent;

    public int this[Resource resource] => _cells.Sum(c => c.Resource == resource ? c.Count : 0);

    public Inventory(int cellCount, int cellCapacity = int.MaxValue)
    {
        _cells = new InventoryCell[cellCount];
        _onChangedEvent = new();

        for (int i = 0; i < cellCount; i++)
            _cells[i] = new(cellCapacity);
    }

    public Inventory(IEnumerable<InventoryCell> cells)
    {
        _cells = cells.ToArray();
        _onChangedEvent = new();
    }

    public bool TryAdd(Resource resource, int count, out ResourceCountPair overflowStuff)
    {
        int remainingCount = count;

        foreach (var cell in _cells)
        {
            if (cell.OverFlow || remainingCount == 0)
                continue;

            else if (cell.Resource == null)
            {
                cell.SetResource(resource);
                remainingCount = cell.Add(remainingCount);
            }
            else
            {
                remainingCount = cell.Add(remainingCount);
            }
        }

        overflowStuff = new(resource, remainingCount);
        
        _onChangedEvent.Invoke();
        return remainingCount == 0;
    }

    public IEnumerable<ResourceCountPair> GetResources(bool clear = true)
    {
        Dictionary<Resource, int> resources = new(_cells.Length);

        foreach (var cell in _cells)
        {
            if (resources.ContainsKey(cell.Resource))
                resources[cell.Resource] += cell.Count;
            else
                resources.Add(cell.Resource, cell.Count);

            if (clear) cell.Clear();
        }

        return resources.ToArray().Select(p => new ResourceCountPair(p.Key, p.Value));
    }

    public void AddOnChangeAction(UnityAction action)
        => _onChangedEvent.AddListener(action);

    public void RemoveOnChangeAction(UnityAction action)
        => _onChangedEvent.RemoveListener(action);
}