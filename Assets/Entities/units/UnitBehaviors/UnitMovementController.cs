using UnityEngine;
using System.Linq;
using System;

public class UnitMovementController : UnitBehaviour
{
    private Vector3 _targetPosition;

    public event Action OnMoveEndEvent;

    public UnitMovementController(Unit unit, float speed, float range) : base(unit, range)
    {
        NavMeshAgent.speed = speed;
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

    public override void BehaviourEnter()
    {
        Unit.Animator.SetTrigger("move");
    }

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
