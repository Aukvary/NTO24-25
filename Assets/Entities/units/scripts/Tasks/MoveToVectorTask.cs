using System.Collections;
using UnityEngine;

namespace NTO24
{
    public class MoveToVectorTask : IUnitTask
    {
        private IMovable _unit;

        private bool _startMove;

        private Coroutine _startMoveAwaiter;

        public Entity Entity => _unit.EntityReference;

        public bool IsComplete => !_unit.HasPath && _startMove;

        public Vector3 Destination {  get; private set; }

        public MoveToVectorTask(IMovable unit, Vector3 destination)
        {
            _unit = unit;
            Destination = destination;
            _startMove = false;
            _startMoveAwaiter = null;
        }

        private IEnumerator StartMove()
        {
            while (!_unit.HasPath)
                yield return null;
            _startMove = true;

        }

        public void Enter()
        {
            _unit.MoveTo(Destination);
            _startMoveAwaiter = Entity.StartCoroutine(StartMove());

            if (Entity is IAnimationable animationable)
                animationable.SetAnimation(AnimationController.Animations.Move);
        }

        public void Exit()
        {
            if (_startMoveAwaiter != null)
                Entity.StopCoroutine(_startMoveAwaiter);
            _unit.Stop();
        }
    }
}