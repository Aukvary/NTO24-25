using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [System.Serializable]
    private class UnitTransformPair
    {
        public Unit Unit;
        public Transform Position;
    }

    [SerializeField]
    private List<UnitTransformPair> _units;

    [SerializeField]
    private BearActivityManager _activityManager;

    private Storage _storage;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        foreach (var bear in _units)
        {
            var unit = bear.Unit.Spawn(bear.Position.position, _storage);
            _activityManager.AddUnit(unit);
        }
    }
}