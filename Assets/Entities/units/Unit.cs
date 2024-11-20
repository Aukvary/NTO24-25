using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private UnitBehaviour _behavior;
    private Inventory _inventory = new();

    public UnitList _unitList;

    public UnitStates UnitState { get; private set; }
    public UnitBehaviour Behavior 
    {
        get => _behavior;
        set
        {
            StopAllCoroutines();
            if (Behavior == value)
                return;
            Behavior?.BehaviourExit();
            value?.BehaviourEnter();
            _behavior = value;
        }
    }

    public float Strength => _strength;

    public float Speed => _speed;

    public float Damage => _damage;

    public float AttackDelay => _attackDelay;

    public Inventory Inventory => _inventory;

    public bool IsBee => _isBee;

    private Vector3 _position => transform.position;


    public event Action<Unit> OnPickItem;


    private void Awake()
    {
        Spawn(_unitList);
    }

    private void Update()
    {
        Behavior?.BehaviourUpdate();
    }

    public void Spawn(UnitList list)
    {
        list.Add(this);
        _behavior = _moveController;
        _moveController = new(this, _speed);
        _extractionController = new(this);
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
            OnPickItem?.Invoke(this);
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

    public void PutInSorage(Storage storage)
    {

    }

    private IEnumerator AwaitOfMove(Action afterAction)
    {
        while (!_moveController.HasPath)
            yield return null;
        while (_moveController.HasPath)
            yield return null;
        afterAction?.Invoke();

    }
}