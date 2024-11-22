using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float _strength;

    [SerializeField, Min(0f)]
    private float _speed;

    [SerializeField, Min(0f)]
    private float _damage;

    [SerializeField, Min(0f)]
    private float _attackDelay;

    [SerializeField]
    private bool _isBee;

    private UnitMovementController _moveController;
    private UnitExtractionController _extractionController;

    private Animator _animator;
    private BehaviourAnimation _behaviourAnimation;

    private UnitBehaviour _behavior;
    private Inventory _inventory = new();

    private Vector3 _spawnPosition;

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

    public float Strength => _strength;

    public float Speed => _speed;

    public float Damage => _damage;

    public float AttackDelay => _attackDelay;

    public Inventory Inventory => _inventory;

    public BehaviourAnimation BehaviourAnimation => _behaviourAnimation;

    public bool IsBee => _isBee;

    public Storage Storage { get; private set; }

    private Vector3 _position => transform.position;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _behaviourAnimation = GetComponentInChildren<BehaviourAnimation>();

        _behavior = _moveController;
        _moveController = new(this, _speed);
        _extractionController = new(this);
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

    public void PickItem(PickableItem item)
    {
        MoveTo(item.transform.position);
        StartCoroutine(AwaitOfMove(() =>
        {
            item?.Interact(this);
        }));

    }

    public void Follow(Unit unit)
    {
        _moveController.FollowUnit = unit;
    }

    public void Attack(Unit unit)
    {
        MoveTo(unit.transform.position);



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
        else if (Behaviour is UnitExtractionController)
            _animator.SetTrigger("punch");
        else
            _animator.SetTrigger("idle");
    }

}