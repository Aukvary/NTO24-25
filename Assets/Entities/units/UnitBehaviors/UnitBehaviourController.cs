using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
            {
                Unit.BehaviourAnimation.OnPunchAnimationEvent -= TryInteract;
            }
            else
            {
                Unit.BehaviourAnimation.OnPunchAnimationEvent += TryInteract;
                TargetPosition = value.Transform.position;
            }
        }
    }

    public Vector3 TargetPosition
    {
        get => _targetPosition;

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
            Ray ray = new(Unit.transform.position, Vector3.up + _targetPosition - Unit.transform.position);
            RaycastHit hit;
            try
            {
                hit = Physics.RaycastAll(ray)
                .FirstOrDefault(hit => hit.transform.gameObject == TargetTransform?.gameObject);
                return hit;
            }
            catch (MissingReferenceException)
            {
                return default;
            }

        }
    }

    public bool HasPath
        => TargetHit.collider == null ? true : Vector3.Distance(TargetHit.point, Unit.transform.position) > Range;

    public UnitBehaviourController(Unit unit, float range)
    {
        Unit = unit;
        Range = range;
        NavMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 pos)
    {
        Target = null;
        if (NavMeshAgent.enabled)
            NavMeshAgent.destination = pos;
    }

    public void BehaviourUpdate()
    {
        SetAnimation();
        if (!HasPath && NavMeshAgent.hasPath)
            NavMeshAgent.ResetPath();

        if (NavMeshAgent.hasPath)
            return;

        if (Target == null)
            return;
        var direction = TargetPosition - Unit.transform.position;
        direction.y = Unit.transform.position.y;

        var angle = Quaternion.LookRotation(direction);
        Unit.transform.rotation = Quaternion.RotateTowards(
            Unit.transform.rotation,
            angle,
            Time.deltaTime * NavMeshAgent.angularSpeed
            );

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
        if (HasPath)
        {
            if (TargetTransform != null)
                NavMeshAgent.destination = TargetTransform.position;
            else
            {
                Target = null;
                return;
            }
        }
            
        if (Target.CanInteract(Unit))
            Target.Interact(Unit);
        else
            Target = null;
    }
}