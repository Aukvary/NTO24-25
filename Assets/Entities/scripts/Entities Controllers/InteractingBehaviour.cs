using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InteractingBehaviour : EntityComponent
{
    [SerializeField]
    private UnityEvent<Entity> _onChangeTargetEvent;

    [SerializeField]
    private UnityEvent _onInteractFailedEvent;

    [SerializeField]
    private UnityEvent<bool> _onPossibilityInteractChangeEvent;

    private EntityStat _rangeStat;
    private EntityStat _interactStat;

    private Entity _target;

    private bool _canInteract;

    private UnityAction<Entity> _interactAction;

    public Entity Target => _target;

    public Vector3 TargetPosition => Target.transform.position;

    public bool CanInteract
    {
        get
        {
            if (!Target)
            {
                if (_canInteract)
                {
                    _canInteract = false;
                    _onPossibilityInteractChangeEvent.Invoke(false);
                }
                return false;
            }

            Ray ray = new(transform.position, TargetPosition - transform.position);

            var hit = Physics.RaycastAll(ray).First(h => h.transform == Target.transform);

            bool can = Vector3.Distance(transform.position, hit.point) < Range;

            if (can != _canInteract)
            { 
                _canInteract = can; 
                _onPossibilityInteractChangeEvent.Invoke(can); 
            }

            return can;
        }
    }

    public float Range => _rangeStat.StatValue;

    protected override void Awake()
    {
        base.Awake();
        if (Entity is IStatsable stats)
        {
            _rangeStat = stats[EntityStatsType.AttackRange];
            _interactStat = stats[EntityStatsType.InteractRange];
        }
        else throw new System.Exception("stats component was missed");

       /* if (Entity is IAnimationable entity)
            entity.BehaviourAnimation.OnPunchAnimationEvent += TryInteract;*/
    }

    public void SetTarget(Entity target, UnityAction<Entity> action)
    {
        _target = target;
        _interactAction = action;
    }

    public void ResetTarget()
    {
        _target = null;
        _interactAction = null;
    }

    public void TryInteract()
    {
        if (CanInteract)
            _interactAction?.Invoke(Target);
        else
            _onInteractFailedEvent.Invoke();
    }

    public void AddOnTargetChangeAction(UnityAction<Entity> action)
        => _onChangeTargetEvent.AddListener(action);
    public void RemoveOnTargetChangeAction(UnityAction<Entity> action)
        => _onChangeTargetEvent.RemoveListener(action);

    public void AddOnInteractFailedAction(UnityAction action)
        => _onInteractFailedEvent.AddListener(action);
    public void RemoveOnInteractFailedAction(UnityAction action)
        => _onInteractFailedEvent.RemoveListener(action);

    public void AddnPossibilityInteractChangeAction(UnityAction<bool> action)
        => _onPossibilityInteractChangeEvent.AddListener(action);
    public void RemoveOnInteractFailedAction(UnityAction<bool> action)
        => _onPossibilityInteractChangeEvent.RemoveListener(action);
}