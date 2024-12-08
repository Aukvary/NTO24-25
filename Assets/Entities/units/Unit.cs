using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class Unit : MonoBehaviour, IInteractable
{

    [Header("Stats")]

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private float _attackAngle;

    [SerializeField]
    private List<float> _damage;

    [SerializeField]
    private List<float> _strength;

    [SerializeField]
    private List<float> _maxHealth;

    [SerializeField]
    private List<float> _regeneration;  

    private float _health;


    private Animator _animator;
    private BehaviourAnimation _behaviourAnimation;
    private NavMeshAgent _navMeshAgent;
    
    
    private int _attackLevel;
    private int _strenghtLevel;
    private int _healthLevel;

    private Transform _transform;

    public UnitBehaviourController Behaviour { get; private set; }

    public float Damage => _damage[AttackLevel];
    public float Strength => _strength[StrenghtLevel];
    public float MaxHealth => _maxHealth[HealthLevel];
    public float Regeneration => _regeneration[_healthLevel];
    public float Speed => _speed;

    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChangeEvent?.Invoke(this);
        }
    }

    public BehaviourAnimation BehaviourAnimation => _behaviourAnimation;

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

    public Transform Transform => _transform;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    public event Action<Unit> OnHealthChangeEvent;

    public event Action<Unit> OnLevelUpEvent;


    public event Action<Unit> OnLevelUpDamageEvent;
    public event Action<Unit> OnLevelUpStrenghtEvent;
    public event Action<Unit> OnLevelUpHealthEvent;


    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _behaviourAnimation = GetComponentInChildren<BehaviourAnimation>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Behaviour = new(this, _attackRange);
        _health = MaxHealth;
        _transform = transform;
    }

    private void Update()
    {
        if (Health > 0) 
            Behaviour?.BehaviourUpdate();
        if (Health > 0)
            Health += Regeneration * Time.deltaTime;
    }

    public void MoveTo(Vector3 newPostion)
    {
        Behaviour.MoveTo(newPostion);
    }

    public void InteractWith(IInteractable obj)
    {
        if (Health > 0 && obj != Behaviour.Target)
            Behaviour.Target = obj;
    }

    public virtual bool CanInteract(Unit unit)
    {
        try
        {
            var hit = unit.Behaviour.TargetHit;

            if (unit.Behaviour.HasPath)
            {
                unit.NavMeshAgent.destination = transform.position;
                return false;
            }

            if (hit.collider == null)
                return false;


            if (Vector3.Angle(unit.transform.forward, hit.point - unit.transform.position) > unit._attackAngle)
                return false;
        }
        catch (MissingReferenceException)
        {
            unit.Behaviour.Target = null;
            return false;
        }

        return true;
    }

    public void Interact(Unit unit)
    {
        Health -= unit.Damage;

        if (_health > 0)
            return;
        Behaviour.Target = null;
        Die(unit);
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
                OnLevelUpDamageEvent?.Invoke(this);
                break;

            case UpgradeType.Strenght:
                StrenghtLevel += 1;
                OnLevelUpStrenghtEvent?.Invoke(this);
                break;

            case UpgradeType.Health:
                HealthLevel += 1;
                OnLevelUpHealthEvent?.Invoke(this);
                break;
        }
        OnLevelUpEvent?.Invoke(this);
    }

    public void Upgrade(UpgradeType type, int level)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                AttackLevel = level;
                OnLevelUpDamageEvent?.Invoke(this);
                break;

            case UpgradeType.Strenght:
                StrenghtLevel = level;
                OnLevelUpStrenghtEvent?.Invoke(this);
                break;

            case UpgradeType.Health:
                HealthLevel = level;
                OnLevelUpHealthEvent?.Invoke(this);
                break;
        }
        OnLevelUpEvent?.Invoke(this);
    }

    private void OnDestroy()
    {
        _transform = null;
    }

    protected abstract void Die(Unit unit);
}