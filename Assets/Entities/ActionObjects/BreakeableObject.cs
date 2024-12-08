using UnityEngine;

public class BreakeableObject : InteractBuild
{
    [SerializeField]
    private float _regeneration;

    public float Regeneration => _regeneration;

    private void Update()
    {
        Health += _regeneration * Time.deltaTime;
    }

    protected override void Break()
    {
        Destroy(gameObject);
    }

    protected override float GetDamage(Unit unit)
        => (unit.Damage + unit.Strength) / 2;
}