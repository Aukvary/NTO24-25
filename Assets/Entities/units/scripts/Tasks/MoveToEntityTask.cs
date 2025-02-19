using System.Linq;
using UnityEngine;

public struct MoveToEntityTask : IUnitTask
{
    private float _range;

    public Entity Target { get; private set; }
    public Unit Unit { get; private set; }

    public bool IsComplete
    {
        get
        {
            Ray ray = new(Unit.transform.position, Target.transform.position - Unit.transform.position);

            var target = Target.transform;
            RaycastHit hit = Physics.RaycastAll(ray, _range).First(h => h.transform == target);

            return hit.transform != null;
        }
    }

    private Vector3 _targetPosition => Target.transform.position;

    public MoveToEntityTask(Unit unit, Entity target, float range)
    {
        Unit = unit;
        Target = target;
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