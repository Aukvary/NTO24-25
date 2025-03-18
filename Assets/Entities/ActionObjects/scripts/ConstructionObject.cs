using Newtonsoft.Json;
using NTO24.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class ConstructionObject : Entity, IInteractable, ISavableComponent
    {
        [field: SerializeField]
        public UnityEvent OnBuiltEvent { get; private set; }

        [SerializeField]
        private List<Pair<Resource, int>> _materials;

        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        public Interactable Interactable { get; private set; }

        private ItemCellUI[] _itemCells;

        private bool _wasBuilt;

        public IEnumerable<Pair<Resource, int>> Materials => _materials;

        public string Name => "Construction";

        public string[] Data
            => new string[] {
                JsonConvert.SerializeObject(_materials),
                _wasBuilt.ToString()
            };

        public bool IsInteractable(IInteractor interactor)
        {
            if (interactor is not IInventoriable inventory)
                return false;

            bool containsResources = _materials.Any(pair =>
            {
                if (pair.Value2 == 0)
                    return true;
                return Storage.Resources[pair.Value1] > 0;
            });

            bool wasBuild = _materials.All(pair => pair.Value2 == 0);

            if (!containsResources)
                OnDataChangeEvent.Invoke();

            return containsResources && !wasBuild;
        }

        protected override void Awake()
        {
            base.Awake();

            Interactable = GetComponent<Interactable>();

            Interactable.OnInteractEvent.AddListener(Interact);
            _itemCells = GetComponentsInChildren<ItemCellUI>();
        }

        protected override void Start()
        {
            UpdateUI();
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            _materials = JsonConvert.DeserializeObject<List<Pair<Resource, int>>>(data.ElementAt(0));
            _wasBuilt = bool.Parse(data.ElementAt(1));

            if (_wasBuilt)
                OnBuiltEvent.Invoke();
        }

        private void Interact(IInteractor interactor)
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                if (Storage.Resources[_materials[i].Value1] == 0)
                    continue;

                if (_materials[i].Value2 == 0)
                    continue;

                Storage.Resources.RemoveResources(_materials[i].Value1, 1);
                _materials[i] = new(_materials[i].Value1, _materials[i].Value2 - 1);
            }

            if (_materials.All(p => p.Value2 == 0))
            {
                _wasBuilt = true;
                OnDataChangeEvent.Invoke();
                OnBuiltEvent.Invoke();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _itemCells.Length; i++)
            {
                if (i < _materials.Count)
                {
                    _itemCells[i].Source = _materials[i].Value2 > 0 ? _materials[i] : null;
                }
                else
                    _itemCells[i].Source = null;
            }
        }
    }
}