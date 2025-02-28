using System.Linq;
using UnityEngine;

namespace NTO24
{
    public struct FollowEntityTask : IUnitTask
    {
        private IMovable _unit;

        public Entity Target { get; private set; }
        public Entity Entity => _unit.EntityReference;

        public bool IsComplete => false;
        
        
        private bool _needToStop{
            get
            {
                Ray ray = new(Entity.transform.position, _targetPosition - Entity.transform.position);

                var target = Target.transform;
                RaycastHit hit = Physics.RaycastAll(ray, _unit.StopDistance).First(h => h.transform == target);

                return hit.transform != null;
            }
        }

        private Vector3 _targetPosition => Target.transform.position;

        public FollowEntityTask(IMovable unit, Entity target)
        {
            _unit = unit;
            Target = target;
        }

        public void Enter()
        {
            _unit.MoveTo(_targetPosition);
        }

        public void Update()
        {
            if (_needToStop)
                _unit.Stop();
            else
                _unit.MoveTo(_targetPosition);
        }

        public void Exit()
        {
            _unit.Stop();
        }
    }
}