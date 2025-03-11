using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class Storage : Entity, IInteractable, IInventoriable
    {
        public Inventory Inventory { get; private set; }
        public Interactable Interactable { get; private set; }

        public static IInventoriable Resources { get; private set; }

        public void Initialize()
        {
            base.Awake();
            Inventory = GetComponent<Inventory>();

            Inventory.Initialize(Parser.ResourceNames.Count());

            Interactable = GetComponent<Interactable>();

            Interactable.OnInteractEvent.AddListener(i =>
            {
                if (i.EntityReference is not IInventoriable inventory)
                    throw new System.Exception($"entity {i.EntityReference.name} hasnt inventory");

                foreach (var item in inventory.GetItems())
                    Inventory.TryAddItems(item, out _);
            });
            Resources = this;
        }

        public bool IsInteractable(IInteractor interactor)
        {
            if (interactor is not IInventoriable inventory)
                throw new System.Exception($"entity {interactor.EntityReference.name} hasnt inventory");

            return inventory.HasItems;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                foreach (string name in Parser.ResourceNames)
                    Inventory.TryAddItems($"{name}:5".ToResources(), out _);
            }
        }
    }
}