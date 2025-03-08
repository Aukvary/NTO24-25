using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class ConstructionObject : Entity, IInteractable
    {
        [field: SerializeField]
        public UnityEvent OnBuiltEvent { get; private set; }

        [SerializeField]
        private List<Pair<Resource, int>> _materials;

        public IEnumerable<Pair<Resource, int>> Materials => _materials;

        public Interactable Interactable { get; private set; }

        public bool IsInteractable(IInteractor interactor)
        {
            if (interactor is not IInventoriable inventory)
                return false;

            return _materials.Any(pair => inventory[pair.Value1] > 0);
        }

        protected override void Awake()
        {
            base.Awake();
            Interactable.OnInteractEvent.AddListener(Interact);
        }

        private void Interact(IInteractor interactor)
        {
            if (interactor.EntityReference is not IInventoriable inventory)
                return;

            for (int i = 0; i < _materials.Count; i++)
            {
                if (inventory[_materials[i].Value1] == 0)
                    continue;

                if (_materials[i].Value2 == 0)
                    continue;

                inventory.RemoveResources(_materials[i].Value1, 1);
                _materials[i] = new(_materials[i].Value1, _materials[i].Value2 - 1);
            }

            if (_materials.All(p => p.Value2 == 0))
                OnBuiltEvent.Invoke();
        }
    }
}