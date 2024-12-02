using UnityEngine;
using System.Linq;

public class UnitMovementController : UnitBehaviour
{
    private float _speed;

    private Unit _unit;
    private Unit _followUnit;

    private Vector3 _targetPosition;

    public UnitMovementController(Unit unit, float speed, float range) : base(unit, range)
    {
        NavMeshAgent.speed = speed;
        _speed = speed;
    }

    public Vector3 Position => Unit.transform.position;

    public Vector3 TargetPosition
    {
        get => _targetPosition;

        set
        {
            Unit.Behaviour = this;
            NavMeshAgent.destination = value;
        }
    }

    private RaycastHit _targetHit
    {
        get
        {
            Ray ray = new(Unit.transform.position, Vector3.up + TargetPosition - Unit.transform.position);
            return Physics.RaycastAll(ray, Range).FirstOrDefault(hit => hit.transform.position == TargetPosition);
        }
    }

    private bool _hasPath 
        => _targetHit.collider == null ? true : Vector3.Distance(_targetHit.point, Unit.transform.position) > Range;

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(NavMeshAgent.hasPath ? "move" : "idle");
    }
    public override void BehaviourExit()
    {
        NavMeshAgent.ResetPath();
        Unit.Animator.SetTrigger("idle");
    }
}
