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

    private Transform _transform;

    public IEnumerable<ResourceCountPair> DropableItems => _dropableResources;

    public float Health
    {
        get => _heath;

        protected set
        {
            OnHealthChangeEvent?.Invoke();
            _heath = Mathf.Clamp(value, 0, MaxHealth);
        }
    }

    private float _maxHealth;

    public float MaxHealth => _maxHealth;

    public Transform Transform => _transform;

    public virtual bool CanInteract(Unit unit)
        => _heath > 0;

    public event Action OnHealthChangeEvent;

    public virtual void Interact(Unit unit)
    {
        Health -= GetDamage(unit);

        _onHitEvents.Invoke();
        if (_heath > 0)
            return;

        if (unit is Bear bear)
            Drop(bear);


        Break();
    }

    protected virtual void Awake()
    {
        _maxHealth = _heath;
        _transform = transform;
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

    private void OnDestroy()
    {
        _transform = null;
    }

    protected abstract float GetDamage(Unit Unit);

    protected abstract void Break();
}