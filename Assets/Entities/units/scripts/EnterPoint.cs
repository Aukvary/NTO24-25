using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterPoint : MonoBehaviour
{
    private List<Unit> _units = new();

    [field: SerializeField]
    public  BearSpawner BearSpawner { get; private set; }

    [field: SerializeField]
    public Storage Storage { get; private set; }

    [field: SerializeField]
    public  ContollableActivityManager BearActivityManager { get; private set; }

    [SerializeField]
    private List<BeesSpawner> _beesSpawners;

    [SerializeField]
    private UnityEvent<Unit> _onUnitsCountChangeEvent;

    public IEnumerable<Unit> Units => _units;
    public IEnumerable<BeesSpawner> BeesSpawners => _beesSpawners;


    private void Awake()
    {
        InitializeSpawners();
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