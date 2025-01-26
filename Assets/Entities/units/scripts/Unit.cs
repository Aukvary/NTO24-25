using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : Entity, IHealthable, IMovable, IStatsable, ITasker
{
    [SerializeField]
    private List<EntityStat> _stats;

    [field: SerializeField]
    public UnityEvent<Vector3> OnTargetPositionChangedEvent { get; private set; }

    public EntityHealth HealthComponent { get; private set; }

    public MovementBehaviour MovementController { get; private set; }

    public IEnumerable<EntityStat> Stats => _stats;

    public IEnumerable<UnitTask> Tasks { get; private set; }

    protected UnitTask CurrentTask { get; private set; }

    private Queue<UnitTask> _tasks => Tasks as Queue<UnitTask>;

    protected override void Awake()
    {
        base.Awake();

        Tasks = new Queue<UnitTask>();

        HealthInitialize();
        MovementInitialize();

    }

    protected virtual void HealthInitialize()
    {
        HealthComponent = GetComponent<EntityHealth>();
    }

    protected virtual void MovementInitialize()
    {
        MovementController = GetComponent<MovementBehaviour>();
    }

    protected override void Update()
    {
        CurrentTask?.Update();
    }

    protected override void FixedUpdate()
    {
        CurrentTask?.FixedUpdate();
    }

    public void SetTask(UnitTask task)
    {
        if (_tasks.TryPeek(out var t))
            t.Exit(this);
        _tasks.Clear();
        _tasks.Enqueue(task);
        task.Enter(this);
        CurrentTask = task;
    }

    public void AddTask(UnitTask task)
        => _tasks.Enqueue(task);

    protected UnitTask NextTask()
    {
        UnitTask task = _tasks.Dequeue();
        task.Exit(this);
        if (_tasks.TryPeek(out var t))
        {
            t.Enter(this);
            CurrentTask = t;
        }
        else
            CurrentTask = null;

        return task;
    }
}