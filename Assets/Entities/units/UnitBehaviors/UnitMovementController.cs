using UnityEngine;
using UnityEngine.AI;
public class UnitMovementController : UnitBehaviour
{
    private float _speed;

    private Unit _unit;
    private Unit _followUnit;

    private NavMeshAgent _navMeshAgent;

    private Vector3 _targetPosition;

    public UnitMovementController(Unit unit, float speed) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
        _speed = speed;
    }

    public Vector3 Position => Unit.transform.position;

    public Vector3 TargetPosition
    {
        get => _targetPosition;

        set
        {
            if (Unit.Behaviour != this)
                Unit.Behaviour = this;
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
                Unit.Behaviour = this;

            }
            _followUnit = value;
        }
    }

    public bool HasPath => _navMeshAgent.hasPath;

    public override void BehaviourUpdate()
    {

    }
    public override void BehaviourExit()
    {
        _navMeshAgent.ResetPath();
        FollowUnit = null;
    }
}
