using UnityEngine;
using System.Linq;

public class UnitExtractionController : UnitBehaviour
{
    private ResourceObjectSpawner _resource;
    public ResourceObjectSpawner Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            Unit.Behaviour = value == null ? null : this;
        }
    }

    private RaycastHit _targetHit
    {
        get
        {
            Ray ray = new(Unit.transform.position, Vector3.up + Resource.transform.position - Unit.transform.position);
            var hit = Physics.RaycastAll(ray, Range).FirstOrDefault(hit => hit.transform == Resource.transform);
            return hit;
        }
    }

    private bool _hasPath
        => _targetHit.collider == null ? true : Vector3.Distance(_targetHit.point, Unit.transform.position) > Range;

    public UnitExtractionController(Unit unit, float range) :
        base(unit, range) { }

    private void Extract()
    {
        if (Resource != null && Resource.IsRestored)
            _resource.Interact(Unit);
        else
            Resource = null;
    }

    public override void BehaviourEnter()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent += Extract;
        NavMeshAgent.destination = Resource.transform.position;
        Unit.Animator.SetTrigger("move");
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(NavMeshAgent.hasPath ? "move" : "punch");

        if (Resource == null)
        {
            Resource = null;
            return;
        }


        if (!_hasPath)
            NavMeshAgent.ResetPath();


        if (_hasPath)
            return;

        var direction = _resource.transform.position - Unit.transform.position;
        direction.y = Unit.transform.position.y;

        var angle = Quaternion.LookRotation(direction);
        Unit.transform.rotation = Quaternion.RotateTowards(
            Unit.transform.rotation,
            angle,
            Time.deltaTime * NavMeshAgent.angularSpeed
            );
    }

    public override void BehaviourExit()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent -= Extract;
    }
}