namespace NTO24
{
    public abstract class Unit : Entity, IHealthable, IMovable, IStatsable, ITaskSolver, IAnimationable
    {
        public float StopDistance { get; protected set; }

        public EntityHealth HealthController { get; private set; }

        public MovementController MovementController { get; private set; }

        public StatsController StatsController { get; private set; }

        public TaskController TaskController { get; private set; }

        public AnimationController AnimationController { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            HealthInitialize();
            MovementInitialize();

            StatsController = GetComponent<StatsController>();
            TaskController = GetComponent<TaskController>();
            AnimationController = GetComponentInChildren<AnimationController>();
        }

        protected virtual void HealthInitialize()
        {
            HealthController = GetComponent<EntityHealth>();
        }

        protected virtual void MovementInitialize()
        {
            MovementController = GetComponent<MovementController>();
        }
    }
}