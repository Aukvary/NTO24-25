using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class AttackController : EntityComponent
    {
        [SerializeField]
        private float _attackAngle;

        [field: SerializeField]
        public UnityEvent<IHealthable> OnChangeTargetEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<IHealthable> OnAttackEvent { get; private set; }

        private IHealthable _target;

        private EntityStat _rangeStat;
        private EntityStat _damageStat;
        private EntityStat _extractPowerStat;

        public IHealthable Target
        {
            get => _target;

            set
            {
                _target = value;
                OnChangeTargetEvent.Invoke(value);
            }
        }

        public bool CanAttack
        {
            get
            {
                if (Target == null)
                    return false;

                Ray ray = new(transform.position, (TargetPosition + Vector3.up * 0.5f) - transform.position);

                var hit = Physics.RaycastAll(ray)
                    .FirstOrDefault(h => h.transform == Target.EntityReference.transform);


                Vector3 end = hit.point;

                bool can = Vector3.Distance(transform.position, end) <= Range;

                return can;
            }
        }

        public float Range => _rangeStat.StatValue;
        public float Damage => _damageStat.StatValue;
        public Vector3 TargetPosition => Target.EntityReference.transform.position;

        protected override void Start()
        {
            base.Start();
            if (Entity is not IStatsable stats)
                throw new System.Exception("stats component was missed");

            _rangeStat = stats[StatNames.AttackRange];
            _damageStat = stats[StatNames.Damage];

            try { _extractPowerStat = stats[StatNames.InteractPower]; }
            catch(StatMissedException) { _extractPowerStat = null; }
            

        }

        public float GetDamage(DamageType type)
            => type == DamageType.Damage ? _damageStat.StatValue : _extractPowerStat.StatValue;

    }
}