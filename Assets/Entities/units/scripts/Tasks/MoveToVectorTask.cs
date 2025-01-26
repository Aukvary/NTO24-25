using System.Collections;
using UnityEngine;

public struct MoveToVectorTask : UnitTask
{
    private Vector3 _destination;

    private bool _startMove;

    private Coroutine _startMoveAwaiter;

    public Unit Unit { get; private set; }

    public bool IsComplete => !(Unit as IMovable).HasPath && _startMove;

    public MoveToVectorTask(Unit unit, Vector3 destination)
    {
        Unit = unit;
        _destination = destination;
        _startMove = false;
        _startMoveAwaiter = null;
    }

    private IEnumerator StartMove()
    {
        while (!(Unit as IMovable).HasPath)
            yield return null;
        _startMove = true;
    }


    public void Enter()
    {
        (Unit as IMovable).MoveTo(_destination);
        _startMoveAwaiter = Unit.StartCoroutine(StartMove());
    }

    public void Exit()
    {
        Unit.StopCoroutine(_startMoveAwaiter);
        (Unit as IMovable).Stop();
    }
}