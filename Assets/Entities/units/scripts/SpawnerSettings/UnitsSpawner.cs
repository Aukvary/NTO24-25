using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UnitsSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnSettings _spawnSettings;

    [SerializeField]
    private List<Transform> _spawnPositions;

    public IEnumerable<Vector3> SpawnPositions { get; private set; }

    private void Awake()
    {
        SpawnPositions = _spawnPositions.Select(t => t.position);
    }

    public abstract void Spawn();
}