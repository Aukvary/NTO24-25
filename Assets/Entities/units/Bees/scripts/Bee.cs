using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public class Bee : Unit, IDropable, IAttacker
    {
        [SerializeField]
        private List<Pair<Resource, int>> _dropableItems;

        public IEnumerable<Pair<Resource, int>> DropableItems => _dropableItems;

        public AttackController AttackController { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AttackController = GetComponent<AttackController>();

            HealthController.OnDeathEvent.AddListener(entity =>
            {
                if (entity is IInventoriable inventory)
                    (this as IDropable).Drop(inventory);
            });
        }
    }
}