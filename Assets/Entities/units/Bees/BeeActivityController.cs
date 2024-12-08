using System.Linq;
using UnityEngine;

public class BeeActivityController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _agrRange;

    private Bee _unit;
    private BearActivityManager _manager;

    private BreakeableObject _durovHouse;

    private Vector3 _targetPosition;

    public Bee Spawn(Vector3 spawnPosition, BearActivityManager bearActivityManager, BreakeableObject durovHome)
    {
        var bee = Instantiate(this, spawnPosition, Quaternion.identity);

        bee._durovHouse = durovHome;
        bee._manager = bearActivityManager;

        Bee unit = bee.GetComponent<Bee>();

        bee._unit = unit;

        bee._manager.AddUnit(unit);

        return unit;
    }

    private void Update()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        float min = float.MaxValue;
        Bear bear = null;
        
        foreach (var u in _manager.Bears.Where(b => b.Alive))
        {
            var bMin = Mathf.Min(min, Vector3.Distance(u.transform.position, transform.position));
            if (bMin < min && u.Alive)
            {
                min = bMin;
                bear = u;
            }
        }
        if (_unit.Behaviour.Target is Bear b && b == bear)
        {
            return;
        }

        if (min <= _agrRange && bear.Alive)
        {
            _unit.InteractWith(bear);
            return;
        }

        if (_unit.Behaviour.Target == null)
            _unit.InteractWith(_durovHouse);
    }
}