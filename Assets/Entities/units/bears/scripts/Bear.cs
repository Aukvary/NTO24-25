using UnityEngine;

namespace NTO24
{
    public class Bear : Unit, IRestoreable, IIconable, IControllable, IAttacker, IInventoriable
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        public AttackController AttackController { get; private set; }
        public Inventory Inventory { get; private set; }
        public RestoreController RestoreController { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            AttackController = GetComponent<AttackController>();
            Inventory = GetComponent<Inventory>();
            RestoreController = GetComponent<RestoreController>();
        }

        protected override void HealthInitialize()
        {
            base.HealthInitialize();
            HealthController.AddOnDeathAction(entity =>
            {
                RestoreController.StartRestoring();
            });
        }
    }
}
