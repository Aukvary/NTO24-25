using UnityEngine;

namespace NTO24
{
    public class RestoreController : EntityComponent
    {
        [SerializeField]
        private float _restoreTime;

        private EntityHealth _healthController;

        private Collider _collider;
        private Vector3 _spawnPosition;

        protected override void Awake()
        {
            base.Awake();

            _collider = GetComponent<Collider>();

            _spawnPosition = transform.position;

            _healthController = GetComponent<EntityHealth>();

            _healthController.OnDeathEvent.AddListener(alive =>
            {
                _collider.enabled = alive;
                transform.position = _spawnPosition;
            });
        }

        public Coroutine StartRestoring()
            => StartCoroutine(Restore());
        
        private System.Collections.IEnumerator Restore()
        {
            yield return new WaitForSeconds(_restoreTime);
            _healthController.Alive = true;
        }
    }
}