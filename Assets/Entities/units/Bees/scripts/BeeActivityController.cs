using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace NTO24
{
    public class BeeActivityController : MonoBehaviour
    {
        private Bee _bee;

        private EntityTrigger _trigger;

        private IHealthable _target;

        private IHealthable _burov;

        private List<IHealthable> _enemies;

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
            Bee bee = Instantiate(_bee, position, Quaternion.identity);

            _burov = burov;

            return bee;
        }

        private void Awake()
        {
            _bee = GetComponent<Bee>();
            _trigger = GetComponentInChildren<EntityTrigger>();

            _trigger.OnEntityEnter.AddListener(e =>
            {
                if (e.EntityType == _bee.EntityType || e is not IHealthable health)
                    return;

                _enemies.Add(health);
                SetTarget();

                health.OnDeathEvent.AddListener(e => _enemies.Remove(health));
            });

            _trigger.OnEntityExit.AddListener(e =>
            {
                if (e is not IHealthable health || e.EntityType != _bee.EntityType)
                    return;

                if (_enemies.Contains(health))
                    _enemies.Remove(health);

                if (health == Target)
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
                (_bee as ITaskSolver).SetTask(new AttackTask(_bee, _enemies.First()));
            else if (Target != _burov)
                (_bee as ITaskSolver).SetTask(new AttackTask(_bee, _burov));
        }
    }
}