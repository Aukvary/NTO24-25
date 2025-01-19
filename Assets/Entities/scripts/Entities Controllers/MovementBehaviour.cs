using UnityEngine;
using UnityEngine.AI;

public class MovementBehaviour : EntityComponent
{
    private NavMeshAgent _navMeshAgent;

    private EntityStat _speedStat;

    public float Speed
    {
        get => _navMeshAgent.speed;

        set => _navMeshAgent.speed = value;
    }

    public Vector3 TargetPosition
    {
        get => _navMeshAgent.destination;

        set => _navMeshAgent.destination = value;
    }

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (Entity is not IStatsable stats)
            throw new System.Exception("stats component was missed");
        _speedStat = stats[EntityStatsType.Speed];
        _navMeshAgent.speed = _speedStat.StatValue;

        _speedStat.AddOnLevelChangeAction(_ => _navMeshAgent.speed = _speedStat.StatValue);

    }

    public void ResetPath()
        => _navMeshAgent.ResetPath();
}