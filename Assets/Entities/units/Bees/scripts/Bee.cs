using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public class Bee : Unit, IDropable
    {
        [SerializeField]
        private List<Pair<Resource, int>> _dropableItems;
        public IEnumerable<Pair<Resource, int>> DropableItems => _dropableItems;

        protected override void HealthInitialize()
        {
            base.HealthInitialize();
            HealthController.AddOnDeathAction(entity =>
            {
                if (entity is IInventoriable inventory)
                    (this as IDropable).Drop(inventory);
            });
        }
    }
}