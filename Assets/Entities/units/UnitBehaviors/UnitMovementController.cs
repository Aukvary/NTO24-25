using UnityEngine;
using UnityEngine.AI;
public class UnitMovementController : UnitBehaviour
{
    private float _speed;

    private Unit _unit;
    private Unit _followUnit;

    private NavMeshAgent _navMeshAgent;

    public override UnitStates UnitState => UnitStates.Walk;

    public UnitMovementController(Unit unit, float speed) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
        _speed = speed;
    }

    public Vector3 TargetPosition 
    {
        get => _navMeshAgent.destination;

        set
        {
            if (Unit.Behavior != this)
                Unit.Behavior = this;
            _navMeshAgent.destination = value;
        }
    }

    public Unit FollowUnit
    {
        get => _followUnit;

        set
        {
            if (value != null)
            {
                Unit.Behavior = this;

            }
            _followUnit = value;
        }
    }

    public bool HasPath => _navMeshAgent.hasPath;
    public override void BehaviourExit()
    {
        _navMeshAgent.ResetPath();
        FollowUnit = null;
    }

    public System.Collections.IEnumerator StartFollow()
    {
        while (_followUnit != null)
        {
            TargetPosition = _followUnit.transform.position;
            yield return null;
        }
    }
}
