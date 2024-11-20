using UnityEngine;

public class ResourceObjectSpawner : ActionObject, BreakableObject
{
    [SerializeField, Min(0f)]
    private float _timeToExtract;

    [SerializeField, Min(0f)]
    private float _timeToRestore;

    [SerializeField]
    private PickableItem _pickableItem;

    private bool _isRestored = true;

    private Collider _collider;
    private MeshRenderer _renderer;

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
            _isRestored = value;
            _collider.enabled = value;
            _renderer.enabled = value;
            if (value)
                _timeToExtract = _baseTimeToExtract;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _baseTimeToExtract = _timeToExtract;
    }

    public override void Interact(Unit unit)
    {
        if (!IsRestored)
            return;
        TimeToExtract -= unit.Strength;
    }

    public void ToBreak()
    {
        StartCoroutine(StartRestoring());
        _pickableItem.Spawn(transform.position);
    }

    private System.Collections.IEnumerator StartRestoring()
    {
        IsRestored = false;
        yield return new WaitForSeconds(_timeToRestore);
        IsRestored = true;
    }
}