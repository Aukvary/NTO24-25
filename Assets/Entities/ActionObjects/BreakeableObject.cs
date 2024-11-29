using UnityEngine;
using System.Collections.Generic;
using System;

public class BreakeableObject : ActionObject
{
    [SerializeField]
    private float _heath;

    [SerializeField]
    private List<SelectingUpgradeButton.ResourseCountPair> _dropItems;

    [SerializeField]
    private UnityEngine.Events.UnityEvent _afterBreakEvents;

    public float Health => _heath;

    public event Action OnHitEvent;

    public override void Interact(Unit unit)
    {
        _heath -= unit.Damage;
        OnHitEvent?.Invoke();

        if (_heath > 0)
            return;
        _afterBreakEvents.Invoke();

        foreach (var item in _dropItems)
            for (int i = 0; i < item.Count; i++)
                unit.Inventory.TryToAdd(item.Resource);
    }
}