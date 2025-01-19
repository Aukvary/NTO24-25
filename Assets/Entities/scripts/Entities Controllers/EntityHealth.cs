using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : EntityComponent
{
    [Header("Health Events")]
    [SerializeField]
    private UnityEvent<Entity, HealthChangeType> OnHealthChangeEvent;

    [SerializeField]
    private UnityEvent<Entity> OnDeathEvent;

    [SerializeField]
    private UnityEvent<bool> OnAliveChangeEvent;

    private float _currentHealth;

    private IStatsable _entityStats;
    private EntityStat _maxHealth;
    private EntityStat _regeneration;

    private bool _alive;

    public float Health
    {
        get => _currentHealth;

        private set => _currentHealth = value;
    }

    public float Regeneration { get; private set; }

    public float MaxHealth { get; private set; }

    public bool Alive 
    {
        get => _alive;

        set
        {
            OnAliveChangeEvent.Invoke(value);
            _alive = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _entityStats = Entity.GetComponent<IStatsable>();
        _maxHealth = _entityStats[EntityStatsType.MaxHealth];
        _maxHealth.AddOnLevelChangeAction(_ => MaxHealth = _maxHealth.StatValue);

        _regeneration = _entityStats[EntityStatsType.Regeneration];
        _regeneration.AddOnLevelChangeAction(_ => Regeneration = _regeneration.StatValue);
    }

    protected override void Update()
    {
        if (!Alive)
            return;
        ChangeHealth(Regeneration * Time.deltaTime, HealthChangeType.Heal);
    }

    public void ChangeHealth(float deltaHealth, HealthChangeType type, Entity by = null)
    {
        Health = Mathf.Clamp(Health + deltaHealth, 0, MaxHealth);

        OnHealthChangeEvent.Invoke(by, type);

        if (Health > 0)
            return;

        Alive = false;
        OnDeathEvent.Invoke(by);
    }

    public void AddOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
        => OnHealthChangeEvent.AddListener(action);
    public void RemoveOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
        => OnHealthChangeEvent.RemoveListener(action);
    public void AddOnDeathAction(UnityAction<Entity> action)
        => OnDeathEvent.AddListener(action);
    public void RemoveOnDeathAction(UnityAction<Entity> action)
        => OnDeathEvent.RemoveListener(action);
    public void AddOnAliveChangeAction(UnityAction<bool> action)
        => OnAliveChangeEvent.AddListener(action);
    public void RemoveOnAliveChangeAction(UnityAction<bool> action)
        => OnAliveChangeEvent.RemoveListener(action);
}