using System.Linq;
using UnityEngine;

public class BeeActivityController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _agrRange;

    private Unit _unit;
    private BearActivityManager _manager;

    private Unit[] _bears;

    private BreakeableObject _durovHouse;

    private Vector3 _targetPosition;

    public void Spawn(Vector3 spawnPosition, BearActivityManager bearActivityManager, BreakeableObject durovHome)
    {
        var bee = Instantiate(this, spawnPosition, Quaternion.identity);

        bee._bears = bearActivityManager.Bears.ToArray();
        bee._durovHouse = durovHome;
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        if (_unit.AttackBehaviour.AttackedUnit != null)
            return;

        float min = float.MaxValue;
        Unit bear = null;
        
        foreach (var b in _bears)
        {
            var bMin = Mathf.Min(min, Vector3.Distance(b.transform.position, transform.position));
            if (bMin < min && b.Alive)
            {
                min = bMin;
                bear = b;
            }
        }

        if (min <= _agrRange)
        {
            _unit.Attack(bear);
            print("penis");
            return;
        }
        if (_unit.AttackBehaviour.BreakeableObject == null)
            _unit.Attack(_durovHouse);
    }
}