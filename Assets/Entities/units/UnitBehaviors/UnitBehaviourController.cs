using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class UnitBehaviourController
{
    private IInteractable _targetObject;

    private Vector3 _targetPosition;

    public Unit Unit { get; private set; }

    public float Range { get; private set; }

    protected NavMeshAgent NavMeshAgent { get; private set; }

    public IInteractable Target
    {
        get => _targetObject;

        set
        {
            _targetObject = value;
            if (value == null)
                Unit.BehaviourAnimation.OnPunchAnimationEvent -= TryInteract;
            else
            {
                Unit.BehaviourAnimation.OnPunchAnimationEvent += TryInteract;
                TargetPosition = value.Transform.position;
            }
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            try
            {
                return Target.Transform.position;
            }
            catch (NullReferenceException) { return _targetPosition; }
        }

        private set
        {
            _targetPosition = value;
            NavMeshAgent.destination = value;
        }
    }

    public Transform TargetTransform => Target?.Transform;

    public RaycastHit TargetHit
    {
        get
        {
            Vector3 start = Unit.transform.position;
            Vector3 end = TargetPosition - Unit.transform.position;

            if (Target is Unit)
                end += Vector3.up;

            Ray ray = new(start, end);
            Debug.DrawRay(start, end, Color.red);
            RaycastHit hit;
            hit = Physics.RaycastAll(ray)
                .FirstOrDefault(hit => hit.transform == TargetTransform && hit.transform != null);
            return hit;

        }
    }

    public bool HasPath
        => TargetHit.transform == null ? true : Vector3.Distance(TargetHit.point, Unit.transform.position) > Range;

    public UnitBehaviourController(Unit unit, float range)
    {
        Unit = unit;
        Range = range;
        NavMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 pos)
    {
        Target = null;
        TargetPosition = pos;
        try
        {
            if (NavMeshAgent.enabled)
                NavMeshAgent.destination = pos;
        }
        catch { }
    }

    public void BehaviourUpdate()
    {
        SetAnimation();

        if (!HasPath && NavMeshAgent.hasPath)
            NavMeshAgent.ResetPath();
        else if (HasPath && Target != null && Target is Unit unit)
            NavMeshAgent.destination = TargetPosition;

        if (NavMeshAgent.hasPath || Target == null)
            return;


        

    }

    private void SetAnimation()
    {
        if (NavMeshAgent.hasPath)
            Unit.Animator.SetTrigger("move");
        else if (Target != null)
            Unit.Animator.SetTrigger("punch");
        else
            Unit.Animator.SetTrigger("idle");
    }

    private void TryInteract()
    {
        if (Target == null) 
            return;
        if (!Target.CanInteract(Unit))
            Target = null;
        if (HasPath)
        {
            if (TargetTransform != null)
                NavMeshAgent.destination = TargetTransform.position;
            else
                Target = null;
            return;
        }
        if (Target.CanInteract(Unit))
            Target.Interact(Unit);
    }
}