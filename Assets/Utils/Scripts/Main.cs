using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NTO24.UI;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class Main : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private TutorialController _tutorial;

        [SerializeField]
        private BearSpawner _bearSpawner;

        [SerializeField]
        private List<BeeSpawner> _beeSpawners;

        [SerializeField]
        private Storage Storage;

        [SerializeField]
        private Entity _burov;

        [SerializeField]
        private List<ResourceCluster> _resourceClusters;

        [SerializeField]
        private List<Pair<Resource, int>> _clusterCounts;

        [Header("UI")]
        [SerializeField]
        private EntityHUD _entityHUD;

        [SerializeField]
        private StorageHUD _storageHUD;

        [SerializeField]
        private UpgradeHUD _upgradeHUD;

        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent PreinitializeEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent PostinitializeEvent { get; private set; }

        [HideInInspector]
        public static Main Instance { get; private set; }
        private static List<IEnumerator> _requestes = new();

        private int _currentRequestCount = 0;

        private ControllableManager _bearActivityManager;

        private EntitySelector _entitySelector;

        private UpgradeController _upgradeController;

        public IEnumerable<BeeSpawner> BeeSpawners => _beeSpawners;

        private void Awake()
        {
            Instance = this;

            _bearActivityManager = GetComponent<ControllableManager>();
            _entitySelector = GetComponent<EntitySelector>();
            _upgradeController = GetComponent<UpgradeController>();
        }

        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return CallInitializeDepending<PreInitializeAttribute>();
            PreinitializeEvent.Invoke();

            InitializeClusters();

            InitializeSpawners();
            InitializeStorage();
            _upgradeController.Initialize(_entitySelector, Storage, Entity.GetEntites<Bear>().First());

            _bearActivityManager.Initialize(_entitySelector);


            InitializeUI();
            PostinitializeEvent.Invoke();

            yield return SendServerRequest();
            yield return CallInitializeDepending<PostInitializeAttribute>();

            yield return _tutorial.StartAdvicing(_entitySelector, _upgradeController);
            _beeSpawners.ForEach(s => s.StartSpawn());
        }

        private IEnumerator CallInitializeDepending<T>() where T : InitializeAttribute
        {
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(T))
                .GetTypes().Where(t => t.GetCustomAttribute<T>() != null);

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<T>();

                yield return (IEnumerator)type
                    .GetMethod(attribute.MethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                    .Invoke(null, null);
            }
        }

        private void InitializeSpawners()
        {
            _bearSpawner.Spawn();

            _beeSpawners.ForEach(s => s.Initialize(_burov as IHealthable));
        }

        private void InitializeStorage()
        {
            Storage.Initialize();
        }

        private void InitializeUI()
        {
            _entityHUD.Initialize(_entitySelector);
            _storageHUD.Initialize(Storage);
            _upgradeHUD.Initialize(_upgradeController);
        }

        public static void AddServerRequest(IEnumerator request)
            => _requestes.Add(request);

        private IEnumerator SendServerRequest()
        {
            if (_requestes.Count == 0)
                yield break;

            foreach (var coroutine in _requestes)
                StartCoroutine(SendRequest(coroutine));

            yield return new WaitUntil(() => _currentRequestCount == 0);
        }

        private IEnumerator SendRequest(IEnumerator request)
        {
            _currentRequestCount++;
            yield return request;
            _currentRequestCount--;
        }

        private void InitializeClusters()
        {
            var strSeed = SaveManager.Seed.ToString();

            int count = 0;
            if (!int.TryParse(strSeed[0].ToString(), out count))
                count = (strSeed[0] % 4) + 1;

            List<Resource> deniedResources = new();

            System.Random random = new System.Random(SaveManager.Seed);
            for (int i = 0, j = 1; i < _resourceClusters.Count; i++, j++)
            {
                Resource resourse = null;

                resourse = _resourceClusters[i].Spawn(SaveManager.Seed, j, count, deniedResources, random);

                for (int k = 0; k < _clusterCounts.Count; k++)
                {
                    if (_clusterCounts[k].Value1 == resourse)
                    {
                        _clusterCounts[k] = new(resourse, _clusterCounts[k].Value2 - 1);
                    }

                    if (_clusterCounts[k].Value2 == 0)
                        deniedResources.Add(_clusterCounts[k].Value1);
                }
            }
        }

        private void OnDestroy()
        {
            Entity.Clear();
        }
    }
}