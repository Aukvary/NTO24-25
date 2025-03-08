using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace NTO24
{
    public class InteractingController : EntityComponent
    {
        [field: SerializeField]
        public UnityEvent<IInteractable> OnChangeTargetEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<IInteractable> OnInteractEvent { get; private set; }

        private EntityStat _rangeStat;

        private IInteractable _target;

        public IInteractable Target
        {
            get => _target;

            set
            {
                _target = value;
                OnChangeTargetEvent.Invoke(value);
            }
        }

        public Vector3 TargetPosition => Target.EntityReference.transform.position;

        public bool CanInteract
        {
            get
            {
                Ray ray = new(transform.position, TargetPosition - transform.position);

                var hit = Physics.RaycastAll(ray).First(h => h.transform == Target.EntityReference.transform);

                bool can = Vector3.Distance(transform.position, hit.point) < Range;

                return can;
            }
        }

        public float Range => _rangeStat.StatValue;

        protected override void Awake()
        {
            base.Awake();
            if (Entity is IStatsable stats)
            {
                _rangeStat = stats[StatsNames.InteractRange];
            }
            else throw new System.Exception("stats component was missed");
        }
    }
}