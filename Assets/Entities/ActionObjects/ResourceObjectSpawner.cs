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
    private uint _resourceCount;

    [SerializeField, Range(0, 100)]
    private float _dropChance;

    [SerializeField]
    private Resource _resorce;

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
            unit.Inventory.TryToAdd(_resorce);

        TimeToExtract -= unit.Strength;
        if (TimeToExtract < 0)
            ToBreak(unit);
    }

    public void ToBreak(Unit unit)
    {
        for (int i = 0; i < _resourceCount; i++)
            unit.Inventory.TryToAdd(_resorce);

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