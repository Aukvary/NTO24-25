using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class BeeActivityController : MonoBehaviour
    {
        [SerializeField]
        private EntityTrigger _trigger;

        private Bee _bee;

        private IHealthable _target;

        private IHealthable _burov;

        private List<IHealthable> _enemies = new();

        public Bee Bee => _bee;

        public IHealthable Target
        {
            get => _target;

            private set
            {
                _target?.OnDeathEvent.RemoveListener(SetTarget);
                value?.OnDeathEvent.AddListener(SetTarget);

                _target = value;
            }
        }

        public Bee Spawn(Vector3 position, IHealthable burov)
        {
            BeeActivityController bee = Instantiate(this, position, Quaternion.identity);

            bee._burov = burov;

            return bee._bee;
        }

        private void Awake()
        {
            _bee = GetComponent<Bee>();

            _trigger.OnEntityEnter.AddListener(e =>
            {
                if (e.EntityType == _bee.EntityType || e is not Unit unit)
                    return;

                _enemies.Add(unit);

                SetTarget();

                unit.HealthController.OnDeathEvent.AddListener(e => _enemies.Remove(unit));
            });

            _trigger.OnEntityExit.AddListener(e =>
            {
                if (e.EntityType == _bee.EntityType || e is not Unit unit)
                    return;

                if (_enemies.Contains(unit))
                    _enemies.Remove(unit);

                if (unit as IHealthable == Target)
                {
                    Target = null;
                    SetTarget();
                }
            });
        }

        private void Start()
        {
            SetTarget();
        }

        private void SetTarget(Entity e = null)
        {
            if (_enemies.Any() && (Target == _burov || Target == null))
            {
                var target = _enemies.First();
                (_bee as ITaskSolver).SetTask(new AttackTask(_bee, target));
                Target = target;
            }
            else if (Target != _burov)
            {
                (_bee as ITaskSolver).SetTask(new AttackTask(_bee, _burov));
                Target = _burov;
            }
        }
    }
}