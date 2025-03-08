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

                Ray ray = new(transform.position, TargetPosition - transform.position);

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



        protected override void Awake()
        {
            base.Awake();
            if (Entity is IStatsable stats)
            {
                _rangeStat = stats[StatsNames.AttackRange];
                _damageStat = stats[StatsNames.Damage];
            }
            else throw new System.Exception("stats component was missed");
        }
    }
}