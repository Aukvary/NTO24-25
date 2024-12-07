using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceObjectSpawner : InteractBuild
{
    [SerializeField, Range(0, 100)]
    private float _dropChance;

    private bool _isRestored = true;

    private Collider _collider;
    private MeshFilter _renderer;

    [Header("Restoring")]
    [SerializeField]
    private Mesh _restoredMesh;
    [SerializeField]
    private Mesh _restoringMesh;

    [SerializeField, Min(0f)]
    private float _timeToRestore;

    public bool IsRestored
    {
        get => _isRestored;

        private set
        {
            _renderer.mesh = value ? _restoredMesh : _restoringMesh;

            _isRestored = value;
            _collider.enabled = value;
            if (value)
                Health = MaxHealth;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshFilter>();
    }

    protected override float GetDamage(Unit Unit)
        => Unit.Strength;

    public override void Interact(Unit unit)
    {
        if (unit is Bee)
            return;
        var bear = unit as Bear;

        if (Random.Range(0f, 1f) <= (_dropChance / 100))
            bear.Inventory
                .TryToAdd(DropableItems
                .ElementAt(Random.Range(0, DropableItems.Count() - 1)).Resource);

        base.Interact(unit);
    }

    protected override void Break()
    {
        StartCoroutine(StartRestoring());
    }

    private System.Collections.IEnumerator StartRestoring()
    {
        IsRestored = false;
        yield return new WaitForSeconds(_timeToRestore);
        IsRestored = true;
    }
}