using UnityEngine;

public class ResourceObjectSpawner : ActionObject, BreakableObject
{
    [SerializeField]
    private Mesh _restoredMesh;
    [SerializeField]
    private Mesh _restoringMesh;

    [SerializeField, Min(0f)]
    private float _timeToExtract;

    [SerializeField, Min(0f)]
    private float _timeToRestore;

    [SerializeField]
    private Resource _resorce;

    private bool _isRestored = true;

    private Collider _collider;
    private MeshFilter _renderer;

    private float _baseTimeToExtract;

    public float TimeToExtract
    {
        get => _timeToExtract;

        set
        {
            _timeToExtract = value;
            if (value <= 0)
                ToBreak();
        }
    }

    public bool IsRestored
    {
        get => _isRestored;

        private set
        {
            if (value)
                _renderer.mesh = _restoredMesh;
            else
                _renderer.mesh = _restoringMesh;

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
        unit.Inventory.TryToAdd(_resorce);
        TimeToExtract -= unit.Strength;
    }

    public void ToBreak()
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