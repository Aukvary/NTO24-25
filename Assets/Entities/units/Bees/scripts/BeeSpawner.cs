using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class BeeSpawner : UnitSpawner, ISavableComponent
    {
        [SerializeField]
        private BeeActivityController _bee;

        [SerializeField]
        private Transform _spawnPosition;

        [SerializeField]
        private float _spawnCooldown;

        private IHealthable _burov;

        private bool _startSpawning = false;

        private int _level;

        private int _time = 0;

        public string Name => "CyberApire";

        [HideInInspector]
        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        public string[] Data
            => new string[] { 
                _startSpawning.ToString(),
                _time.ToString(),
                _level.ToString()
            };

        public void Initialize(IHealthable burov)
        {
            _burov = burov;
            StartCoroutine(UpdateServerInfo());
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            _startSpawning = bool.Parse(data.ElementAt(0));
            _time = int.Parse(data.ElementAt(1));
            _level = int.Parse(data.ElementAt(2));


            if (_startSpawning)
                StartCoroutine(ServerSpawn(_time));
        }

        public void StartSpawn()
        {
            if (_startSpawning)
                return;
            
            StartCoroutine(Spawn());
        }

        private IEnumerator ServerSpawn(float time)
        {
            StartCoroutine(StartTimer(time));
            yield return new WaitForSeconds(time);
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            _startSpawning = true;
            while (true)
            {
                SpawnBee();
                _level++;

                StartCoroutine(StartTimer(_spawnCooldown));
                OnDataChangeEvent?.Invoke();
                yield return new WaitForSeconds(_spawnCooldown);
            }
        }

        public void SpawnBee()
        {
            IStatsable bee = _bee.Spawn(_spawnPosition.position, _burov);
            foreach (var stat in bee.Stats)
                stat.CurrentLevel = _level;
        }

        private IEnumerator StartTimer(float time)
        {
            _time = (int)time;

            while (_time > 0)
            {
                _time--;
                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator UpdateServerInfo()
        {
            while (true)
            {
                yield return new WaitForSeconds(60);
                OnDataChangeEvent.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SpawnBee();
        }
    }
}