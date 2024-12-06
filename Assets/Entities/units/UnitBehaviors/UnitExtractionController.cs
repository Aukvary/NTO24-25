using System.Linq;
using UnityEngine;

public class UnitExtractionController : UnitBehaviour
{
    private ResourceObjectSpawner _resource;
    public ResourceObjectSpawner Resource
    {
        get => _resource;
        set
        {
            if (Unit.Inventory.Resources.All(c => c.OverFlow || 
            (c.Resource != Resource && c.Resource != null)) && 
            value != null)
                Resource = null;

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

    public bool Extracting => Resource != null && !NavMeshAgent.hasPath && !_hasPath;
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
    }

    public override void BehaviourUpdate()
    {

        if (Resource == null)
        {
            Resource = null;
            return;
        }

        Unit.Animator.SetTrigger(_hasPath ? "move" : "punch");


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