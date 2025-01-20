using UnityEngine;

public abstract class SpawnSettings : ScriptableObject
{
    public abstract Unit Spawn(Vector3 spawnPosition);
}