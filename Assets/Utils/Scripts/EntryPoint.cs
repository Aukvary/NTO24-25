using NTO24.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntryPoint : MonoBehaviour
    {
        [field: SerializeField]
        public Storage Storage { get; private set; }

        [SerializeField]
        private StorageHUD _storageHUD;

        [SerializeField]
        private UpgradeHUD _upgradeHUD;

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

        private List<Unit> _units = new();

        public IEnumerable<Unit> Units => _units;
        public IEnumerable<BeesSpawner> BeesSpawners => _beesSpawners;


        private void Awake()
        {
            _preinitializeEvent.Invoke();
            InitializeSpawners();
            InitializeStorage();

            BearActivityManager.Initialize(this);
            _postinitializeEvent.Invoke();
        }

        private void InitializeSpawners()
        {
            foreach (var bear in BearSpawner.Spawn())
                Add(bear);
        }

        private void InitializeStorage()
        {
            Storage.Initialize();
            _upgradeHUD.Initialize(Storage);
            _storageHUD.Initialize(Storage);
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