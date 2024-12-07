using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBuild : MonoBehaviour, IInteractable, IDropableEntity
{
    [SerializeField]
    private float _heath;

    [SerializeField]
    private List<ResourceCountPair> _dropableResources;

    [SerializeField]
    private UnityEvent _onBreakEvents;

    [SerializeField]
    private UnityEvent _onHitEvents;
    public IEnumerable<ResourceCountPair> DropableItems => _dropableResources;

    public float Health
    {
        get => _heath;

        protected set
        {
            _heath = value;
            _onHitEvents.Invoke();
        }
    }

    private float _maxHealth;

    public float MaxHealth => _maxHealth;

    public Transform Transform => transform;

    public bool CanInteract(Unit unit)
        => _heath > 0;

    public virtual void Interact(Unit unit)
    {
        Health -= GetDamage(unit);

        if (_heath > 0)
            return;

        if (unit is Bear bear)
            Drop(bear);

        Break();
    }

    protected virtual void Awake()
    {
        _maxHealth = _heath;
    }

    public void AddListerForHit(UnityAction action)
        => _onHitEvents.AddListener(action);

    public void AddListnerForDeath(UnityAction action)
        => _onBreakEvents.AddListener(action);

    public void Drop(Bear unit)
    {
        foreach (var item in DropableItems)
            for (int i = 0; i < item.Count; i++)
                unit.Inventory.TryToAdd(item.Resource);

    }

    protected abstract float GetDamage(Unit Unit);

    protected abstract void Break();
}