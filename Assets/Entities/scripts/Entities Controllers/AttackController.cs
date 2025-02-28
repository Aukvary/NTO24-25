using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class AttackController : EntityComponent
    {
        [SerializeField]
        private float _attackAngle;

        [SerializeField]
        private UnityEvent<IHealthable> _onChangeTargetEvent;

        [SerializeField]
        private UnityEvent<IHealthable> _onAttackEvent;

        private IHealthable _target;

        private EntityStat _rangeStat;
        private EntityStat _damageStat;

        public IHealthable Target
        {
            get => _target;

            set
            {
                _target = value;
                _onChangeTargetEvent.Invoke(value);
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
                Debug.DrawRay(transform.position, end - transform.position, Color.red);
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
                _rangeStat = stats[EntityStatsType.AttackRange];
                _damageStat = stats[EntityStatsType.Damage];
            }
            else throw new System.Exception("stats component was missed");
        }

        public void AddOnTargetChaneAction(UnityAction<IHealthable> action)
            => _onChangeTargetEvent.AddListener(action);

        public void RemoveOnTargetChaneAction(UnityAction<IHealthable> action)
            => _onChangeTargetEvent.RemoveListener(action);

        public void AddOnAttackAction(UnityAction<IHealthable> action)
            => _onAttackEvent.AddListener(action);

        public void RemoveOnAttackAction(UnityAction<IHealthable> action)
            => _onAttackEvent.RemoveListener(action);
    }
}