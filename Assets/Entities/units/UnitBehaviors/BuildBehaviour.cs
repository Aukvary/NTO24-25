using Unity.VisualScripting;
using UnityEngine;

public class BuildBehaviour : UnitBehaviour
{
    private ConstructionObject _build;


    private UnityEngine.AI.NavMeshAgent _navMeshAgent;

    public ConstructionObject Build
    {
        get => _build;

        set 
        { 
            _build = value;
            if (value == null)
            {
                Unit.Behaviour = null;
            }
            else
            {
                Unit.Behaviour = this;
            }
        }
    }

    public BuildBehaviour(Unit unit) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void ToBuild()
    {
        if (Build != null)
            Build.Interact(Unit);
        else
        {
            Unit.Behaviour = null;
        }

    }

    public override void BehaviourEnter()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent += ToBuild;

        _navMeshAgent.destination = Build.transform.position;
    }

    public override void BehaviourUpdate()
    {
        if (_navMeshAgent.hasPath)
            return;
        if (Build == null)
            return;

        var direction = Build.transform.position - Unit.transform.position;
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
        Unit.BehaviourAnimation.OnPunchAnimationEvent -= ToBuild;
    }
}