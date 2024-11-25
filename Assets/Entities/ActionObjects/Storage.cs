using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : ActionObject
{
    private Dictionary<Resource, uint> _resources;

    public IEnumerable<KeyValuePair<Resource, uint>> SrorageResources 
        => _resources;

    public uint this[Resource resource]
    {
        get => _resources[resource];

        set
        {
            _resources[resource] = value;
            OnLayOut?.Invoke(this);
        }
    }

    public event Action<Storage> OnLayOut;


    private void Awake()
    {
        Resource[] resourceType = Resources.LoadAll<Resource>("Prefabs");

        _resources = resourceType.ToDictionary(r => r, r => 0u);

    }

    
    public override void Interact(Unit unit)
    {
        foreach (var cell in unit.Inventory.LayOutItems())
        {
            _resources[cell.Key] += cell.Value;
        }
        OnLayOut?.Invoke(this);
    }
}