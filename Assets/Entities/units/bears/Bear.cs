using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Bear : Unit, ILoadable
{
    [SerializeField]
    private float _restoreTime;

    [SerializeField]
    private Storage _storage;

    [SerializeField]
    private BearActivityManager _bearActivityManager;

    [SerializeField]
    private Sprite _headSprite;

    [SerializeField]
    private string _unitName;

    private Collider _collider;
    private MeshRenderer[] _meshRenderers;

    private Inventory _inventory;

    private Vector3 _spawnPosition;

    public bool Alive
    {
        get => Health > 0;

        set
        {
            transform.position = _spawnPosition;
            if (value)
                Health = MaxHealth;

            Behaviour.Target = null;
            foreach (var item in _meshRenderers)
            {
                item.enabled = value;
            }
            NavMeshAgent.enabled = value;
            _collider.enabled = value;
        }
    }

    public Inventory Inventory => _inventory;

    public Sprite HeadSprite => _headSprite;

    public Storage Storage => _storage;

    public string UnitName => _unitName;

    public bool HasPath => NavMeshAgent.hasPath;

    public bool Loaded { get; set; }


    protected override void Awake()
    {
        base.Awake();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();

        _inventory = new(this);
        _bearActivityManager.AddUnit(this);

        _spawnPosition = transform.position;

        Initialize();

    }

    public async void Initialize()
    {
        await _inventory.InitializeUser();
        Loaded = true;
    }

    public override bool CanInteract(Unit unit)
        => base.CanInteract(unit) && Alive;

    protected override void Die(Unit by)
    {
        Alive = false;
        StartCoroutine(StartRestore());
    }

    private IEnumerator StartRestore()
    {
        Alive = false;
        yield return new WaitForSeconds(_restoreTime);
        Alive = true;
    }
}