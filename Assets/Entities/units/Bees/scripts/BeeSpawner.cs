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

        private bool _startSpawning;

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
            => _burov = burov;

        public void ServerInitialize(IEnumerable<string> data)
        {
            _startSpawning = bool.Parse(data.ElementAt(0));
            _time = int.Parse(data.ElementAt(1));
            _level = int.Parse(data.ElementAt(2));

            StartCoroutine(Spawn(_time));
        }

        public void StartSpawn()
        {
            if (_startSpawning)
                return;
            StartCoroutine(Spawn(_spawnCooldown));
        }

        private IEnumerator Spawn(float time)
        {
            _startSpawning = true;
            while (true)
            {
                yield return new WaitForSeconds(time);
                time = _spawnCooldown;


                IStatsable bee = _bee.Spawn(_spawnPosition.position, _burov);

                foreach (var stat in bee.Stats)
                    stat.CurrentLevel = _level;

                _level++;

                StartCoroutine(StartTimer());
            }
        }

        private IEnumerator StartTimer()
        {
            _time = (int)_spawnCooldown;

            while (_time > 0)
            {
                _time--;
                yield return new WaitForSeconds(1);
            }
        }
    }
}