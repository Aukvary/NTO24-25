public struct InteractTask : IUnitTask
{
    public IInteractable Target { get; private set; }

    public Unit Unit { get; private set; }

    public bool IsComplete => !Target.Interactable;

    public InteractTask(IInteractor unit, IInteractable target)
    {
        Unit = unit as Unit;
        Target = target;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }
}