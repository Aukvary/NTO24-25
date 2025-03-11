using System.Collections;
using UnityEngine;

namespace NTO24
{
    public class BeeSpawner : UnitSpawner
    {
        [SerializeField]
        private BeeActivityController _bee;

        [SerializeField]
        private Transform _spawnPosition;

        [SerializeField]
        private float _spawnCooldown;

        private int _level;

        public void StartSpawn(IHealthable burov)
        {
            StartCoroutine(Spawn(burov));
        }

        private IEnumerator Spawn(IHealthable burov)
        {
            while (true)
            {
                IStatsable bee = _bee.Spawn(_spawnPosition.position, burov);

                foreach (var stat in bee.Stats)
                    stat.CurrentLevel = _level;

                _level++;
            }
        }
    }
}