using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bear : Unit, IInventoriable, IRestoreable, IIconable, IControllable
{
    private Collider _collider;
    private MeshRenderer[] _renderers;

    private Vector3 _spawnPosition;

    private Coroutine _restoreCoroutine;

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

    protected override void Awake()
    {
        base.Awake();

        _renderers = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();

    }


    protected override void HealthInitialize()
    {
        base.HealthInitialize();
        HealthComponent.AddOnDeathAction(entity =>
        {
            _restoreCoroutine = (this as IRestoreable).StartRestoring();
        });

        HealthComponent.AddOnAliveChangeAction(alive =>
        {
            if (alive) 
                StopCoroutine(_restoreCoroutine);

            _collider.enabled = alive;
            transform.position = _spawnPosition;
            foreach (var renderer in _renderers)
                renderer.enabled = alive;
        });
    }
}
