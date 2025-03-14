using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public enum DamageType
    {
        Damage,
        Extract
    }

    public class EntityHealth : EntityComponent, ISavableComponent
    {
        [field: SerializeField]
        public DamageType DamageBy { get; private set; }

        [field: SerializeField]
        public  UnityEvent<Entity, HealthChangeType> OnHealthChangeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<Entity> OnDeathEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent OnRevivalEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<bool> OnAliveChangeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent OnUpgadeEvent { get; private set; }

        public UnityEvent OnDataChangeEvent { get; private set; } = new();

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
                    OnRevivalEvent.Invoke();
                }
            } 
        }

        public string Name => "Entity Health";

        public string[] Data => new string[] { Health.ToString() , Alive.ToString()};

        protected override void Awake()
        {
            if (!TryGetComponent<StatsController>(out var stats))
                throw new System.Exception("Stats component was missed");

            _maxHealth = stats[StatNames.MaxHealth];
            try
            {
                _regeneration = stats[StatNames.Regeneration];
            }
            catch (StatMissedException) { _regeneration = null; }


            _maxHealth.AddOnLevelChangeAction(OnUpgadeEvent.Invoke);
            _regeneration?.AddOnLevelChangeAction(OnUpgadeEvent.Invoke);

            Health = _maxHealth.StatValue;
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            Health = float.Parse(data.ElementAt(0));

            bool alive = bool.Parse(data.ElementAt(1));

            if (!alive)
                Alive = alive;
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