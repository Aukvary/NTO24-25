using System.Collections.Generic;
using UnityEngine;

public class BeesSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _spawnPositions;

    [SerializeField]
    private float _spawnCoolDown;

    [SerializeField]
    private BearActivityManager _bearActivityManager;

    [SerializeField]
    private BeeActivityController _unit;

    [SerializeField]
    private BreakeableObject _durovHome;

    private int _waspLevel = 0;

    private BreakeableObject _pairy;

    private void Awake()
    {
        _pairy = GetComponent<BreakeableObject>();
        _pairy.OnHitEvent += Spawn;
    }

    private void Start()
    {
        StartCoroutine(StartSpawn());
    }
    private System.Collections.IEnumerator StartSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnCoolDown);

            Spawn();

            _waspLevel++;
        }
    }

    private void Spawn()
    {

        var position = _spawnPositions[Random.Range(0, _spawnPositions.Count - 1)].position;
        var wasp = _unit.Spawn(position, _bearActivityManager, _durovHome);
        
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
            wasp.Upgrade(type, _waspLevel);
    }
}