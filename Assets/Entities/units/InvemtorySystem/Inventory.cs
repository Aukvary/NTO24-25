using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

#pragma warning disable CS4014
public class Inventory
{
    private Cell[] _cells = new Cell[6];

    private Bear _bear;

    private User _inventoryUser;

    private Dictionary<string, Resource> _resourcesNamePair;

    private Dictionary<Resource, int> _resources;

    public IEnumerable<Cell> Resources => _cells;

    public event Action<Bear> OnInventoryChanged;

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

    public Inventory(Bear unit)
    {
        for (int i = 0; i < _cells.Length; i++)
            _cells[i] = new Cell();
        _bear = unit;

        var ress = UnityEngine.Resources.LoadAll<Resource>("Prefabs");
        _resourcesNamePair = ress.ToDictionary(r => r.name, r => r);

        _resources = ress.ToDictionary(r => r, r => 0);
    }

    public async Task InitializeUser()
    {

        _inventoryUser = new(_bear.UnitName);
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
                OnInventoryChanged?.Invoke(_bear);
                return;
            }
            else if (cell.Resource.ResourceName == resource.ResourceName)
            {
                cell.Add();
                if (update)
                    _inventoryUser.UpdateUser(resource.ResourceName, this[resource]);

                OnInventoryChanged?.Invoke(_bear);
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
        OnInventoryChanged?.Invoke(_bear);
        return resources;
    }
}