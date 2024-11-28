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
        _navMeshAgent.speed = speed;
        _speed = speed;
    }

    public Vector3 Position => Unit.transform.position;

    public Vector3 TargetPosition
    {
        get => _targetPosition;

        set
        {
            Unit.Behaviour = this;
            _navMeshAgent.destination = value;
        }
    }

    public bool HasPath => _navMeshAgent.hasPath;

    public override void BehaviourEnter()
    {
        Unit.Animator.SetTrigger("move");
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(_navMeshAgent.hasPath ? "move" : "idle");
    }
    public override void BehaviourExit()
    {
        _navMeshAgent.ResetPath();
        Unit.Animator.SetTrigger("idle");
    }
}
