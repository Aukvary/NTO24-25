using UnityEngine;

public abstract class UnitBehaviour
{
    public Unit Unit { get; private set; }

    public UnitBehaviour(Unit unit)
    {
        Unit = unit;
    }

    public virtual void BehaviourEnter() { }
    public virtual void BehaviourExit() { }

    public virtual void BehaviourUpdate() { }
}