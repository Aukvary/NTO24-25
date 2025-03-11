using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public class Bear : Unit, IRestoreable, IIconable, IControllable, IAttacker, IInventoriable, 
        IInteractor
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public Sprite[] Icon { get; private set; }

        public AttackController AttackController { get; private set; }
        public Inventory Inventory { get; private set; }
        public RestoreController RestoreController { get; private set; }
        public InteractingController InteractingController { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            HealthController.OnDeathEvent.AddListener(entity =>
            {
                RestoreController.StartRestoring();
            });

            AttackController = GetComponent<AttackController>();
            Inventory = GetComponent<Inventory>();
            Inventory.Initialize(6, (int)StatsController[StatNames.CellCapacity].StatValue);


            RestoreController = GetComponent<RestoreController>();
            InteractingController = GetComponent<InteractingController>();
        }
    }
}
