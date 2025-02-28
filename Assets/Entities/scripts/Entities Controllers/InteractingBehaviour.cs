using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace NTO24
{
    public class InteractingBehaviour : EntityComponent
    {
        [SerializeField]
        private UnityEvent<IInteractable> _onChangeTargetEvent;

        [SerializeField]
        private UnityEvent<IInteractable> _onInteractEvent;

        [SerializeField]
        private UnityEvent _onInteractFailedEvent;

        [field: SerializeField]
        private UnityEvent<bool> _onPossibilityChangeEvent;

        private EntityStat _rangeStat;

        private IInteractable _target;

        private bool _canInteract;

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

                if (can != _canInteract)
                { 
                    _canInteract = can; 
                    _onPossibilityChangeEvent.Invoke(can); 
                }

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

        public void TryInteract()
        {
            if (CanInteract)
            {
                Target.InteractBy(Entity as IInteractor);
                _onInteractEvent?.Invoke(Target);
            }
            else
                _onInteractFailedEvent.Invoke();
        }

        public void AddOnChangeTargetAction(UnityAction<IInteractable> action)
            => _onChangeTargetEvent.AddListener(action);

        public void RemoveOnChangeTargetAction(UnityAction<IInteractable> action)
            => _onChangeTargetEvent.RemoveListener(action);

        public void AddOnInteractAction(UnityAction<IInteractable> action)
            => _onInteractEvent.AddListener(action);

        public void RemoveOnInteractAction(UnityAction<IInteractable> action)
            => _onInteractEvent.RemoveListener(action);

        public void AddOnInteractFailedAction(UnityAction action)
            => _onInteractFailedEvent.AddListener(action);

        public void RemoveOnInteractFailedAction(UnityAction action)
            => _onInteractFailedEvent.RemoveListener(action);

        public void AddOnPossibilityChangeAction(UnityAction<bool> action)
            => _onPossibilityChangeEvent.AddListener(action);

        public void RemoveOnPossibilityChangeAction(UnityAction<bool> action)
            => _onPossibilityChangeEvent.RemoveListener(action);
    }
}