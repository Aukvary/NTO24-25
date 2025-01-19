using UnityEngine;

public abstract class SpawnSettings : ScriptableObject
{
    [field: SerializeField]
    public Unit _unit { get; private set; }

    public abstract void Spawn(Vector3 spawnPosition);
}