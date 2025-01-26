public struct AttackTask : UnitTask
{
    private IHealthable _target;

    public Unit Unit { get; private set; }

    public bool IsComplete => _target.Alive;

    public AttackTask(Unit unit, IHealthable target)
    {
        if (unit is not IAttacker)
            throw new System.Exception($"Unit {unit.GetType().Name} can't attack anything");

        Unit = unit;
        _target = target;
    }

    public void Enter()
    {
        (Unit as IAttacker).Attack(_target);
    }

    public void Exit()
    {
        (Unit as IAttacker).Attack(null);
    }
}