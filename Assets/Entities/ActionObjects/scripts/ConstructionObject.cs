using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionObject : Entity, IInteractable
{
    [SerializeField]
    private List<ResourceCountPair> _materials;

    [field: SerializeField]
    public UnityEvent<Entity> OnInteracEvent { get; private set; }

    public bool Interactable => throw new System.NotImplementedException();

    public IEnumerable<ResourceCountPair> Materials => _materials;
}