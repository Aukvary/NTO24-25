using UnityEngine;

public class BreakeableObject : ActionObject
{
    [SerializeField]
    private float _heath;

    public override void Interact(Unit unit)
    {
        _heath -= (unit.Strength + unit.Damage) / 2;
    }
}