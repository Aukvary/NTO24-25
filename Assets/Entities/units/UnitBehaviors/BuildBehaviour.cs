using UnityEngine;
using System.Linq;

public class BuildBehaviour : UnitBehaviour
{
    private ConstructionObject _build;

    public ConstructionObject Build
    {
        get => _build;

        set
        {
            _build = value;
            Unit.Behaviour = value == null ? null : this;
        }
    }

    private RaycastHit _targetHit
    {
        get
        {
            Ray ray = new(Unit.transform.position, Vector3.up + Build.transform.position - Unit.transform.position);
            var hit = Physics.RaycastAll(ray, Range).FirstOrDefault(hit => hit.transform == Build.transform);
            return hit;
        }
    }

    private bool _hasPath
        => _targetHit.collider == null ? true : Vector3.Distance(_targetHit.point, Unit.transform.position) > Range;

    public BuildBehaviour(Unit unit, float range) :
        base(unit, range)
    { }

    private void ToBuild()
    {
        if (Build != null)
            Build.Interact(Unit);
        else
            Unit.Behaviour = null;
    }

    public override void BehaviourEnter()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent += ToBuild;

        NavMeshAgent.destination = Build.transform.position;
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(NavMeshAgent.hasPath ? "move" : "punch");

        if (Build == null)
        {
            Build = null;
            return;
        }

        Unit.Animator.SetTrigger(_hasPath ? "move" : "punch");


        if (!_hasPath)
            NavMeshAgent.ResetPath();


        if (_hasPath)
            return;

        var direction = Build.transform.position - Unit.transform.position;
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
        Unit.BehaviourAnimation.OnPunchAnimationEvent -= ToBuild;
    }
}