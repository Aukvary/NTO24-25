using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    #region SerializeStats
    [Header("Stats")]

    [SerializeField, Min(0f)]
    private float _speed;

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
    #endregion

    [SerializeField]
    private Sprite _headSprite;

    private bool _alive = true;
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
    private UnitBehaviour _behavior;
    private Inventory _inventory;
    #endregion

    private Vector3 _spawnPosition;


    #region Levels
    private int _attackLevel;
    private int _strenghtLevel;
    private int _healthLevel;
    #endregion

    public UnitStates UnitState { get; private set; }
    public UnitBehaviour Behaviour
    {
        get => _behavior;
        set
        {
            if (Behaviour == value)
                return;
            StopAllCoroutines();
            Behaviour?.BehaviourExit();
            value?.BehaviourEnter();
            _behavior = value;
        }
    }

    #region StatsProperty
    public float Strength => _strength[_strenghtLevel];

    public float Speed => _speed;

    public float Damage => _damage[_attackLevel];

    public float MaxHealth => _maxHealth[_healthLevel];

    public float Regeneration => _regeneration[_healthLevel];
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

    private bool Alive
    {
        get => _alive;

        set
        {
            _alive = value;
            transform.position = _spawnPosition;
            if (value)
                Health = MaxHealth;

            foreach (var item in _meshRenderers)
            {
                item.enabled = value;
            }
            _navMeshAgent.enabled = value;
            _collider.enabled = value;
        }
    }

    public Inventory Inventory => _inventory;
    public Sprite HeadSprite => _headSprite;

    public BehaviourAnimation BehaviourAnimation => _behaviourAnimation;

    public bool IsBee => _isBee;

    public Storage Storage { get; private set; }

    private Vector3 _position => transform.position;

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
        _moveController = new(this, _speed);
        _extractionController = new(this);
        _buildBehaviour = new(this);
        _inventory = new(this);
    }

    public Unit Spawn(Vector3 spawnPostion, Storage storage)
    {
        var newUnit = Instantiate(this, spawnPostion, Quaternion.identity);
        newUnit._spawnPosition = spawnPostion;
        newUnit.Storage = storage;

        return newUnit;
    }

    private void Update()
    {
        Behaviour?.BehaviourUpdate();
        SetAnimation();
        if (Alive)
            Health += Regeneration * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G))
            Health -= 10;
        if (Input.GetKeyDown(KeyCode.H))
            Health += 10;
    }

    public void MoveTo(Vector3 newPostion)
    {
        _moveController.TargetPosition = newPostion;
    }

    public void Extract(ResourceObjectSpawner spawner)
    {
        MoveTo(spawner.transform.position);
        StartCoroutine(AwaitOfMove(() =>
        {
            _extractionController.Resource = spawner;
        }));

    }

    public void Attack(Unit unit)
    {
        MoveTo(unit.transform.position);



    }

    public void Build(ConstructionObject obj)
    {
        MoveTo(obj.transform.position);
        StartCoroutine(AwaitOfMove(() =>
        {
            _buildBehaviour.Build = obj;
        }));
    }

    public void LayOutItems(Storage storage)
    {
        MoveTo(storage.transform.position);
        StartCoroutine(AwaitOfMove(() =>
        {
            storage.Interact(this);
        }));
    }

    private IEnumerator AwaitOfMove(Action afterAction)
    {
        while (!_moveController.HasPath)
            yield return null;
        while (_moveController.HasPath)
            yield return null;
        afterAction?.Invoke();
    }

    private void SetAnimation()
    {
        if (_moveController.HasPath)
            _animator.SetTrigger("move");
        else if (Behaviour is UnitExtractionController || Behaviour is BuildBehaviour)
            _animator.SetTrigger("punch");
        else
            _animator.SetTrigger("idle");
    }

    private IEnumerator StartRestore()
    {
        Alive = false;
        yield return new WaitForSeconds(_restoreTime);
        Alive = true;
    }
}