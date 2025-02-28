using UnityEngine;

namespace NTO24
{
    public class RestoreController : EntityComponent
    {
        [SerializeField]
        private float _restoreTime;

        private EntityHealth _healthController;

        private Collider _collider;
        private Renderer[] _renderers;
        private Vector3 _spawnPosition;

        protected override void Awake()
        {
            base.Awake();

            _collider = GetComponent<Collider>();
            _renderers = GetComponentsInChildren<Renderer>();

            _spawnPosition = transform.position;

            _healthController = GetComponent<EntityHealth>();

            _healthController.AddOnAliveChangeAction(alive =>
            {
                _collider.enabled = alive;
                transform.position = _spawnPosition;
                foreach (var renderer in _renderers) 
                    renderer.enabled = alive;
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