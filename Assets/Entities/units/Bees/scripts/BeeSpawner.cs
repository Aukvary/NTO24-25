using UnityEngine;

namespace NTO24
{
    public class BeeSpawner : UnitSpawner
    {
        [SerializeField]
        private BeeActivityController _bee;

        [SerializeField]
        private Transform _spawnPosition;

        public void Spawn(IHealthable burov)
        {
            Bee bee = _bee.Spawn(_spawnPosition.position, burov);

            OnSpawnEvent.Invoke(bee);
        }
    }
}