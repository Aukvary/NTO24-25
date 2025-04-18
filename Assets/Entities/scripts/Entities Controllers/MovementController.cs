using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace NTO24
{
    public class MovementController : EntityComponent
    {
        [field: SerializeField]
        public UnityEvent<Vector3> OnDestinationChangedEvent { get; private set; }

        private NavMeshAgent _navMeshAgent;

        private EntityStat _speedStat;

        public float Speed
        {
            get => _navMeshAgent.speed;

            set => _navMeshAgent.speed = value;
        }

        public Vector3 Destination
        {
            get => _navMeshAgent.destination;

            set
            {
                _navMeshAgent.destination = value;
                OnDestinationChangedEvent?.Invoke(value);
            }
        }

        public float AngularSpeed => _navMeshAgent.angularSpeed;

        public bool HasPath => _navMeshAgent.hasPath;

        protected override void Start()
        {
            base.Start();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            if (Entity is not IStatsable stats)
                throw new System.Exception("stats component was missed");
            _speedStat = stats[StatNames.Speed];
            _navMeshAgent.speed = _speedStat.StatValue;

            _speedStat.AddOnLevelChangeAction(()=> _navMeshAgent.speed = _speedStat.StatValue);

        }

        public void ResetPath()
            => _navMeshAgent.ResetPath();
    }
}