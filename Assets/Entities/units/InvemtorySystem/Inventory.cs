using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    private Cell[] _cells = new Cell[6];

    private Unit _unit;

    private User _inventoryUser;

    private Dictionary<string, Resource> _resourcesNamePair;

    private Dictionary<Resource, int> _resources;

    public IEnumerable<Cell> Resources => _cells;

    public event Action<Unit> OnInventoryChanged;

    public int this[Resource res]
    {
        get
        {
            int sum = 0;

            foreach (var cell in _cells)
                if (cell.Resource == res)
                    sum += cell.Count;

            return sum;
        }
    }

    public Inventory(Unit unit)
    {
        for (int i = 0; i < _cells.Length; i++)
            _cells[i] = new Cell();
        _unit = unit;

        var ress = UnityEngine.Resources.LoadAll<Resource>("Prefabs");
        _resourcesNamePair = ress.ToDictionary(r => r.name, r => r);

        _resources = ress.ToDictionary(r => r, r => 0);

        InitializeUser();
    }

    private async void InitializeUser()
    {
        if (_unit.IsBee)
            return;

        _inventoryUser = new(_unit.UnitName);
        await _inventoryUser.InitializeUser(_resourcesNamePair.Keys.ToArray());

        foreach (var res in _inventoryUser.Resources)
            for (int i = 0; i < res.Value; i++)
                TryToAdd(_resourcesNamePair[res.Key], false);
    }

    public void TryToAdd(Resource resource, bool update = true)
    {
        foreach (Cell cell in _cells)
        {
            if (cell.OverFlow)
                continue;

            else if (cell.Resource == null)
            {
                cell.Set(resource);
                if (update)
                    _inventoryUser.UpdateUser(resource.ResourceName, this[resource]);
                OnInventoryChanged?.Invoke(_unit);
                return;
            }
            else if (cell.Resource.ResourceName == resource.ResourceName)
            {
                cell.Add();
                if (update)
                    _inventoryUser.UpdateUser(resource.ResourceName, this[resource]);

                OnInventoryChanged?.Invoke(_unit);
                return;
            }
        }
    }

    public Dictionary<Resource, int> LayOutItems()
    {
        foreach (var name in _resourcesNamePair.Keys)
            _inventoryUser.UpdateUser(name, 0);
        Dictionary<Resource, int> resources = new();
        foreach (Cell cell in _cells)
        {
            if (cell.Resource == null)
                continue;
            var cond = resources.TryAdd(cell.Resource, cell.Count);
            if (!cond)
                resources[cell.Resource] += cell.Count;
        }
        for (int i = 0; i < _cells.Length; i++)
            _cells[i].Reset();
        OnInventoryChanged?.Invoke(_unit);
        return resources;
    }
}