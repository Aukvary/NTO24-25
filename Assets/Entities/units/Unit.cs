using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : Entity, IHealthable, IStatsable
{
    [SerializeField]
    private List<EntityStat> _stats;

    private Animator _animator;
    private Renderer[] _renderers;

    private MovementBehaviour _movementComponent;

    protected InteractingBehaviour Interactor { get; private set; }
    public EntityHealth HealthComponent { get; private set; }
    public BehaviourAnimation BehaviourAnimation { get; private set; }

    public override bool Interactable => HealthComponent.Alive;
    public IEnumerable<EntityStat> Stats => _stats;

    public EntityStat this[EntityStatsType stat] => _stats.First(s => s.Stat == stat);

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
        BehaviourAnimation = GetComponentInChildren<BehaviourAnimation>();


        InitializeHealth();
        InitializeInteractor();
    }

    protected virtual void InitializeHealth()
    {
        HealthComponent = GetComponent<EntityHealth>();

        HealthComponent.AddOnAliveChangeAction(alive =>
        {
            _movementComponent.ResetPath();
            foreach (var renderer in _renderers)
                renderer.enabled = alive;
        });

    }

    protected virtual void InitializeInteractor()
    {
        Interactor = GetComponent<InteractingBehaviour>();

        Interactor.AddOnTargetChangeAction(entity =>
        {
            if (entity != null)
                _movementComponent.TargetPosition = Interactor.TargetPosition;
        });

        Interactor.AddnPossibilityInteractChangeAction(can =>
        {
            if (can)
                _movementComponent.ResetPath();
            else if (!can && Interactor.Target != null)
                _movementComponent.TargetPosition = Interactor.TargetPosition;

        });


        Interactor.AddOnInteractFailedAction(() =>
        {
            _movementComponent.TargetPosition = Interactor.TargetPosition;
        });
    }

    public void MoveTo(Vector3 newPos)
    {
        if (!HealthComponent.Alive)
            return;
        Interactor.Target = null;
        _movementComponent.TargetPosition = newPos;
    }

    protected override void Interact(Entity entity)
    {
        if (!HealthComponent.Alive)
            return;

        if (entity is Unit unit)
        {

        }
    }
}