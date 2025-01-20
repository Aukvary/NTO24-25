using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitsSpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnSettings> _spawnSettings;

    [SerializeField]
    private UnityEvent<Unit> _onSpawnEvent;

    [SerializeField]
    private List<Transform> _spawnPositions;

    protected IEnumerable<SpawnSettings> SpawnSettings => _spawnSettings;

    public IEnumerable<Vector3> SpawnPositions { get; private set; }

    protected virtual void Awake()
    {
        SpawnPositions = _spawnPositions.Select(t => t.position);
    }

    protected void InvokeOnSpawnEvent(Unit unit)
        => _onSpawnEvent?.Invoke(unit);

    public void AddOnSpawnAction(UnityAction<Unit> action)
        => _onSpawnEvent.AddListener(action);

    public void RemoveOnSpawnAction(UnityAction<Unit> action)
        => _onSpawnEvent.RemoveListener(action);

    public abstract void Spawn();
}