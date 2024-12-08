using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    private User _levelUser;

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

        _levelUser = new(UnitName + "_level");

        _spawnPosition = transform.position;

        OnLevelUpDamageEvent += async u => await _levelUser.UpdateUser(nameof(AttackLevel), AttackLevel);

        OnLevelUpStrenghtEvent += async u => await _levelUser.UpdateUser(nameof(StrenghtLevel), StrenghtLevel);

        OnLevelUpHealthEvent += async u => await _levelUser.UpdateUser(nameof(HealthLevel), HealthLevel);

        Initialize();
    }

    public async void Initialize()
    {
        await _inventory.InitializeUser();
            await _levelUser.InitializeUser(nameof(AttackLevel), nameof(StrenghtLevel), nameof(HealthLevel));

        AttackLevel = _levelUser.Resources[nameof(AttackLevel)];
        StrenghtLevel = _levelUser.Resources[nameof(StrenghtLevel)];
        HealthLevel = _levelUser.Resources[nameof(HealthLevel)];

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