using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Storage : MonoBehaviour, IInteractable, ILoadable
{
    private Dictionary<Resource, int> _resources;

    [SerializeField]
    public UnityEngine.Events.UnityEvent<Storage> OnLayOutItems;

    [SerializeField]
    private string _name = "storage";

    private User _storageUser;

    private Dictionary<string, Resource> _resourcesNamePair;

    private string[] _names;

    public IEnumerable<KeyValuePair<Resource, int>> SrorageResources 
        => _resources;

    public bool Loaded { get; set; }

    public Transform Transform => transform;

    public int this[Resource resource]
    {
        get => _resources[resource];

        #pragma warning disable
        set => Set(resource, value);
    }


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
        if (_resources.Any(p => p.Value > 0))
            OnLayOutItems?.Invoke(this);
        Loaded = true;
    }

    
    public async void Interact(Unit unit)
    {
        if (unit is Bee)
            return;
        foreach (var cell in (unit as Bear).Inventory.LayOutItems())
            await Set(cell.Key, cell.Value + _resources[cell.Key]);
    }

    private async Task Set(Resource resource, int count)
    {
        _resources[resource] = count;
        await _storageUser.UpdateUser(resource.ResourceName, _resources[resource]);
        OnLayOutItems?.Invoke(this);
    }

    public bool CanInteract(Unit unit)
        => (unit as Bear).Inventory.Resources.Any(c => c.Count > 0);
}