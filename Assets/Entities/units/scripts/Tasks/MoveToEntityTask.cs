using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct MoveToEntityTask : UnitTask
{
    private Entity _target;
    private float _range;

    public Unit Unit { get; private set; }

    public bool IsComplete
    {
        get
        {
            Ray ray = new(Unit.transform.position, _target.transform.position - Unit.transform.position);

            var target = _target.transform;
            RaycastHit hit = Physics.RaycastAll(ray, _range).First(h => h.transform == target);

            return hit.transform != null;
        }
    }

    private Vector3 _targetPosition => _target.transform.position;

    public MoveToEntityTask(Unit unit, Entity target, float range)
    {
        Unit = unit;
        _target = target;
        _range = range;
    }

    public void Enter()
    {
        (Unit as IMovable).MoveTo(_targetPosition);
    }

    public void Exit()
    {
        (Unit as IMovable).Stop();
    }
}