using NTO24.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private Storage Storage;

        [SerializeField]
        private BearSpawner _bearSpawner;

        [SerializeField]
        private ControllableManager BearActivityManager;

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

        private EntitySelector _entitySelector;

        private UpgradeController _upgradeController;

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
            _upgradeController.Initialize(_entitySelector, Storage, Entity.GetEntites<Bear>().First());

            BearActivityManager.Initialize(this, _entitySelector);


            InitializeUI();
            _postinitializeEvent.Invoke();
        }

        private void InitializeSpawners()
        {
            _bearSpawner.OnSpawnEvent.AddListener(Entity.Add);
            _bearSpawner.Spawn();

            _beesSpawners.ForEach(s => s.OnSpawnEvent.AddListener(Entity.Add));
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

        private void OnDestroy()
        {
            Entity.Clear();
        }
    }
}