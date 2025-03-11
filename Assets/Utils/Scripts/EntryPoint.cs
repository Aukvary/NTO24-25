using NTO24.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private BearSpawner _bearSpawner;

        [SerializeField]
        private List<BeeSpawner> _beeSpawners;

        [SerializeField]
        private Storage Storage;

        [SerializeField]
        private Entity _burov;

        [Header("UI")]
        [SerializeField]
        private EntityHUD _entityHUD;

        [SerializeField]
        private StorageHUD _storageHUD;

        [SerializeField]
        private UpgradeHUD _upgradeHUD;

        [Header("Events")]
        [SerializeField]
        private UnityEvent _preinitializeEvent;

        [SerializeField]
        private UnityEvent _postinitializeEvent;

        private ControllableManager _bearActivityManager;

        private EntitySelector _entitySelector;

        private UpgradeController _upgradeController;

        public IEnumerable<BeeSpawner> BeeSpawners => _beeSpawners;

        private void Awake()
        {
            _bearActivityManager = GetComponent<ControllableManager>();
            _entitySelector = GetComponent<EntitySelector>();
            _upgradeController = GetComponent<UpgradeController>();
        }

        private void Start()
        {
            _preinitializeEvent.Invoke();

            InitializeSpawners();
            InitializeStorage();
            _upgradeController.Initialize(_entitySelector, Storage, Entity.GetEntites<Bear>().First());

            _bearActivityManager.Initialize(_entitySelector);


            InitializeUI();
            _postinitializeEvent.Invoke();
        }

        private void InitializeSpawners()
        {
            _bearSpawner.Spawn();
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