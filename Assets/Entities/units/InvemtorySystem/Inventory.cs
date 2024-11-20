using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

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
        OnInventoryChanged?.Invoke(this);
        return resources;
    }
}