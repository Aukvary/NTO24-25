using System.Collections;
using UnityEngine;

public struct MoveToVectorTask : IUnitTask
{
    private bool _startMove;

    private Coroutine _startMoveAwaiter;

    public Unit Unit { get; private set; }

    public bool IsComplete => !(Unit as IMovable).HasPath && _startMove;

    public Vector3 Destination {  get; private set; }

    public MoveToVectorTask(IMovable unit, Vector3 destination)
    {
        Unit = unit as Unit;
        Destination = destination;
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
        (Unit as IMovable).MoveTo(Destination);
        _startMoveAwaiter = Unit.StartCoroutine(StartMove());
    }

    public void Exit()
    {
        Unit.StopCoroutine(_startMoveAwaiter);
        (Unit as IMovable).Stop();
    }
}