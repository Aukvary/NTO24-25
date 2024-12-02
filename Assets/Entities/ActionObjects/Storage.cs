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

    private Resource[] _allResources;
    private string[] _allNames;

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

        _resources = allResources.ToDictionary(r => r, r => 0);
    }

    private async void Initialize(Resource[] resources, string[] names)
    {
        _allResources = resources;
        _allNames = names;
        _storageUser = new(_name);
        await _storageUser.InitializeUser(names);
        for (int i = 0; i < resources.Length; i++) 
        {
            _resources[resources[i]] = _storageUser.Resources[names[i]];
        }
        OnLayOut?.Invoke(this);
    }

    
    public async override void Interact(Unit unit)
    {
        await Refresh();
        foreach (var cell in unit.Inventory.LayOutItems())
            await Set(cell.Key, cell.Value + _resources[cell.Key]);
    }

    private async Task Set(Resource resource, int count, bool refresh = false)
    {
        if (refresh)
            await Refresh();
        _resources[resource] = count;
        await _storageUser.UpdateUser(resource.ResourceName, _resources[resource]);
        OnLayOut?.Invoke(this);
    }

    private async Task Refresh()
    {
        await _storageUser.UpdateUser();
        for (int i = 0; i < _allResources.Length; i++)
            _resources[_allResources[i]] = _storageUser.Resources[_allNames[i]];
    }
}