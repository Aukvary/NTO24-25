using System.Collections.Generic;
using UnityEngine;

public class BeesSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _spawnPositions;

    [SerializeField]
    private BearActivityManager _bearActivityManager;

    [SerializeField]
    private BeeActivityController _unit;

    [SerializeField]
    private BreakeableObject _durovHome;

    private void Start()
    {
        _unit.Spawn(_spawnPositions[0].position, _bearActivityManager, _durovHome);
    }
}