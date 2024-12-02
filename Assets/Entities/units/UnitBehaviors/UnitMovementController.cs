using UnityEngine;
using System.Linq;
using System;

public class UnitMovementController : UnitBehaviour
{
    private float _speed;

    private Unit _unit;
    private Unit _followUnit;

    private Vector3 _targetPosition;

    public event Action OnMoveEndEvent;

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
            Unit.StopCoroutine(StartEvent());
            OnMoveEndEvent = null;
            Unit.Behaviour = this;
            NavMeshAgent.destination = value;
            Unit.StartCoroutine(StartEvent());
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

    public override void BehaviourEnter()
    {
        Unit.Animator.SetTrigger("move");
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(NavMeshAgent.hasPath ? "move" : "idle");
        if (NavMeshAgent.hasPath)
            return;
    }
    public override void BehaviourExit()
    {
        NavMeshAgent.ResetPath();
        Unit.Animator.SetTrigger("idle");
        OnMoveEndEvent = null;
    }

    private System.Collections.IEnumerator StartEvent()
    {
        while (!NavMeshAgent.hasPath)
            yield return null;
        while (NavMeshAgent.hasPath)
            yield return null;
        OnMoveEndEvent?.Invoke();
        OnMoveEndEvent = null;
    }
}
