using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntityHealth : EntityComponent
    {
        [field: SerializeField]
        public  UnityEvent<Entity, HealthChangeType> OnHealthChangeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<Entity> OnDeathEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<bool> OnAliveChangeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent OnUpgadeEvent { get; private set; }

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
                OnAliveChangeEvent.Invoke(value);

                if (value)
                {
                    Health = MaxHealth;
                    OnHealthChangeEvent.Invoke(Entity, HealthChangeType.Heal);
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

            _maxHealth.AddOnLevelChangeAction(OnUpgadeEvent.Invoke);
            _regeneration?.AddOnLevelChangeAction(OnUpgadeEvent.Invoke);

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

            OnHealthChangeEvent.Invoke(by, type);

            if (Health > 0)
                return;

            Alive = false;

            OnDeathEvent.Invoke(by);
        }
    }
}