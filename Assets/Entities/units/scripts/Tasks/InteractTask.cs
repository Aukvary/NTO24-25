namespace NTO24
{
    public struct InteractTask : IUnitTask
    {
        private IInteractor _unit;

        public IInteractable Target { get; private set; }

        public Entity Entity => _unit.EntityReference;

        public bool IsComplete => !Target.Interactable;

        public InteractTask(IInteractor unit, IInteractable target)
        {
            _unit = unit;
            Target = target;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}