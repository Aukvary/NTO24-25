using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntityHealth : EntityComponent
    {
        [SerializeField]
        private UnityEvent<Entity, HealthChangeType> _onHealthChangeEvent;

        [SerializeField]
        private UnityEvent<Entity> _onDeathEvent;

        [SerializeField]
        private UnityEvent<bool> _onAliveChangeEvent;

        [SerializeField]
        private UnityEvent _onUpgadeEvent;

        private float _currentHealth;

        private EntityStat _maxHealth;
        private EntityStat _regeneration;

        private bool _alive = true;

        public float Health
        {
            get => _currentHealth;

            private set => _currentHealth = value;
        }

        public float Regeneration => _regeneration?.StatValue ?? 0;

        public float MaxHealth => _maxHealth.StatValue;

        public bool Alive 
        {
            get => _alive;

            set
            {
                _alive = value;
                _onAliveChangeEvent.Invoke(value);

                if (value)
                {
                    Health = MaxHealth;
                    _onHealthChangeEvent.Invoke(Entity, HealthChangeType.Heal);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            if (Entity is not IStatsable stats)
                throw new System.Exception("Stats component was missed");

            _maxHealth = stats[StatsNames.MaxHealth];
            _regeneration = stats[StatsNames.Regeneration];

            _maxHealth.AddOnLevelChangeAction(_onUpgadeEvent.Invoke);
            _regeneration?.AddOnLevelChangeAction(_onUpgadeEvent.Invoke);

            Health = _maxHealth.StatValue;
        }

        protected override void Update()
        {
            if (!Alive)
                return;
            ChangeHealth(Regeneration * Time.deltaTime, HealthChangeType.Heal);
        }

        public void ChangeHealth(float deltaHealth, HealthChangeType type, Entity by = null)
        {
            Health = Mathf.Clamp(Health + deltaHealth * (type == HealthChangeType.Heal ? 1 : -1), 0, MaxHealth);

            _onHealthChangeEvent.Invoke(by, type);

            if (Health > 0)
                return;

            Alive = false;

            _onDeathEvent.Invoke(by);
        }

        public void AddOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
            => _onHealthChangeEvent.AddListener(action);
        public void RemoveOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
            => _onHealthChangeEvent.RemoveListener(action);

        public void AddOnDeathAction(UnityAction<Entity> action)
            => _onDeathEvent.AddListener(action);
        public void RemoveOnDeathAction(UnityAction<Entity> action)
            => _onDeathEvent.RemoveListener(action);

        public void AddOnAliveChangeAction(UnityAction<bool> action)
            => _onAliveChangeEvent.AddListener(action);
        public void RemoveOnAliveChangeAction(UnityAction<bool> action)
            => _onAliveChangeEvent.RemoveListener(action);

        public void AddOnUpgradeAction(UnityAction action)
            => _onUpgadeEvent.AddListener(action);
        public void RemoveOnUpgradeAction(UnityAction action)
            => _onUpgadeEvent.RemoveListener(action);
    }
}