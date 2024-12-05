using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Storage : ActionObject, ILoadable
{
    private Dictionary<Resource, int> _resources;

    [SerializeField]
    private string _name = "storage";

    private User _storageUser;

    private Dictionary<string, Resource> _resourcesNamePair;

    private string[] _names;

    public IEnumerable<KeyValuePair<Resource, int>> SrorageResources 
        => _resources;

    public bool Loaded { get; set; }

    public int this[Resource resource]
    {
        get => _resources[resource];

        set => Set(resource, value);
    }

    public event Action<Storage> OnLayOut;


    private void Awake()
    {
        var allResources = Resources.LoadAll<Resource>("Prefabs");
        _names = allResources.Select(r => r.ResourceName).ToArray();
        Loaded = false;

        Initialize();

        var ress = Resources.LoadAll<Resource>("Prefabs");
        _resourcesNamePair = ress.ToDictionary(r => r.name, r => r);

        _resources = ress.ToDictionary(r => r, r => 0);

        _resources = allResources.ToDictionary(r => r, r => 0);
    }

    public async void Initialize()
    {
        _storageUser = new(_name);
        await _storageUser.InitializeUser(_names);

        foreach (var res in _storageUser.Resources)
            _resources[_resourcesNamePair[res.Key]] = res.Value;
        OnLayOut?.Invoke(this);
        Loaded = true;
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