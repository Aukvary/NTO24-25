using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntryPoint : MonoBehaviour
{
    [field: SerializeField]
    public  BearSpawner BearSpawner { get; private set; }

    [field: SerializeField]
    public Storage Storage { get; private set; }

    [field: SerializeField]
    public  ControllableActivityManager BearActivityManager { get; private set; }

    [SerializeField]
    private List<BeesSpawner> _beesSpawners;

    [SerializeField]
    private UnityEvent _preinitializeEvent;

    [SerializeField]
    private UnityEvent _postinitializeEvent;

    [SerializeField]
    private UnityEvent<Unit> _onUnitsCountChangeEvent;

    private List<Unit> _units = new();

    public IEnumerable<Unit> Units => _units;
    public IEnumerable<BeesSpawner> BeesSpawners => _beesSpawners;


    private void Awake()
    {
        _preinitializeEvent.Invoke();
        InitializeSpawners();
        _postinitializeEvent.Invoke();
    }

    private void InitializeSpawners()
    {
        BearSpawner.AddOnSpawnAction(u => Add(u));

        foreach (BeesSpawner spawner in _beesSpawners)
            spawner.AddOnSpawnAction(u => Add(u));
    }



    public void Add(Unit unit)
    {
        if (unit == null)
            return;

        _units.Add(unit);


        if (unit is not IRestoreable)
            unit.HealthComponent.AddOnDeathAction(e => _units.Remove(unit));
    }
}