using NTO24.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntryPoint : MonoBehaviour
    {
        [field: SerializeField]
        public Storage Storage { get; private set; }

        [field: SerializeField]
        public  BearSpawner BearSpawner { get; private set; }

        [field: SerializeField]
        public  ControllableManager BearActivityManager { get; private set; }

        [SerializeField]
        private List<BeesSpawner> _beesSpawners;

        [SerializeField]
        private UnityEvent _preinitializeEvent;

        [SerializeField]
        private UnityEvent _postinitializeEvent;

        [SerializeField]
        private UnityEvent<Unit> _onUnitsCountChangeEvent;

        [Header("UI")]
        [SerializeField]
        private EntityHUD _entityHUD;

        [SerializeField]
        private StorageHUD _storageHUD;

        [SerializeField]
        private UpgradeHUD _upgradeHUD;

        private List<Unit> _units = new();

        private List<Bear> _bears = new();

        private EntitySelector _entitySelector;

        private UpgradeController _upgradeController;

        public IEnumerable<Unit> Units => _units;
        public IEnumerable<Bear> Bears => _bears;
        public IEnumerable<BeesSpawner> BeesSpawners => _beesSpawners;

        private void Awake()
        {
            _entitySelector = GetComponent<EntitySelector>();
            _upgradeController = GetComponent<UpgradeController>();
        }

        private void Start()
        {
            _preinitializeEvent.Invoke();
            Resources.Initialize();

            InitializeSpawners();
            InitializeStorage();
            _upgradeController.Initialize(_entitySelector, Storage, Bears.First());

            BearActivityManager.Initialize(this, _entitySelector);


            InitializeUI();
            _postinitializeEvent.Invoke();
        }

        private void InitializeSpawners()
        {
            foreach (var bear in BearSpawner.Spawn())
            {
                Add(bear);
                _bears.Add(bear);
            }
        }

        private void InitializeStorage()
        {
            Storage.Initialize();
        }

        public void InitializeUI()
        {
            _entityHUD.Initialize(_entitySelector);
            _storageHUD.Initialize(Storage);
            _upgradeHUD.Initialize(_upgradeController);
        }

        public void Add(Unit unit)
        {
            if (unit == null)
                return;

            _units.Add(unit);


            if (unit is not IRestoreable)
                unit.HealthController.AddOnDeathAction(e => _units.Remove(unit));
        }
    }
}