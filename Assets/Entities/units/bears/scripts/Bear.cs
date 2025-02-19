using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bear : Unit, IInventoriable, IRestoreable, IIconable, IControllable
{
    private Collider _collider;
    private MeshRenderer[] _renderers;

    private Vector3 _spawnPosition;

    [field: SerializeField]
    public List<EntityStat> _stats;

    [field: SerializeField]
    public float RestoreTime { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public int CellCapacity { get; private set; }

    [field: SerializeField]
    public UnityEvent OnUserInitializeEvent { get; private set; }

    [field: SerializeField]
    public UnityEvent<ResourceCountPair> OnFailedAddEvent { get; private set; }


    public Animator Animator { get; private set; }

    public int CellCount => 6;

    public Inventory Inventory { get; private set; }

    public bool IsInitialized { get; set; }

    public User User { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        User = new User(Name);

        _renderers = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();

    }


    protected override void HealthInitialize()
    {
        base.HealthInitialize();
        HealthComponent.AddOnDeathAction(entity =>
        {
            (this as IRestoreable).StartRestoring();
        });

        HealthComponent.AddOnAliveChangeAction(alive =>
        {
            _collider.enabled = alive;
            transform.position = _spawnPosition;
            foreach (var renderer in _renderers)
                renderer.enabled = alive;
        });
    }
}
