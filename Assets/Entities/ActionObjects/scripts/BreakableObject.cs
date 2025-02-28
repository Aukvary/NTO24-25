using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class BreakableObject : Entity, IHealthable, IDropable, IStatsable, IRestoreable
    {
        [SerializeField]
        private List<Pair<Resource, int>> _dropableResources;

        [SerializeField]
        private UnityEvent<Entity, HealthChangeType> _onHealthChangeEvent;

        [SerializeField]
        private UnityEvent<Entity> _onBrokeEvent;

        public EntityHealth HealthController { get; private set; }

        public StatsController StatsController { get; private set; }

        public RestoreController RestoreController { get; private set; }

        public IEnumerable<Pair<Resource, int>> DropableItems => _dropableResources;

        protected override void Awake()
        {
            base.Awake();
            InitializeHealth();
            StatsController = GetComponent<StatsController>();
            RestoreController = GetComponent<RestoreController>();
        }

        private void InitializeHealth()
        {
            HealthController = GetComponent<EntityHealth>();

            HealthController.AddOnHealthChangeAction(_onHealthChangeEvent.Invoke);

            HealthController.AddOnDeathAction(entity =>
            {
                _onBrokeEvent.Invoke(entity);

                if (entity is IInventoriable inventory)
                    (this as IDropable).Drop(inventory);

                RestoreController?.StartRestoring();
            });
        }
    }
}