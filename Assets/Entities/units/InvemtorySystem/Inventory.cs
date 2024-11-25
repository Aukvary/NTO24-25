using System;
using System.Collections.Generic;

public class Inventory
{
    private Cell[] _resources = new Cell[6];

    private Unit _unit;

    public event Action<Unit> OnInventoryChanged;

    public Inventory(Unit unit)
    {
        for (int i = 0; i < _resources.Length; i++)
            _resources[i] = new Cell();
        _unit = unit;
    }

    public IEnumerable<Cell> Resources => _resources;
    public bool TryToAdd(Resource resource)
    {
        foreach (Cell cell in _resources)
        {
            if (cell.OverFlow)
            {
                continue;
            }
            else if (cell.Resource == null)
            {
                cell.Set(resource);
                OnInventoryChanged?.Invoke(_unit);
                return true;
            }
            else if (cell.Resource.ResourceName == resource.ResourceName)
            {
                cell.Add();
                OnInventoryChanged?.Invoke(_unit);
                return true;
            }
        }
        return false;
    }

    public Dictionary<Resource, uint> LayOutItems()
    {
        Dictionary<Resource, uint> resources = new();
        foreach (Cell cell in _resources)
        {
            if (cell.Resource == null)
                continue;
            var cond = resources.TryAdd(cell.Resource, cell.Count);
            if (!cond)
                resources[cell.Resource] += cell.Count;
        }
        for (int i = 0; i < _resources.Length; i++)
            _resources[i].Reset();
        OnInventoryChanged?.Invoke(_unit);
        return resources;
    }
}