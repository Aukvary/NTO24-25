using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Storage : ActionObject
{
    private Dictionary<Resource, int> _resources;

    private readonly string _name = "storage";

    private User _storageUser;

    private Dictionary<string, Resource> _resourcesNamePair;

    public IEnumerable<KeyValuePair<Resource, int>> SrorageResources 
        => _resources;

    public int this[Resource resource]
    {
        get => _resources[resource];

        set => Set(resource, value);
    }

    public event Action<Storage> OnLayOut;


    private void Awake()
    {
        var allResources = Resources.LoadAll<Resource>("Prefabs");
        var allNames = allResources.Select(r => r.ResourceName).ToArray();

        Initialize(allResources, allNames);

        var ress = UnityEngine.Resources.LoadAll<Resource>("Prefabs");
        _resourcesNamePair = ress.ToDictionary(r => r.name, r => r);

        _resources = ress.ToDictionary(r => r, r => 0);

        _resources = allResources.ToDictionary(r => r, r => 0);
    }

    private async void Initialize(Resource[] resources, string[] names)
    {
        _storageUser = new(_name);
        await _storageUser.InitializeUser(names);

        foreach (var res in _storageUser.Resources)
            _resources[_resourcesNamePair[res.Key]] = res.Value;
        OnLayOut?.Invoke(this);
    }

    
    public async override void Interact(Unit unit)
    {
        foreach (var cell in unit.Inventory.LayOutItems())
            await Set(cell.Key, cell.Value + _resources[cell.Key]);
    }

    private async Task Set(Resource resource, int count)
    {
        _resources[resource] = count;
        await _storageUser.UpdateUser(resource.ResourceName, _resources[resource]);
        OnLayOut?.Invoke(this);
    }
}