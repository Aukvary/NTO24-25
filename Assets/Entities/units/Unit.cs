using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Unit : MonoBehaviour, ILoadable
{
    #region SerializeStats
    [Header("Stats")]

    [SerializeField, Min(0f)]
    private float _speed;

    [SerializeField, Min(0f)]
    private float _attackRange;

    [SerializeField, Min(0f)]
    private float _attackAngle;

    [SerializeField]
    private List<float> _strength;

    [SerializeField, Min(0f)]
    private List<float> _damage;

    [SerializeField, Min(0f)]
    private List<float> _maxHealth;

    [SerializeField, Min(0f)]
    private List<float> _regeneration;

    [SerializeField]
    private float _restoreTime;

    [SerializeField]
    private bool _isBee;

    [SerializeField]
    private List<SelectingUpgradeButton.ResourseCountPair> _dropResources;
    #endregion

    [SerializeField]
    private Storage _storage;

    [SerializeField]
    private BearActivityManager _bearActivityManager;

    [SerializeField]
    private Sprite _headSprite;

    [SerializeField]
    private string _unitName;

    private float _health;

    #region UnitComponentsAlive
    private Collider _collider;
    private NavMeshAgent _navMeshAgent;
    private MeshRenderer[] _meshRenderers;
    #endregion

    #region AnyComponents
    private Animator _animator;
    private BehaviourAnimation _behaviourAnimation;
    #endregion

    #region UnitBehaviour
    private BuildBehaviour _buildBehaviour;
    private UnitExtractionController _extractionController;
    private UnitMovementController _moveController;
    private AttackBehaviour _attackBehaviour;
    private UnitBehaviour _behavior;
    private Inventory _inventory;
    #endregion

    private Vector3 _spawnPosition;


    #region Levels
    private int _attackLevel;
    private int _strenghtLevel;
    private int _healthLevel;
    #endregion

    public UnitBehaviour Behaviour
    {
        get => _behavior;
        set
        {
            if (Behaviour == value)
                return;

            Behaviour?.BehaviourExit();
            if (value == null)
                _moveController.BehaviourEnter();
            else
                value.BehaviourEnter();

            _behavior = value == null ? _moveController : value;
        }
    }

    #region StatsProperty
    public float Damage => _damage[AttackLevel];
    public float Strength => _strength[StrenghtLevel];
    public float MaxHealth => _maxHealth[HealthLevel];
    public float Regeneration => _regeneration[_healthLevel];

    public float Speed => _speed;
    #endregion 

    public float Health
    {
        get => _health;

        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChangeEvent?.Invoke(this);
            if (_health == 0f)
            {
                if (IsBee)
                    Destroy(gameObject);
                else
                    StartCoroutine(StartRestore());
            }
        }
    }

    public bool Alive
    {
        get => Health > 0;

        set
        {
            transform.position = _spawnPosition;
            if (value)
                Health = MaxHealth;

            foreach (var item in _meshRenderers)
            {
                item.enabled = value;
            }
            _behavior = null;
            _navMeshAgent.enabled = value;
            _collider.enabled = value;
        }
    }

    public Inventory Inventory => _inventory;
    public Sprite HeadSprite => _headSprite;

    public BehaviourAnimation BehaviourAnimation => _behaviourAnimation;

    public bool IsBee => _isBee;

    public Storage Storage => _storage;

    private Vector3 _position => transform.position;

    public int AttackLevel
    {
        get => _attackLevel;

        set => _attackLevel = Math.Clamp(value, 0, 4);
    }
    public int StrenghtLevel
    {
        get => _strenghtLevel;
        set => _strenghtLevel = Math.Clamp(value, 0, 4);
    }
    public int HealthLevel
    {
        get => _healthLevel;

        set => _healthLevel = Math.Clamp(value, 0, 4);
    }

    public Animator Animator => _animator;

    public UnitMovementController MovementController => _moveController;
    public AttackBehaviour AttackBehaviour => _attackBehaviour;

    public string UnitName => _unitName;

    public bool Loaded { get; set; }


    public event Action<Unit> OnHealthChangeEvent;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _behaviourAnimation = GetComponentInChildren<BehaviourAnimation>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();


        _health = MaxHealth;
        _behavior = _moveController;
        _moveController = new(this, _speed, _attackRange);
        _extractionController = new(this, _attackRange);
        _buildBehaviour = new(this, _attackRange);
        _attackBehaviour = new(this, _attackRange, _attackAngle);

        _inventory = new(this);

        _spawnPosition = transform.position;

        _bearActivityManager.AddUnit(this);

        Initialize();
    }

    public async void Initialize()
    {
        await _inventory.InitializeUser();
        Loaded = true;
    }

    private void Update()
    {
        Behaviour?.BehaviourUpdate();
        if (Alive)
            Health += Regeneration * Time.deltaTime;
    }

    public void MoveTo(Vector3 newPostion)
    {
        if (!Alive)
            return;
        _moveController.TargetPosition = newPostion;
    }

    public void Extract(ResourceObjectSpawner spawner)
    {
        if (!Alive)
            return;
        _extractionController.Resource = spawner;
    }

    public void Attack(Unit unit)
    {
        if (!Alive)
            return;
        _attackBehaviour.AttackedUnit = unit;
    }

    public void Attack(BreakeableObject obj)
    {
        if (!Alive)
            return;
        _attackBehaviour.BreakeableObject = obj;
    }

    public void Build(ConstructionObject obj)
    {
        if (!Alive)
            return;
        _buildBehaviour.Build = obj;
    }

    public void LayOutItems(Storage storage)
    {
        if (!Alive)
            return;
        _moveController.TargetPosition = storage.transform.position;
        _moveController.OnMoveEndEvent += () => storage.Interact(this);
    }

    public bool DamageUnit(Unit from, out List<SelectingUpgradeButton.ResourseCountPair> res)
    {
        res = null;

        Health -= from.Damage;
        if (Health <= 0)
            res = _dropResources;
        return Health <= 0;
    }

    private IEnumerator StartRestore()
    {
        Alive = false;
        yield return new WaitForSeconds(_restoreTime);
        Alive = true;
    }

    public bool CanUpgrade(UpgradeType type) => type switch
    {
        UpgradeType.Damage => AttackLevel != 4,
        UpgradeType.Strenght => StrenghtLevel != 4,
        UpgradeType.Health => HealthLevel != 4,
        _ => false,
    };

    public void Upgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                AttackLevel += 1;
                break;

            case UpgradeType.Strenght:
                StrenghtLevel += 1;
                break;

            case UpgradeType.Health:
                HealthLevel += 1;
                break;
        }
    }

    public void Upgrade(UpgradeType type, int level)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                AttackLevel = level;
                break;

            case UpgradeType.Strenght:
                StrenghtLevel = level;
                break;

            case UpgradeType.Health:
                HealthLevel = level;
                break;
        }
    }
}