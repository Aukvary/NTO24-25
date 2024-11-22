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
            if (value == null)
            {
                Unit.BehaviourAnimation.OnPunchAnimationEvent -= Extract;
                Unit.Behaviour = null;
            }
            else
            {
                Unit.Behaviour = this;
                Unit.BehaviourAnimation.OnPunchAnimationEvent += Extract;
            }
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

    public override void BehaviourUpdate()
    {
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
}