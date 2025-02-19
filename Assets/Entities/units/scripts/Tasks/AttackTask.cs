public struct AttackTask : IUnitTask
{
    private IHealthable _target;

    public Unit Unit { get; private set; }

    public bool IsComplete => !_target.Alive;

    public AttackTask(IAttacker unit, IHealthable target)
    {
        Unit = unit as Unit;
        _target = target;
    }

    public void Enter()
    {
        (Unit as IAttacker).Attack(_target);
    }

    public void Exit()
    {
        (Unit as IAttacker).Stop();
    }
}