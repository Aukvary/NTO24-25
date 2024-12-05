using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakeableObject : ActionObject
{
    [SerializeField]
    private float _heath;

    [SerializeField]
    private List<SelectingUpgradeButton.ResourseCountPair> _dropItems;

    [SerializeField]
    public UnityEvent _afterBreakEvents;

    [SerializeField]
    public UnityEvent _onHitEvent;

    public float Health => _heath;

    public override void Interact(Unit unit)
    {
        _heath -= unit.Damage;

        _onHitEvent.Invoke();
        if (_heath > 0)
            return;

        _afterBreakEvents.Invoke();

        foreach (var item in _dropItems)
            for (int i = 0; i < item.Count; i++)
                unit.Inventory.TryToAdd(item.Resource);
        Destroy(gameObject);
    }
}