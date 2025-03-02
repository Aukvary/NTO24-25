using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace NTO24
{
    public class InteractingController : EntityComponent
    {
        [SerializeField]
        private UnityEvent<IInteractable> _onChangeTargetEvent;

        [SerializeField]
        private UnityEvent<IInteractable> _onInteractEvent;

        private EntityStat _rangeStat;

        private IInteractable _target;

        public IInteractable Target
        {
            get => _target;

            set
            {
                _target = value;
                _onChangeTargetEvent.Invoke(value);
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
                _rangeStat = stats[EntityStatsType.InteractRange];
            }
            else throw new System.Exception("stats component was missed");
        }

        public void AddOnChangeTargetAction(UnityAction<IInteractable> action)
            => _onChangeTargetEvent.AddListener(action);

        public void RemoveOnChangeTargetAction(UnityAction<IInteractable> action)
            => _onChangeTargetEvent.RemoveListener(action);

        public void AddOnInteractAction(UnityAction<IInteractable> action)
            => _onInteractEvent.AddListener(action);

        public void RemoveOnInteractAction(UnityAction<IInteractable> action)
            => _onInteractEvent.RemoveListener(action);
    }
}