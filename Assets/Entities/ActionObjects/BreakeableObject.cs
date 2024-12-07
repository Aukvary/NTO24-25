using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakeableObject : InteractBuild
{
    [SerializeField]
    private float _regeneration;

    public float Regeneration => _regeneration;

    protected override void Break()
    {
        Destroy(gameObject);
    }

    protected override float GetDamage(Unit unit)
        => (unit.Damage + unit.Strength) / 2;
}