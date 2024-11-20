using UnityEngine;
using UnityEngine.AI;
public class UnitMoveController : UnitBehaviour
{
    private float _speed;

    private Unit _unit;

    private NavMeshAgent _navMeshAgent;

    public override UnitStates UnitState => UnitStates.Walk;

    public UnitMoveController(Unit unit, float speed) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
        _speed = speed;
    }

    public Vector3 TargetPosition 
    {
        get => _navMeshAgent.destination;

        set
        {
            Unit.Behavior = this;
            _navMeshAgent.destination = value;
        }
    }
    public override void BehaviourExit()
    {
        _navMeshAgent.ResetPath();
    }
}
