public struct AttackTask : IUnitTask
{
    public IHealthable Target { get; private set; }

    public Unit Unit { get; private set; }

    public bool IsComplete => !Target.Alive;

    public AttackTask(IAttacker unit, IHealthable target)
    {
        Unit = unit as Unit;
        Target = target;
    }

    public void Enter()
    {
        (Unit as IAttacker).SetTarget(Target);
    }

    public void Exit()
    {
        (Unit as IAttacker).Stop();
    }
}