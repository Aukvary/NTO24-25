using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakeableObject : ActionObject
{
    [SerializeField]
    private float _heath;

    [SerializeField]
    private float _regeneration;

    [SerializeField]
    private List<SelectingUpgradeButton.ResourseCountPair> _dropItems;

    [SerializeField]
    public UnityEvent _afterBreakEvents;

    [SerializeField]
    public UnityEvent _onHitEvent;

    private float _maxHealth;

    public float MaxHealth => _maxHealth;

    public float Regeneration => _regeneration;

    public float Health
    {
        get => _heath;

        set => Mathf.Clamp(value, 0, _maxHealth);
    }

    private void Awake()
    {
        _maxHealth = _heath;
    }

    private void Update()
    {
        Health += _regeneration * Time.deltaTime;
    }

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