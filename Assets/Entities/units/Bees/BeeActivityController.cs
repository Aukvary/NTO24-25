using System.Linq;
using UnityEngine;

public class BeeActivityController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _agrRange;

    private Unit _unit;
    private BearActivityManager _manager;

    private Unit[] _bears;

    private Unit _bear;

    private BreakeableObject _durovHome;

    private Vector3 _targetPosition;

    public void Spawn(Vector3 spawnPosition, BearActivityManager bearActivityManager, BreakeableObject durovHome)
    {
        var bee = Instantiate(this, spawnPosition, Quaternion.identity);

        bee._bears = bearActivityManager.Bears.ToArray(); 
        bee._durovHome = durovHome;
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        SetTarget();

        if (_bear != null)
            _unit.Attack(_bear);
    }

    private void SetTarget()
    {
        if (_bear == null)
        {
            float min = float.MaxValue;
            foreach (var bear in _bears)
                min = Mathf.Min(min, Vector3.Distance(bear.transform.position, transform.position));
            _bear = min >= _agrRange ? _bear : null;
        }

        if (_bear == null)
            _targetPosition = _durovHome.transform.position;
        else
            _targetPosition = _bear.transform.position;
    }
}