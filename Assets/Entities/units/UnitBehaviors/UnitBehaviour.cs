public abstract class UnitBehaviour
{
    public Unit Unit { get; private set; }

    public float Range { get; private set; }

    protected UnityEngine.AI.NavMeshAgent NavMeshAgent { get; private set; }

    public UnitBehaviour(Unit unit, float range)
    {
        Unit = unit;
        Range = range;
        NavMeshAgent = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public virtual void BehaviourEnter() { }
    public virtual void BehaviourExit() { }

    public virtual void BehaviourUpdate() { }
}