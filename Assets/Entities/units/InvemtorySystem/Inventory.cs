using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Cell[] _resources = new Cell[6];

    public event Action<Inventory> OnInventoryChanged;

    public Inventory()
    {
        for (int i = 0; i < _resources.Length; i++)
            _resources[i] = new Cell();
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
                OnInventoryChanged?.Invoke(this);
                return true;
            }
            else if (cell.Resource.ResourceName == resource.ResourceName)
            {
                cell.Add();
                OnInventoryChanged?.Invoke(this);
                return true;
            }
        }
        return false;
    }

    public IEnumerable<Resource> Remove()
    {
        var clone = _resources.Clone();
        _resources.SetValue(null, 0, 1, 2, 3, 4);
        return clone as IEnumerable<Resource>;
    }
}