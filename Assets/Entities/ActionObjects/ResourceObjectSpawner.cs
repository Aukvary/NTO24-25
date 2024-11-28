using System.Collections.Generic;
using UnityEngine;

public class ResourceObjectSpawner : ActionObject
{
    [Header("Restoring")]
    [SerializeField]
    private Mesh _restoredMesh;
    [SerializeField]
    private Mesh _restoringMesh;

    [SerializeField, Min(0f)]
    private float _timeToExtract;

    [SerializeField, Min(0f)]
    private float _timeToRestore;

    [Header("Resorse")]
    [SerializeField]
    private List<SelectingUpgradeButton.ResourseCountPair> _dropResourses;

    [SerializeField, Range(0, 100)]
    private float _dropChance;

    private bool _isRestored = true;

    private Collider _collider;
    private MeshFilter _renderer;

    private float _baseTimeToExtract;

    public float TimeToExtract
    {
        get => _timeToExtract;

        set => _timeToExtract = value;
    }

    public bool IsRestored
    {
        get => _isRestored;

        private set
        {
            _renderer.mesh = value ? _restoredMesh : _restoringMesh;

            _isRestored = value;
            _collider.enabled = value;
            if (value)
                _timeToExtract = _baseTimeToExtract;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshFilter>();
        _baseTimeToExtract = _timeToExtract;
    }

    public override void Interact(Unit unit)
    {
        if (!IsRestored)
            return;



        if (Random.Range(0f, 1f) <= (_dropChance / 100))
            unit.Inventory.TryToAdd(_dropResourses[Random.Range(0, _dropResourses.Count - 1)].Resource);

        TimeToExtract -= unit.Strength;
        if (TimeToExtract < 0)
            ToBreak(unit);
    }

    public void ToBreak(Unit unit)
    {
        foreach (var item in _dropResourses)
            for (int i = 0; i < item.Count; i++)
                unit.Inventory.TryToAdd(item.Resource);

        unit.Behaviour = null;

        StartCoroutine(StartRestoring());
    }

    private System.Collections.IEnumerator StartRestoring()
    {
        IsRestored = false;
        yield return new WaitForSeconds(_timeToRestore);
        IsRestored = true;
    }
}