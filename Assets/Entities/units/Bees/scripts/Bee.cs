namespace NTO24
{
    public class Bee : Unit, IDropable, IAttacker
    {
        public DropController DropController { get; private set; }

        public AttackController AttackController { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            DropController = GetComponent<DropController>();
            AttackController = GetComponent<AttackController>();

            HealthController.OnDeathEvent.AddListener(entity =>
            {
                DropController.Drop();

                Destroy(gameObject, 0.1f);
            });
        }
    }
}