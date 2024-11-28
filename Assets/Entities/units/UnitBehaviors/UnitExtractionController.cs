using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.AI;

public class UnitExtractionController : UnitBehaviour
{
    private ResourceObjectSpawner _resource;

    private NavMeshAgent _navMeshAgent;
    public ResourceObjectSpawner Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            Unit.Behaviour = value == null ? null : this;
        }
    }
    public UnitExtractionController(Unit unit) : 
        base(unit) 
    { 
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

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
        _navMeshAgent.destination = Resource.transform.position;
        Unit.Animator.SetTrigger("move");
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(_navMeshAgent.hasPath ? "move" : "punch");

        if (_navMeshAgent.hasPath)
            return;
        if (Resource == null)
            return;

        var direction = _resource.transform.position - Unit.transform.position;
        direction.y = Unit.transform.position.y;

        var angle = Quaternion.LookRotation(direction);
        Unit.transform.rotation = Quaternion.RotateTowards(
            Unit.transform.rotation,
            angle,
            Time.deltaTime * _navMeshAgent.angularSpeed
            );
    }

    public override void BehaviourExit()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent -= Extract;
    }
}