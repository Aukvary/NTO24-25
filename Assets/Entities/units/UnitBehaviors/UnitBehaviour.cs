using UnityEngine;

public abstract class UnitBehaviour
{
    public Unit Unit { get; private set; }

    public abstract UnitStates UnitState { get; }

    public UnitBehaviour(Unit unit)
    {
        Unit = unit;
    }

    public virtual void BehaviourEnter() { }
    public virtual void BehaviourExit() { }
}