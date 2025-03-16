using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public class BreakableObject : Entity, IHealthable, IDropable, IStatsable, IRestoreable
    {
        public EntityHealth HealthController { get; private set; }

        public DropController DropController { get; private set; }

        public StatsController StatsController { get; private set; }

        public RestoreController RestoreController { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeHealth();
            StatsController = GetComponent<StatsController>();
            DropController = GetComponent<DropController>();
            RestoreController = GetComponent<RestoreController>();
        }

        private void InitializeHealth()
        {
            HealthController = GetComponent<EntityHealth>();

            HealthController.OnDeathEvent.AddListener(entity =>
            {
                if (entity is IInventoriable inventory)
                    DropController.Drop();

                RestoreController?.StartRestoring();
            });
        }
    }
}